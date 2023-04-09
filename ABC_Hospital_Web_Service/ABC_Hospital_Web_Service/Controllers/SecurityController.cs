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

        public SecurityController(IConfiguration appConfig)
        {
            _securityService = new SecurityService(appConfig);
        }

        // Security Endpoints
        [HttpPost("LoginRequest")]
        public ActionResult<bool> LoginRequest([FromBody] UserCredObject requestObj)
        {
            return _securityService.CheckUserCredentials(requestObj.Username, requestObj.Password);
        }

        // For testing purposes, allows new password to be assigned to a user
        [HttpPost("UpdateCredentials")]
        public bool UpdateCredentials([FromBody] UserCredObject requestObj)
        {
            return _securityService.SaveNewCredentials(requestObj.Username, requestObj.Password);
        }
        [HttpGet("GetSessionExpirationTime")]
        public ActionResult<string> GetSessionExpirationTime(string username)
        {
            return _securityService.GetSessionExpirationTime(username);
        }
        [HttpPost("UpdateSessionData")]
        public bool UpdateSessionData([FromBody] UserSessionObject newSession)
        {
            return _securityService.UpdateSessionData(newSession);
        }
        [HttpGet("GetChatBotData")]
        public string GetChatBotData()
        {
            return _securityService.GetChatBotConnectionData();
        }
    }
}
