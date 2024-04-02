using Microsoft.AspNetCore.Mvc;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.PrivacyPolicy;
using RepairBox.BL.DTOs.TermsAndConditions;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class LegalController : ControllerBase
    {
        private ITnCServiceRepo _tncServiceRepo;
        private IPrivacyPolicyServiceRepo _privacyPolicyServiceRepo;

        private const string tncStr = "Terms and Conditions";
        private const string privacyPolicyStr = "Privacy Policy";

        public LegalController(ITnCServiceRepo tncServiceRepo, IPrivacyPolicyServiceRepo privacyPolicyServiceRepo)
        {
            _tncServiceRepo = tncServiceRepo;
            _privacyPolicyServiceRepo = privacyPolicyServiceRepo;
        }

        #region Terms and Conditions
        [HttpPost("AddTnC")]
        public IActionResult AddTnC(AddTnCDTO tnc)
        {
            try
            {
                _tncServiceRepo.AddTnC(tnc);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, tncStr) });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("AddTnCs")]
        public IActionResult AddTnCs(List<AddTnCDTO> tncs)
        {
            try
            {
                _tncServiceRepo.AddTnC(tncs);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, tncStr) });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetTnCbyId")]
        public IActionResult GetTnCbyId(int Id)
        {
            try
            {
                var data = _tncServiceRepo.GetTnC(Id);

                if(data != null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, tncStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetAllTnCs")]
        public IActionResult GetAllTnCs()
        {
            try
            {
                var data = _tncServiceRepo.GetTnC();

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateTnC")]
        public IActionResult UpdateTnC(UpdateTnCDTO tnc)
        {
            try
            {
                bool result = _tncServiceRepo.UpdateTnC(tnc);

                if (result)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, tncStr) });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, tncStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeleteTnC")]
        public IActionResult DeleteTnC(int Id)
        {
            try
            {
                bool result = _tncServiceRepo.DeleteTnC(Id);

                if (result)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, tncStr) });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, tncStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        #endregion

        #region Privacy Policy
        [HttpPost("AddPrivacyPolicy")]
        public IActionResult AddPrivacyPolicy(AddPrivacyPolicyDTO privacyPolicy)
        {
            try
            {
                _privacyPolicyServiceRepo.AddPrivacyPolicy(privacyPolicy);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, privacyPolicyStr) });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("AddPrivacyPolicies")]
        public IActionResult AddPrivacyPolicies(List<AddPrivacyPolicyDTO> privacyPolicies)
        {
            try
            {
                _privacyPolicyServiceRepo.AddPrivacyPolicy(privacyPolicies);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, privacyPolicyStr) });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPrivacyPolicyById")]
        public IActionResult GetPrivacyPolicyById(int Id)
        {
            try
            {
                var data = _privacyPolicyServiceRepo.GetPrivacyPolicy(Id);

                if (data != null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, privacyPolicyStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPrivacyPolicies")]
        public IActionResult GetPrivacyPolicies()
        {
            try
            {
                var data = _privacyPolicyServiceRepo.GetPrivacyPolicy();

                if (data != null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, privacyPolicyStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdatePrivacyPolicy")]
        public IActionResult UpdatePrivacyPolicy(UpdatePrivacyPolicyDTO privacyPolicy)
        {
            try
            {
                bool result = _privacyPolicyServiceRepo.UpdatePrivacyPolicy(privacyPolicy);

                if (result)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, privacyPolicyStr) });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, privacyPolicyStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeletePrivacyPolicy")]
        public IActionResult DeletePrivacyPolicy(int Id)
        {
            try
            {
                bool result = _privacyPolicyServiceRepo.DeletePrivacyPolicy(Id);

                if (result)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, privacyPolicyStr) });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, privacyPolicyStr) });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        #endregion
    }
}
