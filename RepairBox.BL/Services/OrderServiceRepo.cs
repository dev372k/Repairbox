using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using RepairBox.BL.DTOs.Order;
using RepairBox.BL.DTOs.RepairDefect;
using RepairBox.BL.DTOs.Stripe;
using RepairBox.BL.DTOs.User;
using RepairBox.BL.ServiceModels.Order;
using RepairBox.Common.Commons;
using RepairBox.Common.Helpers;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface IOrderServiceRepo
    {
        CalculateOrderAmountDTO CalculateOrder(GetOrderChargesDTO model);
        Task<int> CreateOrder(AddOrderDTO model);
        PaginationModel GetOrderList(int pageNo, DisplayOrderFiltersDTO? orderFilters);
        GetOrderDTO? GetOrderById(int orderId);
        List<GetOrderDTO>? GetOrdersByTechnician(int technicianId);
        (bool, string) UpdateOrderTechnician(UpdateOrderTechnicianDTO model);
        (bool, string) UpdateOrderStatus(UpdateOrderStatusDTO model);
        (bool, string, TrackOrderDTO) GetOrderStatus(int orderId);
    }

    public class OrderServiceRepo : IOrderServiceRepo
    {
        public ApplicationDBContext _context;
        public IStripeService _stripeRepo;

        private int pageSize = DeveloperConstants.PAGE_SIZE;
        public OrderServiceRepo(ApplicationDBContext context, IStripeService stripeRepo)
        {
            _context = context;
            _stripeRepo = stripeRepo;

        }
        public CalculateOrderAmountDTO CalculateOrder(GetOrderChargesDTO model)
        {
            try
            {
                decimal totalCosts = 0;
                decimal subTotal = 0;
                decimal taxPercentage = 0;

                var defects = _context.RepairableDefects.Where(d => model.Defects.Contains(d.Id)).ToList();

                defects.ForEach(d => subTotal += d.Price);
                defects.ForEach(d => totalCosts += d.Cost);

                var setting = _context.Settings.FirstOrDefault();
                if (setting != null)
                    taxPercentage = setting.Tax;

                var priorityProcessCharges = _context.RepairPriorities.FirstOrDefault(p => p.Id == model.PriorityId)?.ProcessCharges;

                decimal totalAmount = subTotal + priorityProcessCharges.Value;

                decimal totalAmountWithoutTax = Math.Floor((totalAmount / (1 + (taxPercentage / 100))) * 100) / 100;

                decimal taxAmount = totalAmount - totalAmountWithoutTax;

                decimal profit = totalAmountWithoutTax - totalCosts;

                return new CalculateOrderAmountDTO
                {
                    SubTotal = subTotal,
                    Tax = taxAmount,
                    PriorityProcessCharges = priorityProcessCharges.Value,
                    Profit = profit,
                    TotalAmount = totalAmount
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> CreateOrder(AddOrderDTO model)
        {
            int orderId = -1;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (model.PaymentMethod == enPaymentMethod.Card)
                {
                    var stripeResponse = await _stripeRepo.CreateCharge(new StripeRequestDTO
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone,
                        CardNumber = model.CardNumber,
                        ExpiryMonth = model.ExpiryMonth,
                        ExpiryYear = model.ExpiryYear,
                        CVV = model.CVV,
                        CardType = model.CardType,
                        Amount = model.Amount
                    });
                    if (stripeResponse.Status == enStripeChargeStatus.succeeded)
                    {
                        AddTransaction(new Transaction
                        {
                            Amount = model.Amount,
                            CardMask = model.CardNumber.Substring(model.CardNumber.Length - 4),
                            CardType = model.CardType,
                            Email = model.Email,
                            CreatedAt = DateTime.UtcNow,
                            StripeCustomerId = stripeResponse.StripeCustomerId,
                            StripeTransactionId = stripeResponse.StripeTransactionId,
                            IsActive = true,
                            IsDeleted = false
                        });

                        orderId = AddOrder(model);
                        AddOrderDefects(model.RepairableDefects, orderId);
                    }
                }
                else
                {
                    orderId = AddOrder(model);
                    AddOrderDefects(model.RepairableDefects, orderId);
                }
                transaction.Commit();
                return orderId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public PaginationModel GetOrderList(int pageNo, DisplayOrderFiltersDTO? orderFilters)
        {
            var orders = _context.Orders.ToList();
            var models = _context.Models.Where(m => orders.Select(o => o.ModelId).Contains(m.Id)).ToList();
            var brands = _context.Brands.Where(b => models.Select(m => m.BrandId).Contains(b.Id)).ToList();
            var users = _context.Users.Where(u => orders.Select(o => o.TechnicianId).Contains(u.Id)).ToList();
            var priorities = _context.RepairPriorities.Where(rp => orders.Select(o => o.PriorityId).Contains(rp.Id)).ToList();
            var statuses = _context.RepairStatuses.Where(rs => orders.Select(o => o.StatusId).Contains(rs.Id)).ToList();
            var orders_defects = _context.OrderDefects.Where(od => orders.Select(o => o.Id).Contains(od.Id)).ToList();
            var all_defects = _context.RepairableDefects.Where(rd => orders_defects.Select(od => od.RepairableDefectId).Contains(rd.Id)).ToList();

            var orderList = new List<GetOrderDTO>();

            foreach (var order in orders)
            {
                var brand = brands.FirstOrDefault(b => b.Id == models.FirstOrDefault(m => m.Id == order.ModelId)?.BrandId);
                var model = models.FirstOrDefault(m => m.Id == order.ModelId);
                var technician = users.FirstOrDefault(u => u.Id == order.TechnicianId && order.TechnicianId != null);
                var priority = priorities.FirstOrDefault(rp => rp.Id == order.PriorityId);
                var status = statuses.FirstOrDefault(rs => rs.Id == order.StatusId);
                var order_defects = orders_defects.Where(od => od.OrderId == order.Id).ToList();
                var defects = all_defects.Where(rd => order_defects.Select(od => od.RepairableDefectId).Contains(rd.Id)).ToList();

                var defectsDTO = defects.Select(defect => new GetDefectForOrderDTO
                {
                    Id = defect.Id,
                    Name = defect.DefectName,
                    RepairTime = defect.RepairTime,
                    Cost = defect.Cost,
                    Price = defect.Price
                }).ToList();

                orderList.Add(new GetOrderDTO
                {
                    Id = order.Id,
                    Name = order.Name,
                    Email = order.Email,
                    Phone = order.Phone,
                    SerialNumber = order.SerialNumber,
                    Address = order.Address,
                    Diagnostics = order.Diagnostics,
                    BrandName = brand?.Name ?? "",
                    ModelId = order.ModelId,
                    ModelName = model?.Name ?? "",
                    TechnicianId = order.TechnicianId,
                    TechnicianName = technician?.Username ?? "",
                    PriorityId = order.PriorityId,
                    PriorityName = priority?.Name ?? "",
                    StatusId = order.StatusId,
                    StatusName = status?.Name ?? "",
                    WarrantyStatus = order.WarrantyStatus,
                    CreatedAt = order.CreatedAt,
                    isPaid = order.IsPaid,
                    TotalAmount = order.TotalAmount,
                    AmountPaid = order.IsPaid ? 0.0M : order.TotalAmount,
                    Defects = defectsDTO
                });
            }

            for (int i = 0; i < orderList.Count; i++)
            {
                orderList[i].OrderAmount = CalculateOrder(
                    new GetOrderChargesDTO
                    {
                        Defects = _context.OrderDefects.Where(od => od.OrderId == orderList[i].Id).Select(od => od.RepairableDefectId).ToList(),
                        PriorityId = orderList[i].PriorityId
                    });
            }

            var TotalPages = 0;

            if(orderFilters == null)
            {
                TotalPages = orderList.Count;
                orderList = orderList.Skip((pageNo - 1) * 10).Take(10).ToList();

                return new PaginationModel
                {
                    TotalPages = CommonHelper.TotalPagesforPagination(TotalPages, 10),
                    CurrentPage = pageNo,
                    Data = orderList
                };
            }
            else
            {
                // Apply search filter
                if (!string.IsNullOrEmpty(orderFilters.searchKeyword))
                {
                    orderList = orderList.Where(dto =>
                        dto.Name.Contains(orderFilters.searchKeyword) ||
                        dto.Email.Contains(orderFilters.searchKeyword) ||
                        dto.Phone.Contains(orderFilters.searchKeyword) ||
                        dto.SerialNumber.Contains(orderFilters.searchKeyword) ||
                        dto.Address.Contains(orderFilters.searchKeyword) ||
                        dto.BrandName.Contains(orderFilters.searchKeyword) ||
                        dto.ModelName.Contains(orderFilters.searchKeyword) ||
                        dto.TechnicianName.Contains(orderFilters.searchKeyword) ||
                        dto.Id.ToString() == orderFilters.searchKeyword
                    ).ToList();
                }

                // Apply Technician Filter
                if(orderFilters.technicianId != null)
                {
                    orderList = orderList.Where(dto => dto.TechnicianId == orderFilters.technicianId).ToList();
                }

                // Apply Payment Filter
                if (orderFilters.isPaid != null)
                {
                    orderList = orderList.Where(dto => dto.isPaid == orderFilters.isPaid).ToList();
                }

                //// Apply Lock state Filter
                //if (orderFilters.isLocked != null)
                //{
                //    orderList = orderList.Where(dto => dto.).ToList();
                //}

                //// Apply Lock state Filter
                //if (orderFilters.isLocked != null)
                //{
                //    orderList = orderList.Where(dto => dto.).ToList();
                //}

                // Apply Warranty Filter
                if (orderFilters.hasWarranty != null)
                {
                    orderList = orderList.Where(dto => dto.WarrantyStatus == orderFilters.hasWarranty).ToList();
                }

                // Apply Status Filter
                if (orderFilters.statusId != null)
                {
                    orderList = orderList.Where(dto => dto.StatusId == orderFilters.statusId).ToList();
                }

                // Apply Priority Filter
                if (orderFilters.priorityId != null)
                {
                    orderList = orderList.Where(dto => dto.PriorityId == orderFilters.priorityId).ToList();
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(orderFilters.sortBy))
                {
                    switch (orderFilters.sortBy)
                    {
                        case "Status":
                            orderList = orderFilters.isSortAscending
                                ? orderList.OrderBy(dto => dto.StatusId).ToList()
                                : orderList.OrderByDescending(dto => dto.StatusId).ToList();
                            break;
                        case "Priority":
                            orderList = orderFilters.isSortAscending
                                ? orderList.OrderBy(dto => dto.PriorityId).ToList()
                                : orderList.OrderByDescending(dto => dto.PriorityId).ToList();
                            break;
                        case "Technician":
                            orderList = orderFilters.isSortAscending
                                ? orderList.OrderBy(dto => dto.TechnicianId).ToList()
                                : orderList.OrderByDescending(dto => dto.TechnicianId).ToList();
                            break;
                        case "Created at":
                            orderList = orderFilters.isSortAscending
                                ? orderList.OrderBy(dto => dto.CreatedAt).ToList()
                                : orderList.OrderByDescending(dto => dto.CreatedAt).ToList();
                            break;
                        case "Order Number":
                            orderList = orderFilters.isSortAscending
                                ? orderList.OrderBy(dto => dto.Id).ToList()
                                : orderList.OrderByDescending(dto => dto.Id).ToList();
                            break;
                        //case "Updated At":
                        //    orderList = orderFilters.isSortAscending
                        //        ? orderList.OrderBy(dto => dto.UpdatedAt).ToList()
                        //        : orderList.OrderByDescending(dto => dto.UpdatedAt).ToList();
                        //    break;
                        //case "Closed At":
                        //    orderList = orderFilters.isSortAscending
                        //        ? orderList.OrderBy(dto => dto.ClosedAt).ToList()
                        //        : orderList.OrderByDescending(dto => dto.ClosedAt).ToList();
                        //    break;

                        default:
                            throw new Exception("Invalid Sort By Value");
                    }
                }

                TotalPages = orderList.Count;
                orderList = orderList.Skip((pageNo - 1) * 10).Take(orderFilters.pageSize).ToList();

                return new PaginationModel
                {
                    TotalPages = CommonHelper.TotalPagesforPagination(TotalPages, orderFilters.pageSize),
                    CurrentPage = pageNo,
                    Data = orderList
                };
            }
        }
        public GetOrderDTO? GetOrderById(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if(order == null) { return null; }

            var model = _context.Models.FirstOrDefault(m => order.ModelId == m.Id);
            var brand = model?.BrandId != null ? _context.Brands.FirstOrDefault(b => model.BrandId == b.Id) : null;
            var user = order.TechnicianId != null ? _context.Users.FirstOrDefault(u => order.TechnicianId == u.Id) : null;
            var priority = _context.RepairPriorities.FirstOrDefault(rp => order.PriorityId == rp.Id);
            var status = _context.RepairStatuses.FirstOrDefault(rs => order.StatusId == rs.Id);
            var order_defects = _context.OrderDefects.Where(od => od.OrderId == order.Id).Select(od => od.RepairableDefectId).ToList();
            var defects = _context.RepairableDefects.Where(rd => order_defects.Contains(rd.Id)).ToList();

            var defectsDTO = defects.Select(defect => new GetDefectForOrderDTO
            {
                Id = defect.Id,
                Name = defect.DefectName,
                RepairTime = defect.RepairTime,
                Cost = defect.Cost,
                Price = defect.Price
            }).ToList();

            var orderDTO = new GetOrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                Email = order.Email,
                Phone = order.Phone,
                SerialNumber = order.SerialNumber,
                Address = order.Address,
                Diagnostics = order.Diagnostics,
                BrandName = brand?.Name ?? "",
                ModelId = order.ModelId,
                ModelName = model?.Name ?? "",
                TechnicianId = order.TechnicianId,
                TechnicianName = user?.Username ?? "",
                PriorityId = order.PriorityId,
                PriorityName = priority?.Name ?? "",
                StatusId = order.StatusId,
                StatusName = status?.Name ?? "",
                WarrantyStatus = order.WarrantyStatus,
                CreatedAt = order.CreatedAt,
                isPaid = order.IsPaid,
                TotalAmount = order.TotalAmount,
                AmountPaid = order.IsPaid ? 0.0M : order.TotalAmount,
                Defects = defectsDTO
            };

            orderDTO.OrderAmount = CalculateOrder(
                new GetOrderChargesDTO
                {
                    Defects = _context.OrderDefects.Where(od => od.OrderId == orderDTO.Id).Select(od => od.RepairableDefectId).ToList(),
                    PriorityId = orderDTO.PriorityId
                });

            return orderDTO;
        }
        public List<GetOrderDTO>? GetOrdersByTechnician(int technicianId)
        {
            var orderList = _context.Orders.Where(o => o.TechnicianId == technicianId).ToList();

            if (orderList.Count == 0) { return null; }

            var orders = new List<GetOrderDTO>();

            for (int i = 0; i < orderList.Count; i++)
            {
                var model = _context.Models.FirstOrDefault(m => orderList[i].ModelId == m.Id);
                var brand = model?.BrandId != null ? _context.Brands.FirstOrDefault(b => model.BrandId == b.Id) : null;
                var user = _context.Users.FirstOrDefault(u => orderList[i].TechnicianId == u.Id);
                var priority = _context.RepairPriorities.FirstOrDefault(rp => orderList[i].PriorityId == rp.Id);
                var status = _context.RepairStatuses.FirstOrDefault(rs => orderList[i].StatusId == rs.Id);
                var order_defects = _context.OrderDefects.Where(od => od.OrderId == orderList[i].Id).Select(od => od.RepairableDefectId).ToList();
                var defects = _context.RepairableDefects.Where(rd => order_defects.Contains(rd.Id)).ToList();

                var defectsDTO = defects.Select(defect => new GetDefectForOrderDTO
                {
                    Id = defect.Id,
                    Name = defect.DefectName,
                    RepairTime = defect.RepairTime,
                    Cost = defect.Cost,
                    Price = defect.Price
                }).ToList();

                var orderDTO = new GetOrderDTO
                {
                    Id = orderList[i].Id,
                    Name = orderList[i].Name,
                    Email = orderList[i].Email,
                    Phone = orderList[i].Phone,
                    SerialNumber = orderList[i].SerialNumber,
                    Address = orderList[i].Address,
                    Diagnostics = orderList[i].Diagnostics,
                    BrandName = brand?.Name ?? "",
                    ModelId = orderList[i].ModelId,
                    ModelName = model?.Name ?? "",
                    TechnicianId = orderList[i].TechnicianId,
                    TechnicianName = user?.Username ?? "",
                    PriorityId = orderList[i].PriorityId,
                    PriorityName = priority?.Name ?? "",
                    StatusId = orderList[i].StatusId,
                    StatusName = status?.Name ?? "",
                    WarrantyStatus = orderList[i].WarrantyStatus,
                    CreatedAt = orderList[i].CreatedAt,
                    isPaid = (orderList[i].PaymentMethod == enPaymentMethod.CoD) ? false : true,
                    TotalAmount = orderList[i].TotalAmount,
                    AmountPaid = (orderList[i].PaymentMethod == enPaymentMethod.CoD) ? 0.0M : orderList[i].TotalAmount,
                    Defects = defectsDTO
                };

                orderDTO.OrderAmount = CalculateOrder(
                    new GetOrderChargesDTO
                    {
                        Defects = _context.OrderDefects.Where(od => od.OrderId == orderDTO.Id).Select(od => od.RepairableDefectId).ToList(),
                        PriorityId = orderDTO.PriorityId
                    });

                orders.Add(orderDTO);
            }

            return orders;
        }
        public (bool, string) UpdateOrderTechnician(UpdateOrderTechnicianDTO model)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == model.OrderId);
            if (order == null) { return (false, "Order"); }

            var technician = _context.Users.FirstOrDefault(u => u.Id == model.TechnicianId);
            if(technician == null) { return (false, "Technician"); }

            order.TechnicianId = model.TechnicianId;
            _context.SaveChanges();

            return (true, "");
        }
        public (bool, string) UpdateOrderStatus(UpdateOrderStatusDTO model)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == model.OrderId);
            if (order == null) { return (false, "Order"); }

            var status = _context.RepairStatuses.FirstOrDefault(rs => rs.Id == model.StatusId);
            if (status == null) { return (false, "Repair Status"); }

            order.StatusId = model.StatusId;
            if (status.Name.ToLower() == "completed")
                order.IsPaid = true;
            else order.IsPaid = false;
            _context.SaveChanges();

            return (true, "");
        }

        public (bool, string, TrackOrderDTO) GetOrderStatus(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) { return (false, "Order", new TrackOrderDTO()); }

            var status = _context.RepairStatuses.FirstOrDefault(rs => rs.Id == order.StatusId);
            if (status == null) { return (false, "Repair Status", new TrackOrderDTO()); }

            return (true, "", new TrackOrderDTO
            {
                Name = order.Name,
                Email = order.Email,
                Phone = order.Phone,
                isPaid = order.IsPaid ? "Yes" : "No",
                TotalAmount = order.TotalAmount,
                Status = status.Name
            });
        }


        // Private Method
        private void AddTransaction(Transaction transaction)
        {
            try
            {
                _context.Transactions.Add(new Transaction
                {
                    Amount = transaction.Amount,
                    CardMask = transaction.CardMask,
                    CardType = transaction.CardType,
                    Email = transaction.Email,
                    CreatedAt = transaction.CreatedAt,
                    StripeCustomerId = transaction.StripeCustomerId,
                    StripeTransactionId = transaction.StripeTransactionId,
                    IsActive = true,
                    IsDeleted = false
                });
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int AddOrder(AddOrderDTO model)
        {
            try
            {
                var orderAmount = CalculateOrder(new GetOrderChargesDTO
                {
                    Defects = model.RepairableDefects,
                    PriorityId = model.PriorityId
                });

                var order = new Order
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    CreatedAt = DateTime.Now,
                    Diagnostics = model.Diagnostics,
                    PaymentMethod = model.PaymentMethod,
                    ModelId = model.ModelId,
                    PriorityId = model.PriorityId,
                    StatusId = 0,
                    WarrantyStatus = model.WarrantyStatus,
                    SerialNumber = model.SerialNumber,
                    IsActive = true,
                    IsDeleted = false,
                    SubTotal = orderAmount.SubTotal,
                    TotalAmount = orderAmount.TotalAmount,
                    TaxAmount = orderAmount.Tax,
                    Profit = orderAmount.Profit
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

                return order.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddOrderDefects(List<int> Defects, int OrderId)
        {
            foreach (var defect in Defects)
            {
                _context.OrderDefects.Add(new OrderDefect
                {
                    OrderId = OrderId,
                    RepairableDefectId = defect,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                });

                _context.SaveChanges();
            }
        }
    }
}
