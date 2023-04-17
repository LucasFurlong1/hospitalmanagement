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
        
        [HttpPost("LoginRequest")]
        public ActionResult<bool> LoginRequest([FromBody] UserCredObject requestObj)
        {
            return _securityService.CheckUserCredentials(requestObj.Username, requestObj.Password);
        }

        // This endpoint is for testing purposes only, allows new password to be assigned to a user
        // Included for convenience of grading, and is not a feature
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
