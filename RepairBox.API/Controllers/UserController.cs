using Microsoft.AspNetCore.Mvc;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.Permission;
using RepairBox.BL.DTOs.Role;
using RepairBox.BL.DTOs.User;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserServiceRepo _userRepo;
        private IRolesPermissionsServiceRepo _rolesPermissionsRepo;
        public UserController(IUserServiceRepo userRepo, IRolesPermissionsServiceRepo rolesPermissionsRepo)
        {
            _userRepo = userRepo;
            _rolesPermissionsRepo = rolesPermissionsRepo;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var data = _userRepo.GetUsers();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var data = _userRepo.GetUserById(id);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(CreateUserDTO createUser)
        {
            try
            {
                bool response = _userRepo.CreateUser(createUser);
                if (response)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "User") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = CustomMessage.EMAIL_ALREADY_EXIST });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateSelfUser")]
        public IActionResult UpdateSelfUser(UpdateSelfUserDTO updateUser)
        {
            try
            {
                _userRepo.ModifySelfUser(updateUser);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "User") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateOtherUser")]
        public IActionResult UpdateOtherUser(UpdateOtherUserDTO updateUser)
        {
            try
            {
                _userRepo.ModifyOtherUser(updateUser);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "User") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool isDeleted = _userRepo.DeleteUser(id);
                if (isDeleted)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "User") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.NOT_FOUND, "User") });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(UserChangePasswordDTO changePassword)
        {
            try
            {
                (string message, bool result) = _userRepo.ChangePassword(changePassword);

                if(result)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = message });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = message });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
            
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                var data = _rolesPermissionsRepo.GetRoles();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetRoleById")]
        public IActionResult GetRoleById(int id)
        {
            try
            {
                var data = _rolesPermissionsRepo.GetRoleById(id);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("CreateRole")]
        public IActionResult CreateRole(AddRoleDTO addRole)
        {
            try
            {
                _rolesPermissionsRepo.CreateRole(addRole);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Role") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateRole")]
        public IActionResult UpdateRole(UpdateRoleDTO updateRole)
        {
            try
            {
                _rolesPermissionsRepo.ModifyRole(updateRole);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Role") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeleteRole")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                bool isDeleted = _rolesPermissionsRepo.DeleteRole(id);
                if (isDeleted)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "Role") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.NOT_FOUND, "Role") });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPermissions")]
        public IActionResult GetPermissions()
        {
            try
            {
                var data = _rolesPermissionsRepo.GetAllPermissions();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPermissionById")]
        public IActionResult GetPermissionById(int id)
        {
            try
            {
                var data = _rolesPermissionsRepo.GetPermissionById(id);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("CreatePermission")]
        public IActionResult CreatePermission(AddPermissionDTO addPermission)
        {
            try
            {
                _rolesPermissionsRepo.CreatePermission(addPermission);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Permission") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdatePermission")]
        public IActionResult UpdatePermission(UpdatePermissionDTO updatePermission)
        {
            try
            {
                _rolesPermissionsRepo.ModifyPermission(updatePermission);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Permission") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeletePermission")]
        public IActionResult DeletePermission(int id)
        {
            try
            {
                bool isDeleted = _rolesPermissionsRepo.DeletePermission(id);
                if (isDeleted)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "Permission") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.NOT_FOUND, "Permission") });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
