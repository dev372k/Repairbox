using Microsoft.AspNetCore.Http;
using RepairBox.BL.DTOs.Dashboard;
using RepairBox.BL.DTOs.Order;
using RepairBox.Common.Commons;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface IDashboardServiceRepo
    {
        GetDashboardDTO GetDashboardData();
        GetDashboardDTO GetDashboardData(DisplayDashboardFiltersDTO dashboardFilters);
    }
    public class DashboardServiceRepo : IDashboardServiceRepo
    {
        private ApplicationDBContext _context;
        public DashboardServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public GetDashboardDTO GetDashboardData()
        {
            var orderList = _context.Orders
                .Select(result => new 
                {
                    Id = result.Id,
                    PriorityId = result.PriorityId,
                    SubTotal = result.SubTotal,
                    TaxAmount = result.TaxAmount,
                    Profit = result.Profit,
                    TotalAmount = result.TotalAmount
                })
                .ToList();

            decimal TotalAmount = 0.0M;
            decimal TotalCost = 0.0M;
            decimal TotalTax = 0.0M;
            decimal TotalProfit = 0.0M;

            for (int i = 0; i < orderList.Count; i++)
            {
                TotalAmount += orderList[i].TotalAmount;
                TotalTax += orderList[i].TaxAmount;
                TotalProfit += orderList[i].Profit;
            }

            TotalCost = TotalAmount - (TotalTax + TotalProfit);

            int brandsCount = _context.Brands.Select(brand => brand.Id).Count();
            int deviceCount = _context.Models.Select(model => model.Id).Count();
            int defectsCount = _context.RepairableDefects.Select(defect => defect.Id).Count();

            var now = DateTime.Now;
            var twelveMonthsAgo = now.AddMonths(-12);

            var annualRepairOrders = _context.Orders
                    .Where(order => order.CreatedAt >= twelveMonthsAgo)
                    .GroupBy(order => new { order.CreatedAt.Year, order.CreatedAt.Month })
                    .Select(group => new { group.Key.Year, group.Key.Month, order_count = group.Count() })
                    .OrderBy(group => group.Year)
                    .ThenBy(group => group.Month)
                    .ToList();

            var annualRepairOrdersList = new List<AnnualRepairOrder>();

            foreach (var order in annualRepairOrders)
            {
                annualRepairOrdersList.Add(new AnnualRepairOrder
                {
                    Year = order.Year,
                    Month = order.Month,
                    OrderCount = order.order_count
                });
            }

            var dashboard = new GetDashboardDTO
            {
                Amount = TotalAmount,
                Cost = TotalCost,
                Tax = TotalTax,
                Profit = TotalProfit,
                RepairOrders = orderList.Count,
                Brands = brandsCount,
                Devices = deviceCount,
                Defects = defectsCount,
                AnnualRepairOrders = annualRepairOrdersList
            };

            return dashboard;
        }
        public GetDashboardDTO GetDashboardData(DisplayDashboardFiltersDTO dashboardFilters)
        {
            var orderList = _context.Orders.ToList();

            if(dashboardFilters.isPaid != null)
            {
                var paymentMethod = (bool)dashboardFilters.isPaid ? enPaymentMethod.Card : enPaymentMethod.CoD;
                orderList = orderList.Where(o => o.PaymentMethod == paymentMethod).ToList();
            }

            if(dashboardFilters.statusId != null)
            {
                orderList = orderList.Where(o => o.StatusId == dashboardFilters.statusId).ToList();
            }

            if(dashboardFilters.priorityId != null)
            {
                orderList = orderList.Where(o => o.PriorityId == dashboardFilters.priorityId).ToList();
            }

            if(dashboardFilters.technicianId != null)
            {
                orderList = orderList.Where(o => o.TechnicianId == dashboardFilters.technicianId).ToList();
            }

            decimal TotalAmount = 0.0M;
            decimal TotalCost = 0.0M;
            decimal TotalTax = 0.0M;
            decimal TotalProfit = 0.0M;

            for (int i = 0; i < orderList.Count; i++)
            {
                TotalAmount += orderList[i].TotalAmount;
                TotalTax += orderList[i].TaxAmount;
                TotalProfit += orderList[i].Profit;
            }

            TotalCost = TotalAmount - (TotalTax + TotalProfit);

            int brandsCount = _context.Brands.Select(brand => brand.Id).Count();
            int deviceCount = _context.Models.Select(model => model.Id).Count();
            int defectsCount = _context.RepairableDefects.Select(defect => defect.Id).Count();

            var now = DateTime.Now;
            var twelveMonthsAgo = now.AddMonths(-12);

            var annualRepairOrders = _context.Orders
                    .Where(order => order.CreatedAt >= twelveMonthsAgo)
                    .GroupBy(order => new { order.CreatedAt.Year, order.CreatedAt.Month })
                    .Select(group => new { group.Key.Year, group.Key.Month, order_count = group.Count() })
                    .OrderBy(group => group.Year)
                    .ThenBy(group => group.Month)
                    .ToList();

            var annualRepairOrdersList = new List<AnnualRepairOrder>();

            foreach (var order in annualRepairOrders)
            {
                annualRepairOrdersList.Add(new AnnualRepairOrder
                {
                    Year = order.Year,
                    Month = order.Month,
                    OrderCount = order.order_count
                });
            }

            var dashboard = new GetDashboardDTO
            {
                Amount = TotalAmount,
                Cost = TotalCost,
                Tax = TotalTax,
                Profit = TotalProfit,
                RepairOrders = orderList.Count,
                Brands = brandsCount,
                Devices = deviceCount,
                Defects = defectsCount,
                AnnualRepairOrders = annualRepairOrdersList
            };

            return dashboard;
        }
    }
}
