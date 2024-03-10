using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using jwtconfig.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwtconfig.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RunData : ControllerBase
    {
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [RequireClaimAttributes(IdentityData.AdminUserClaimName, "true")]
        [HttpPost]
        public IActionResult returnData(string dto)
        {
            return Ok(dto);
        }

        private const string TokenSecret = "AWJDLAKCJDKAWCJDKAWCKJDAWDWADAWDAWDAWDAWDAWDAWD";
        private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);

        [HttpGet]
        public IActionResult getToken()
        {
            var tokenHdlr = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, "matheus2ep@gmail.com"),
                new(JwtRegisteredClaimNames.Email, "matheus2ep@gmail.com"),
                new("userId", 1.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime).AddHours(8),
                Issuer = "matheus",
                Audience = "teste",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHdlr.CreateToken(tokenDescriptor);

            var jwt = tokenHdlr.WriteToken(token);
            return Ok(jwt);
        }
    }
}
