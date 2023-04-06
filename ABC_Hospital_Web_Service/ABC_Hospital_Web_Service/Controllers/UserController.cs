using ABC_Hospital_Web_Service.Models;
using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _userService = new UserService(logger);
            _logger = logger;
        }

        [HttpGet("GetUser")]
        public ActionResult<string> GetUser(string username)
        {
            return _userService.GetUserByUsername(username);
        }

        [HttpGet("GetUsers")]
        public ActionResult<string> GetUsers()
        {
            return _userService.GetUsers();
        }
        [HttpGet("GetUsersByAccountType")]
        public ActionResult<string> GetUsers(string accountType)
        {
            return _userService.GetUsersByAccountType(accountType);
        }
        //CreateUser? May not need
    }
}