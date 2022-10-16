using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentMasterAPI.Models;
using StudentMasterAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserDTO authenticateUser)
        {
            var user = await _userManager.FindByNameAsync(authenticateUser.UserName);
            if(user == null)
            {
                return Unauthorized();
            }

            //Verify user password
            bool isUserValid = await _userManager.CheckPasswordAsync(user, authenticateUser.Password);
            if (isUserValid)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

                //Create login claims 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _configuration["JWT:Audience"],
                    Issuer = _configuration["JWT:Issuer"],
                    Expires = DateTime.UtcNow.AddDays(10),
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                          SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(tokenHandler.WriteToken(token));
                
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUser)
        {
            var user = new User
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                Gender = registerUser.Gender,
                UserName = registerUser.Email,
                Address = registerUser.Address,
            };
            var response = await _userManager.CreateAsync(user, registerUser.Password);

            if (response.Succeeded)
            {
                return Ok("User registered successfully!");
            }
            else
            {
                return BadRequest(response.Errors);
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(DeleteUserDTO deleteUser)
        {
            
            var user = await _userManager.FindByEmailAsync(deleteUser.Email);
            if (user != null)
            {
                var response = await _userManager.DeleteAsync(user);

                if (response.Succeeded)
                {
                    return Ok("User deleted successfully!");
                }
                else
                {
                    return BadRequest(response.Errors);
                }
            }
            return BadRequest("Cannot delete user with this email!");
            
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDTO roleDTO)
        {
            var response = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleDTO.RoleName
            });
            

            if (response.Succeeded)
            {
                return Ok("New Role Created!");
            }
            else
            {
                return BadRequest(response.Errors);
            }
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserDTO assignedRole)
        {
            var user = await _userManager.FindByEmailAsync(assignedRole.Email);
            if(user != null)
            {
                //Assign role to the user with matching email
                var response = await _userManager.AddToRoleAsync(user, assignedRole.RoleName);
                if (response.Succeeded)
                {
                    return Ok("User Assigned to " + assignedRole.RoleName + " role successfully");
                }
                else
                {
                    return BadRequest(response.Errors);
                }
            }
            else
            {
                return BadRequest("There is no user for this email");
            }

            
        }
    }
}
