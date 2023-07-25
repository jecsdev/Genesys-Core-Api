
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Genesys_Core_API.Models;
using Genesys_Core_API.Services;
using Genesys_Core_API.DTO.UserDto;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Genesys_Core_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        private IConfiguration _config;
      public LoginController (LoginService loginService, IConfiguration config)
      {
            _loginService = loginService;
            _config = config;
      }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize(UserDto user)
        {
            var usr = await _loginService.GetUser(user);
            if (usr == null)
            {
                return BadRequest(new { message = "El usuario no existe." });
            }
            string jwtToken = GenerateToken(usr);
            return Ok(new {token = jwtToken});
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Username", user.UserName),
                new Claim("Password", user.Password)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signIn
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

    }
}
