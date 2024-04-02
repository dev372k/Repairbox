using Microsoft.AspNetCore.Mvc;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.Dashboard;
using RepairBox.BL.DTOs.Order;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IDashboardServiceRepo _dashboardRepo;
        private IStatusServiceRepo _statusRepo;
        private IPriorityServiceRepo _priorityRepo;
        private IUserServiceRepo _userRepo;

        public HomeController(IDashboardServiceRepo dashboardRepo, IStatusServiceRepo statusRepo, IPriorityServiceRepo priorityRepo, IUserServiceRepo userRepo)
        {
            _dashboardRepo = dashboardRepo;
            _statusRepo = statusRepo;
            _priorityRepo = priorityRepo;
            _userRepo = userRepo;
        }

        [HttpGet("GetPaymentDropdown")]
        public IActionResult GetPaymentDropdown()
        {
            try
            {
                var data = new List<string> { "Paid", "Unpaid" };
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetStatusDropdown")]
        public IActionResult GetStatusDropdown()
        {
            try
            {
                var data = _statusRepo.GetStatuses();

                if(data == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = new List<string>() });
                }

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPriorityDropdown")]
        public IActionResult GetPriorityDropdown()
        {
            try
            {
                var data = _priorityRepo.GetPrioritiesForDropdown();

                if (data == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = new List<string>() });
                }

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetTechnicianDropdown")]
        public IActionResult GetTechnicianDropdown()
        {
            try
            {
                var data = _userRepo.GetUsersForDropdown();

                if (data == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = new List<string>() });
                }

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetDashboard")]
        public IActionResult GetDashboard()
        {
            try
            {
                var data = _dashboardRepo.GetDashboardData();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("GetDashboardWithFilters")]
        public IActionResult GetDashboardWithFilters(DisplayDashboardFiltersDTO dashboardFilters)
        {
            try
            {
                var data = _dashboardRepo.GetDashboardData(dashboardFilters);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
