using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ISecurityService securityService;
        private readonly IPasswordService passwordService;

        public TokenController(IConfiguration _configuration, ISecurityService _securityService, IPasswordService _passwordService)
        {
            this.configuration = _configuration;
            this.securityService = _securityService;
            this.passwordService = _passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin login)
        {
            var validation = await IsValidUser(login);

            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);

                return Ok(new { token });
            }

            return NotFound();
        }

        //Usando una tupla
        private async Task<(bool, Security)> IsValidUser(UserLogin login)
        {
            var user = await this.securityService.GetLoginByCredentials(login);

            var isValid = this.passwordService.Check(user.Password, login.Password);

            return (isValid, user);
        }

        private string GenerateToken(Security security)
        {
            //Header
            var symmetricSecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, security.UserName),
                new Claim("User", security.User),
                new Claim(ClaimTypes.Role, security.Role.ToString())
            };

            //Payload
            var payload = new JwtPayload(
                this.configuration["Authentication:Issuer"],
                this.configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(10)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}