using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FindMusician.API.Models;
using FindMusician.API.Authentication;
using FindMusician.API.Services;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace FindMusician.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : BaseController
    {
        private UserService _service;

        public LoginController(UserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody] User user, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            try
            {
                bool credenciaisValidas = false;
                if (user != null && !String.IsNullOrWhiteSpace(user.Login))
                {
                    var userBase = _service.Get(user.Login);
                    credenciaisValidas = (userBase != null && user.Login == userBase.Login && user.AccessKey == userBase.AccessKey);
                }

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Login, "Login"),
                        new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Login)
                        }
                    );

                    DateTime creationDate = DateTime.Now;
                    DateTime expirationDate = creationDate + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = creationDate,
                        Expires = expirationDate
                    });
                    var token = handler.WriteToken(securityToken);

                    return Response(new
                    {
                        authenticated = true,
                        created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha ao autenticar");
                    return Response(new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar"
                    });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);                
                return Response();
            }
        }
    }
}