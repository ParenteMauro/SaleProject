using Microsoft.AspNetCore.Mvc;
using SaleProject.Models.Request;
using SaleProject.Models.Response;
using SaleProject.Services;

namespace SaleProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest model)
        {
            ApiResponse response = new ApiResponse();
            var userResponse = await _userService.Auth(model);
            if (userResponse == null) 
            {
                response.Success = false;
                response.Message = "User or Password incorrect";
                return BadRequest(response);
            }
            response.Success = true;
            response.Data = userResponse;
            return Ok(response);
        }
    }
}
