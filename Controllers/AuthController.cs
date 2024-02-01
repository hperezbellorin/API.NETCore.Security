using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApplicacionSeguridad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public string Get(string nameUser)
        {
 
            var data = GetToken(nameUser);
            return data;
        }
        [HttpGet]

        private string GetToken(string nameUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nameUser),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                                 
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:TokenExpiry"]));

            var token = new JwtSecurityToken(issuer,
              audience,
              expires: jwtValidity,
              claims: authClaims,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        //private string GetToken(string userEmail, DateTime? expires, IEnumerable<Claim> claims)
        //{
        //    try
        //    {

          
        //    var handler = new JwtSecurityTokenHandler();
        //    var identity = new ClaimsIdentity(new GenericIdentity(userEmail, "Auth"), claims);

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Issuer = _config["Jwt:Issuer"],
        //        Audience = _config["Jwt:Issuer"],
        //        SigningCredentials = credentials,
        //        Subject = identity,
        //        Expires = DateTime.Now.AddMinutes(120),
        //        IssuedAt = DateTime.UtcNow
        //    });
        //    return handler.WriteToken(securityToken);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

    }
}