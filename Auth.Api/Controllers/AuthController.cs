using Auth.Api.Models;
using Auth.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> authoptions;

        public AuthController(IOptions<AuthOptions> authoptions)
        {
            this.authoptions = authoptions;
        }

        private List<Account> Accounts = new List<Account>()
        {
            new Account()
            {
                Id = Guid.Parse("7f4565ad-a62c-4a4a-896c-27e007357156"),
                Email = "user@email.com",
                Password = "user",
                Roles = new Role[] {Role.User}
            },
            new Account()
            {
                Id = Guid.Parse("43580a9d-e3b0-456d-9721-01a1bbc7226b"),
                Email = "user2@email.com",
                Password = "user2",
                Roles = new Role[] {Role.User}
            },
            new Account()
            {
                Id = Guid.Parse("65fb652a-fff9-4326-8977-dc1ce482f6a9"),
                Email = "admin@email.com",
                Password = "admin",
                Roles = new Role[] {Role.Admin}
            },
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(User);
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            var user = AuthenticateUser(request.Email, request.Password);

            // Генерация token'а 
            if(user != null) {
                var token = GenerateJWT(user);

                return Ok(new
                {
                    access_token= token
                });

            }

            return NotFound();
        }

        private Account AuthenticateUser(string email, string password)
        {
            return Accounts.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        private string GenerateJWT(Account user)
        {
            var authParams = authoptions.Value;

            // header - автоматически генерируется при создании 
            var securityKey = authParams.GetSymmetricSecurityKey();
            var creditial = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //

            // payload, payload - состоит из claims
            // claim - некие утверждения о пользователе 
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            // Чтобы добавить роли пришлось создать свой тип claim'а
            foreach(var role in user.Roles){
                claims.Add(new Claim("role", role.ToString()));
            }
            //

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: creditial);

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

    }
}
