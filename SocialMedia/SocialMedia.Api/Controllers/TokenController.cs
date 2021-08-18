using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public TokenController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin login)
        {
            if (IsValidUser(login))
            {
                var token = GenerateToken();

                return Ok(new { token });
            }

            return NotFound();
        }

        private bool IsValidUser(UserLogin login)
        {
            return true;
        }

        private string GenerateToken()
        {
            //Header
            var symmetricSecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "David Cruz"),
                new Claim(ClaimTypes.Email, "dach_092@hotmail.com"),
                new Claim(ClaimTypes.Role, "Administrador")
            };

            //Payload
            var payload = new JwtPayload(
                this.configuration["Authentication:Issuer"],
                this.configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(2)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}