using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DailyNeeds1.DTO;
using DailyNeeds1.Repo;
using DailyNeeds1.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using log4net;

namespace DailyNeeds1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private UnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private ILog _logger;

        public UserController(UnitOfWork unitOfWork, IConfiguration configuration, ILog logger)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            _logger = logger;
        }

        //public UserController(UnitOfWork uw)
        //{
        //    unitOfWork = uw;
        //}


        [HttpGet,Route("GetAllUsers")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = unitOfWork.UserImplObject.GetAll();
            return Ok(users);
        }

        [HttpPost, Route("Validate")]
        [AllowAnonymous]
        public IActionResult Validate(LoginDTO login)
        {
            try
            {
                User user = unitOfWork.UserImplObject.ValidteUser(login.Username,login.Password);
                //User user = userService.ValidteUser(login.Email, login.Password);

                AuthResponse authReponse = new AuthResponse();
                if (user != null)
                {
                    authReponse.UserName = user.Username;
                    authReponse.RoleId = user.RoleID;
                    authReponse.Token = GetToken(user);
                }
                return StatusCode(200, authReponse);
            }
            catch (Exception ex)
            {

                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        private string GetToken(User? user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            //header part
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );
            //payload part
            var subject = new ClaimsIdentity(new[]
            {
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Role, user.RoleID.ToString()),
                        new Claim(ClaimTypes.Email,user.Email),
             });

            var expires = DateTime.UtcNow.AddMinutes(10);
            //signature part
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }

        [HttpPost,Route("AddNewUser")]
        [Authorize]
        public IActionResult AddNewUser(UserDTO user)
        {
            try
            {
                bool status = unitOfWork.UserImplObject.Add(user);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Error in adding user");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
                //throw;
            }
        }

        [HttpPut,Route("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, UserDTO userUpdate)
        {
            userUpdate.UserID = id;

            try
            {
                bool status = unitOfWork.UserImplObject.Update(userUpdate);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok(userUpdate);
                }
                else
                {
                    return BadRequest($"User with ID {id} not found or update failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
                //throw;
            }
        }

        [HttpDelete,Route("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool status = unitOfWork.UserImplObject.Delete(id);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok($"User with ID {id} deleted");
                }
                else
                {
                    return BadRequest($"User with ID {id} not found or delete failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
                //throw;
            }
        }
    }
}
