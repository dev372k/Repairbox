using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.User;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IUserServiceRepo _userRepo;
        public AuthController(IConfiguration configuration, IUserServiceRepo userRepo)
        {
            _configuration = configuration;
            _userRepo = userRepo;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(1),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, string userEmail)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            _userRepo.SetRefreshToken(userEmail, newRefreshToken);
        }

        private string CreateToken(UserCreateTokenDTO userTokenDTO)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userTokenDTO.Email),
                new Claim(ClaimTypes.Role, userTokenDTO.Role),
                new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(userTokenDTO.Resources))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginDTO userLogin)
        {
            try
            {
                bool response = _userRepo.VerifyUserLogin(userLogin);
                if (response)
                {
                    var userRolesResources = _userRepo.GetUserRoleResourcesForToken(userLogin.Email);

                    if(userRolesResources is null)
                        return Unauthorized();

                    string token = CreateToken(userRolesResources);

                    userRolesResources.Token = token;

                    var refreshToken = GenerateRefreshToken();

                    SetRefreshToken(refreshToken, userLogin.Email);

                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = CustomMessage.LOGIN_SUCCESSFUL, Data = userRolesResources });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = CustomMessage.INCORRECT_CREDENTIALS });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("VerifyCredentials")]
        public IActionResult VerifyCredentials(UserLoginDTO userLogin)
        {
            try
            {
                bool response = _userRepo.VerifyUserLogin(userLogin);
                if (response)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = CustomMessage.CORRECT_CREDENTIALS });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = CustomMessage.INCORRECT_CREDENTIALS });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("IsLoggedIn")]
        [Authorize]
        public IActionResult IsLoggedIn()
        {
            try
            {
                var principal = HttpContext.User;
                var isValidToken = principal.Identity is { IsAuthenticated: true };

                if (!isValidToken)
                {
                    return Unauthorized();
                }

                var userEmail = _userRepo.GetMyEmail();

                if(userEmail == null)
                {
                    return Unauthorized();
                }

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = CustomMessage.USER_LOGGED_IN, Data = userEmail });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                var tokenDTO = _userRepo.GetRefreshToken(userEmail);

                if(tokenDTO == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, "User Token") });
                }
                else if (tokenDTO.Token.Equals(refreshToken))
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = CustomMessage.INVALID_REFRESH_TOKEN });
                }
                else if (tokenDTO.Expires < DateTime.Now)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = CustomMessage.EXPIRED_TOKEN });
                }

                var userRolesResources = _userRepo.GetUserRoleResourcesForToken(userEmail);

                if (userRolesResources is null)
                    return Unauthorized();

                string token = CreateToken(userRolesResources);

                userRolesResources.Token = token;

                var newRefreshToken = GenerateRefreshToken();

                SetRefreshToken(newRefreshToken, userEmail);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = CustomMessage.TOKEN_REFRESHED, Data = token });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex.InnerException?.ToString() ?? string.Empty });
            }

        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                var userEmail = _userRepo.GetMyEmail();

                _userRepo.ClearRefreshToken(userEmail);

                Response.Cookies.Delete("refreshToken");

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = CustomMessage.LOGOUT_SUCCESSFUL });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}