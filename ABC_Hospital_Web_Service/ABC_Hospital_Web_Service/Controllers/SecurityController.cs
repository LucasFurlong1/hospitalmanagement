using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private SecurityService _securityService;

        public SecurityController()
        {
            _securityService = new SecurityService();
        }

        // Security Endpoints
        [HttpPost("LoginRequest")]
        public ActionResult<bool> LoginRequest([FromBody] Models.UserCredObject requestObj)
        {
            return _securityService.CheckUserCredentials(requestObj.Username, requestObj.Password);
        }
        //[HttpGet("CheckUserSession")]

    }
}
