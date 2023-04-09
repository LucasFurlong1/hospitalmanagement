using ABC_Hospital_Web_Service.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class SecurityService
    {
        private readonly IConfiguration _appConfig;
        private SQLInterface _sqlservice;
        private bool formatJson;

        public SecurityService(IConfiguration appConfig, bool format_json = true)
        {
            _appConfig = appConfig;
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
        }

        public string GetChatBotConnectionData()
        {
            List<KeyTokenPair> chatApiConfig = new List<KeyTokenPair>();
            chatApiConfig.Add(new KeyTokenPair());
            chatApiConfig[0].Key = _appConfig.GetValue<string>("AppSettings:ChatApi:Key");
            chatApiConfig[0].Token = _appConfig.GetValue<string>("AppSettings:ChatApi:Token");

            string json = JsonSerializer.Serialize<List<KeyTokenPair>>(chatApiConfig, new JsonSerializerOptions() { WriteIndented = formatJson });

            return json;
        }

        public bool CheckUserCredentials(string username, string password)
        {
            bool success = false;
            UserCredObject userCredsStored = _sqlservice.RetrieveUserCred(username);
            if (userCredsStored.Password != "" && VerifyHashedPassword(userCredsStored, password) == PasswordVerificationResult.Success)
            {
                success = true;
                UserSessionObject session = new UserSessionObject(username, DateTime.Now, DateTime.Now.AddMinutes(30));
                _sqlservice.UpdateUserIdentitySession(session);
            }
            return success;
        }

        public bool SaveNewCredentials(string username, string password)
        {
            UserCredObject userCred = new UserCredObject(username, password);
            userCred = HashPassword(userCred);
            if (VerifyHashedPassword(userCred, password) == PasswordVerificationResult.Success)
            {
                return _sqlservice.StoreUserCred(userCred);
            }
            return false;
        }

        public string GetSessionExpirationTime(string username)
        {
            List<UserSessionObject> session = new List<UserSessionObject>();

            session.Add(_sqlservice.RetrieveUserIdentitySession(username));

            string sessionJson = JsonSerializer.Serialize<List<UserSessionObject>>(session, new JsonSerializerOptions() { WriteIndented = formatJson });
            return sessionJson;
        }

        public bool UpdateSessionData(UserSessionObject session)
        {
            return _sqlservice.UpdateUserIdentitySession(session);
        }

        private UserCredObject HashPassword(UserCredObject userUnHashed)
        {
            userUnHashed.Password = BCrypt.Net.BCrypt.HashPassword(userUnHashed.Password);
            return userUnHashed;
        }

        private PasswordVerificationResult VerifyHashedPassword(UserCredObject userHashed, string passwordGiven)
        {
            return BCrypt.Net.BCrypt.Verify(passwordGiven, userHashed.Password) ?
              PasswordVerificationResult.Success :
              PasswordVerificationResult.Failed;
        }
    }
}
