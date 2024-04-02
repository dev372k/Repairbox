using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.Order;
using RepairBox.BL.DTOs.Priority;
using RepairBox.BL.DTOs.Status;
using RepairBox.BL.ServiceModels.Order;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IPriorityServiceRepo _priorityRepo;
        private IStatusServiceRepo _statusRepo;

        private readonly IOrderServiceRepo _orderRepo;

        public OrderController(IPriorityServiceRepo priorityRepo, IStatusServiceRepo statusRepo, IOrderServiceRepo orderRepo)
        {
            _priorityRepo = priorityRepo;
            _statusRepo = statusRepo;
            _orderRepo = orderRepo;
        }

        [HttpGet("GetPriorities")]
        public IActionResult GetPriorities()
        {
            try
            {
                var data = _priorityRepo.GetPriorities();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPriorityById")]
        public IActionResult GetPriorityById(int priorityId)
        {
            try
            {
                var data = _priorityRepo.GetPriorityById(priorityId);
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.SUCCESS,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpPost("AddPriority")]
        public IActionResult AddPriority(AddPriorityDTO model)
        {
            try
            {
                _priorityRepo.AddPriority(model);
                return Ok(new JSONResponse 
                { 
                    Status = ResponseMessage.SUCCESS, 
                    Message = String.Format(CustomMessage.ADDED_SUCCESSFULLY, "Priority") 
                });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpPut("UpdatePriority")]
        public IActionResult UpdatePriority(UpdatePriorityDTO model)
        {
            try
            {
                _priorityRepo.UpdatePriority(model);
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.SUCCESS,
                    Message = String.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Priority")
                });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpDelete("DeletePriority")]
        public IActionResult DeletePriority(int priorityId)
        {
            try
            {
                bool isDeleted = _priorityRepo.DeletePriority(priorityId);
                if (isDeleted == true)
                {
                    return Ok(new JSONResponse
                    {
                        Status = ResponseMessage.SUCCESS,
                        Message = String.Format(CustomMessage.DELETED_SUCCESSFULLY, "Priority")
                    });
                }
                else
                {
                    return Ok(new JSONResponse
                    {
                        Status = ResponseMessage.FAILURE,
                        ErrorMessage = String.Format(CustomMessage.NOT_FOUND, "Priority")
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpGet("GetStatuses")]
        public IActionResult GetStatuses()
        {
            try
            {
                var data = _statusRepo.GetStatuses();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetStatusById")]
        public IActionResult GetStatusById(int priorityId)
        {
            try
            {
                var data = _statusRepo.GetStatusById(priorityId);
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.SUCCESS,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpPost("AddStatus")]
        public IActionResult AddStatus(AddStatusDTO model)
        {
            try
            {
                _statusRepo.AddStatus(model);
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.SUCCESS,
                    Message = String.Format(CustomMessage.ADDED_SUCCESSFULLY, "Status")
                });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpPut("UpdateStatus")]
        public IActionResult UpdateStatus(UpdateStatusDTO model)
        {
            try
            {
                _statusRepo.UpdateStatus(model);
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.SUCCESS,
                    Message = String.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Status")
                });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpDelete("DeleteStatus")]
        public IActionResult DeleteStatus(int statusId)
        {
            try
            {
                bool isDeleted = _statusRepo.DeleteStatus(statusId);
                if (isDeleted == true)
                {
                    return Ok(new JSONResponse
                    {
                        Status = ResponseMessage.SUCCESS,
                        Message = String.Format(CustomMessage.DELETED_SUCCESSFULLY, "Status")
                    });
                }
                else
                {
                    return Ok(new JSONResponse
                    {
                        Status = ResponseMessage.FAILURE,
                        ErrorMessage = String.Format(CustomMessage.NOT_FOUND, "Status")
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse
                {
                    Status = ResponseMessage.FAILURE,
                    ErrorMessage = ex.Message,
                    ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty
                });
            }
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(AddOrderDTO model)
        {
            try
            {
                Task<int> orderId = _orderRepo.CreateOrder(model);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.ADDED_SUCCESSFULLY, "Order"), Data = orderId });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("CalculateOrderPrice")]
        public IActionResult CalculateOrderPrice(GetOrderChargesDTO model)
        {
            try
            {
                var data = _orderRepo.CalculateOrder(model);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetOrderFilterSortDropdown")]
        public IActionResult GetOrderFilterSortDropdown()
        {
            try
            {
                var FilterSortDropdown = new List<string>
                {
                    "Status", "Priority", "Technician", "Created at", "Order Number"
                };

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = FilterSortDropdown });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList(int pageNo)
        {
            try
            {
                var data = _orderRepo.GetOrderList(pageNo, null);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("GetOrderListWithFilters")]
        public IActionResult GetOrderListWithFilters([FromQuery] int pageNo, [FromBody] DisplayOrderFiltersDTO orderFilters)
        {
            try
            {
                var data = _orderRepo.GetOrderList(pageNo, orderFilters);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetOrderById")]
        public IActionResult GetOrderById(int orderId)
        {
            try
            {
                var data = _orderRepo.GetOrderById(orderId);
                if(data is null)
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = String.Format(CustomMessage.NOT_FOUND, "Order") });
                else
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetOrdersByTechnician")]
        public IActionResult GetOrdersByTechnician(int technicianId)
        {
            try
            {
                var data = _orderRepo.GetOrdersByTechnician(technicianId);
                if (data is null)
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = String.Format(CustomMessage.NOT_FOUND, "Order") });
                else
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateOrderTechnician")]
        public IActionResult UpdateOrderTechnician(UpdateOrderTechnicianDTO model)
        {
            try
            {
                (bool result, string notFound) = _orderRepo.UpdateOrderTechnician(model);

                if (!result)
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = String.Format(CustomMessage.NOT_FOUND, notFound) });
                else
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Order Technician") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(UpdateOrderStatusDTO model)
        {
            try
            {
                (bool result, string notFound) = _orderRepo.UpdateOrderStatus(model);

                if (!result)
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = String.Format(CustomMessage.NOT_FOUND, notFound) });
                else
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Order Status") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetOrderStatus")]
        public IActionResult GetOrderStatus(int orderid)
        {
            try
            {
                (bool result, string status, TrackOrderDTO order) = _orderRepo.GetOrderStatus(orderid);

                if (!result)
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = String.Format(CustomMessage.NOT_FOUND, status) });
                else
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = order });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
