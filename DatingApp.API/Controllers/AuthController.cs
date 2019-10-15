using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto user)
        {

            //validate request

            if(await _authRepo.UserExists(user.Username.ToLower()))
            {
                return BadRequest(string.Format("User with username : {0} allready exists",user.Username));
            }

            var newUser = new User();
            newUser.Username = user.Username.ToLower();

            var createdUser = await _authRepo.Register(newUser,user.Password);
            
            return  StatusCode(201);
        }
    }
}