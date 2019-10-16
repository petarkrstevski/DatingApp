using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _authRepo = authRepo;
             _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto user)
        {

            //validate request

            if (await _authRepo.UserExists(user.Username.ToLower()))
            {
                return BadRequest(string.Format("User with username : {0} allready exists", user.Username));
            }

            var newUser = new User();
            newUser.Username = user.Username.ToLower();

            var createdUser = await _authRepo.Register(newUser, user.Password);

            return StatusCode(201);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto user)
        {
            var userDb = await _authRepo.Login(user.Username.ToLower(), user.Password);

            if (userDb == null)
            {
                return Unauthorized();
            }

            return Ok(new { token = GenerateJWT(userDb)});

        }

        private string GenerateJWT(User userDb)
        {
            var userClaims = new[]{
                 new Claim(ClaimTypes.NameIdentifier,userDb.Id.ToString()),
                 new Claim(ClaimTypes.Name, userDb.Username)
             };

            var configToken = _config.GetSection("AppSettings:Token").Value;
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configToken));

            var creds = new SigningCredentials(tokenKey,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtTokenObject = tokenHandler.CreateToken(tokenDescriptor);

            string jwtToken = tokenHandler.WriteToken(jwtTokenObject);

            return jwtToken;
        }
    }
}