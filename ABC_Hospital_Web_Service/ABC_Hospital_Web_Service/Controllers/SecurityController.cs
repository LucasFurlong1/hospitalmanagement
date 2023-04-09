using ABC_Hospital_Web_Service.Models;
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
        public ActionResult<bool> LoginRequest([FromBody] UserCredObject requestObj)
        {
            return _securityService.CheckUserCredentials(requestObj.Username, requestObj.Password);
        }

        // For testing purposes, allows new password to be assigned to a user
        [HttpPost("UpdateCredentials")]
        public void UpdateCredentials([FromBody] UserCredObject requestObj)
        {
            _securityService.SaveNewCredentials(requestObj.Username, requestObj.Password);
        }
        [HttpGet("GetSessionExpirationTime")]
        public ActionResult<string> GetSessionExpirationTime(string username)
        {
            return _securityService.GetSessionExpirationTime(username);
        }
        [HttpPost("UpdateSessionData")]
        public void UpdateSessionData([FromBody] UserSessionObject newSession)
        {
            _securityService.UpdateSessionData(newSession);
        }
    }
}
