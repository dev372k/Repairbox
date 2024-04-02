using Microsoft.EntityFrameworkCore;
using RepairBox.BL.DTOs.CustomerIdentities;
using RepairBox.BL.DTOs.CustomerInfo;
using RepairBox.BL.DTOs.CustomerProductPurchase;
using RepairBox.BL.DTOs.DeviceInfo;
using RepairBox.BL.DTOs.Model;
using RepairBox.BL.DTOs.PurchaseFromCustomer;
using RepairBox.BL.DTOs.PurchaseFromCustomerInvoiceDTOs;
using RepairBox.Common.Helpers;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RepairBox.BL.Services
{
    public interface IPurchaseFromCustomerServiceRepo
    {
        public GetInvoiceDataDTO? GetPurchaseFromCustomerInvoice(long id);
        public PaginationModel GetPurchaseFromCustomerListings(int pageNo, DisplayPurchaseFromCustomerFiltersDTO? pfcFilters);
        public int AddPurchaseFromCustomer(AddCustomerInfoDTO customerInfoDTO, AddCustomerIdentitiesDTO customerIdentitiesDTO, AddDeviceInfoDTO deviceInfoDTO);
        public void AddPurchaseFromCustomerInvoice(AddPurchaseFromCustomerInvoiceDTO purchaseFromCustomerInvoiceDTO);
        public bool DeletePurchaseFromCustomer(long id);
        public GetPurchaseFromCustomerInvoiceDTO? UpdatePurchaseFromCustomer(UpdateCustomerInfoDTO customerInfoDTO, UpdateCustomerIdentitiesDTO customerIdentitiesDTO, UpdateDeviceInfoDTO deviceInfoDTO, long invoiceId);
    }
    public class PurchaseFromCustomerServiceRepo : IPurchaseFromCustomerServiceRepo
    {
        public ApplicationDBContext _context;
        public PurchaseFromCustomerServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public GetInvoiceDataDTO? GetPurchaseFromCustomerInvoice(long id)
        {
            var invoice = _context.PurchaseFromCustomerInvoices.FirstOrDefault(i => i.invoiceId == id);

            if (invoice == null) { return null; }

            if (invoice.IsDeleted == true || invoice.IsActive == false) { return null; }

            var customerInfo = _context.CustomerInfos.FirstOrDefault(c => c.Id == invoice.CustomerInfoId);
            var deviceInfo = _context.DeviceInfos.FirstOrDefault(d => d.CustomerInfoId == invoice.CustomerInfoId);

            var dto = new GetInvoiceDataDTO();
            dto.CustomerInfo = Omu.ValueInjecter.Mapper.Map<GetCustomerInfoDTO>(customerInfo);
            dto.DeviceInfo = Omu.ValueInjecter.Mapper.Map<GetDeviceInfoDTO>(deviceInfo);
            dto.PurchaseFromCustomerInvoice = Omu.ValueInjecter.Mapper.Map<GetPurchaseFromCustomerInvoiceDTO>(invoice);

            return dto;
        }
        public PaginationModel GetPurchaseFromCustomerListings(int pageNo, DisplayPurchaseFromCustomerFiltersDTO? pfcFilters)
        {
            var purchaseFromCustomerList = _context.PurchaseFromCustomerInvoices
                .Join(
                    _context.CustomerInfos,
                    invoice => invoice.CustomerInfoId,
                    customer => customer.Id,
                    (invoice, customer) => new { Invoice = invoice, Customer = customer }
                )
                .Join(
                    _context.DeviceInfos,
                    joinResult => joinResult.Customer.Id,
                    deviceInfo => deviceInfo.CustomerInfoId,
                    (joinResult, deviceInfo) => new { joinResult.Invoice, joinResult.Customer, DeviceInfo = deviceInfo }
                )
                .Where(result => 
                    result.Invoice.IsActive == true && 
                    result.Invoice.IsDeleted == false
                )
                .Select(result => new GetPurchaseFromCustomerDTO
                {
                    Id = result.Invoice.Id,
                    InvoiceId = result.Invoice.invoiceId,
                    CustomerName = result.Customer.Name,
                    CustomerEmail = result.Customer.Email,
                    CustomerPhone = result.Customer.PhoneNumber,
                    DeviceNameModel = result.DeviceInfo.DeviceNameModel,
                    DeviceIMEI = result.DeviceInfo.IMEI,
                    DeviceSerialNumber = result.DeviceInfo.SerialNumber,
                    DeviceCost = result.DeviceInfo.Cost,
                    DevicePrice = result.DeviceInfo.Price,
                    Date = result.Invoice.Date,
                    QRCodePath = result.Invoice.QRCodePath
                });

            if (pfcFilters == null)
            {
                var totalCount = purchaseFromCustomerList.Count();

                // Apply pagination
                purchaseFromCustomerList = purchaseFromCustomerList.Skip((pageNo - 1) * 10).Take(10);

                return new PaginationModel
                {
                    TotalPages = CommonHelper.TotalPagesforPagination(totalCount, 10),
                    CurrentPage = pageNo,
                    Data = purchaseFromCustomerList.ToList()
                };
            }
            else
            {
                // Apply search filter
                if (!string.IsNullOrEmpty(pfcFilters.searchKeyword))
                {
                    purchaseFromCustomerList = purchaseFromCustomerList.Where(dto =>
                        dto.CustomerName.Contains(pfcFilters.searchKeyword) ||
                        dto.CustomerEmail.Contains(pfcFilters.searchKeyword) ||
                        dto.DeviceNameModel.Contains(pfcFilters.searchKeyword) ||
                        dto.DeviceIMEI.Contains(pfcFilters.searchKeyword) ||
                        dto.DeviceSerialNumber.Contains(pfcFilters.searchKeyword) ||
                        dto.InvoiceId.ToString() == pfcFilters.searchKeyword
                    );
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(pfcFilters.sortBy))
                {
                    switch (pfcFilters.sortBy)
                    {
                        //case "Tracking":
                        //    purchaseFromCustomerList = pfcFilters.isSortAscending
                        //        ? purchaseFromCustomerList.OrderBy(dto => dto.InvoiceId)
                        //        : purchaseFromCustomerList.OrderByDescending(dto => dto.InvoiceId);
                        //    break;
                        case "Order Number":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.Id)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.Id);
                            break;
                        case "Invoice ID":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.InvoiceId)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.InvoiceId);
                            break;
                        case "Full name":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.CustomerName)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.CustomerName);
                            break;
                        case "Phone":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.CustomerPhone)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.CustomerPhone);
                            break;
                        case "Email":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.CustomerEmail)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.CustomerEmail);
                            break;
                        case "IMEI":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.DeviceIMEI)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.DeviceIMEI);
                            break;
                        case "Serial number":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.DeviceSerialNumber)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.DeviceSerialNumber);
                            break;
                        case "Created at":
                            purchaseFromCustomerList = pfcFilters.isSortAscending
                                ? purchaseFromCustomerList.OrderBy(dto => dto.Date)
                                : purchaseFromCustomerList.OrderByDescending(dto => dto.Date);
                            break;
                        //case "Updated at":
                        //    purchaseFromCustomerList = pfcFiltersisSortAscending
                        //        ? purchaseFromCustomerList.OrderBy(dto => dto.CustomerName)
                        //        : purchaseFromCustomerList.OrderByDescending(dto => dto.CustomerName);
                        //    break;
                        default:
                            throw new Exception("Invalid Sort By Value");
                    }
                }

                var totalCount = purchaseFromCustomerList.Count();

                // Apply pagination
                purchaseFromCustomerList = purchaseFromCustomerList.Skip((pageNo - 1) * pfcFilters.pageSize).Take(pfcFilters.pageSize);

                return new PaginationModel
                {
                    TotalPages = CommonHelper.TotalPagesforPagination(totalCount, pfcFilters.pageSize),
                    CurrentPage = pageNo,
                    Data = purchaseFromCustomerList.ToList()
                };
            }
        }
        public int AddPurchaseFromCustomer(AddCustomerInfoDTO customerInfoDTO, AddCustomerIdentitiesDTO customerIdentitiesDTO, AddDeviceInfoDTO deviceInfoDTO)
        {
            var customerInfo = new CustomerInfo
            {
                Name = customerInfoDTO.Name,
                Email = customerInfoDTO.Email,
                PhoneNumber = customerInfoDTO.PhoneNumber,
                Address = customerInfoDTO.Address,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _context.CustomerInfos.Add(customerInfo);
            _context.SaveChanges();

            var customerIdentities = new CustomerIdentities
            {
                CustomerInfoId = customerInfo.Id,
                Image = customerIdentitiesDTO.Image,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _context.CustomerIdentities.Add(customerIdentities);
            _context.SaveChanges();

            var deviceInfo = new DeviceInfo
            {
                CustomerInfoId = customerInfo.Id,
                DeviceNameModel = deviceInfoDTO.DeviceNameModel,
                IMEI = deviceInfoDTO.IMEI,
                SerialNumber = deviceInfoDTO.SerialNumber,
                Cost = deviceInfoDTO.Cost,
                Price = deviceInfoDTO.Price,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _context.DeviceInfos.Add(deviceInfo);
            _context.SaveChanges();

            return customerInfo.Id;
        }
        public void AddPurchaseFromCustomerInvoice(AddPurchaseFromCustomerInvoiceDTO purchaseFromCustomerInvoiceDTO)
        {
            var purchaseFromCustomerInvoice = new PurchaseFromCustomerInvoice
            {
                invoiceId = purchaseFromCustomerInvoiceDTO.invoiceId,
                CustomerInfoId = purchaseFromCustomerInvoiceDTO.CustomerInfoId,
                Date = purchaseFromCustomerInvoiceDTO.Date,
                QRCodePath = purchaseFromCustomerInvoiceDTO.QRCodePath,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _context.PurchaseFromCustomerInvoices.Add(purchaseFromCustomerInvoice);
            _context.SaveChanges();
        }
        public bool DeletePurchaseFromCustomer(long id)
        {
            var invoice = _context.PurchaseFromCustomerInvoices.FirstOrDefault(i => i.invoiceId == id);
            
            if(invoice == null) { return false; }

            invoice.IsDeleted = true;
            invoice.IsActive = false;

            var deviceInfo = _context.DeviceInfos.FirstOrDefault(d => d.CustomerInfoId == invoice.CustomerInfoId);
            deviceInfo.IsDeleted = true;
            deviceInfo.IsActive = false;

            var customerIdentities = _context.CustomerIdentities.FirstOrDefault(ci => ci.CustomerInfoId == invoice.CustomerInfoId);
            customerIdentities.IsDeleted = true;
            customerIdentities.IsActive = false;

            var customerInfo = _context.CustomerInfos.FirstOrDefault(c => c.Id == invoice.CustomerInfoId);
            customerInfo.IsDeleted = true;
            customerInfo.IsActive = false;

            _context.SaveChanges();
            return true;
        }
        public GetPurchaseFromCustomerInvoiceDTO? UpdatePurchaseFromCustomer(UpdateCustomerInfoDTO customerInfoDTO, UpdateCustomerIdentitiesDTO customerIdentitiesDTO, UpdateDeviceInfoDTO deviceInfoDTO, long invoiceId)
        {
            var invoice = _context.PurchaseFromCustomerInvoices.FirstOrDefault(i => i.invoiceId == invoiceId);

            if (invoice == null) { return null; }

            if (invoice.IsDeleted == true || invoice.IsActive == false) { return null; }

            var customerInfo = _context.CustomerInfos.FirstOrDefault(c => c.Id == invoice.CustomerInfoId);

            customerInfo.Name = customerInfoDTO.Name;
            customerInfo.Email = customerInfoDTO.Email;
            customerInfo.PhoneNumber = customerInfoDTO.PhoneNumber;
            customerInfo.Address = customerInfoDTO.Address;

            var customerIdentities = _context.CustomerIdentities.FirstOrDefault(ci => ci.CustomerInfoId == invoice.CustomerInfoId);

            customerIdentities.Image = customerIdentitiesDTO.Image;

            var deviceInfo = _context.DeviceInfos.FirstOrDefault(d => d.CustomerInfoId == invoice.CustomerInfoId);

            deviceInfo.DeviceNameModel = deviceInfoDTO.DeviceNameModel;
            deviceInfo.IMEI = deviceInfoDTO.IMEI;
            deviceInfo.SerialNumber = deviceInfoDTO.SerialNumber;
            deviceInfo.Cost = deviceInfoDTO.Cost;
            deviceInfo.Price = deviceInfoDTO.Price;

            _context.SaveChanges();

            return Omu.ValueInjecter.Mapper.Map<GetPurchaseFromCustomerInvoiceDTO>(invoice);
        }
    }
}
