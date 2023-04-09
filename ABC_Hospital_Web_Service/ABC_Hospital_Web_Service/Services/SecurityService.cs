using ABC_Hospital_Web_Service.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class SecurityService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;

        public SecurityService(bool format_json = true)
        {
            _sqlservice = new SQLInterface();
            formatJson = format_json;
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

        public void SaveNewCredentials(string username, string password)
        {
            UserCredObject userCred = new UserCredObject(username, password);
            userCred = HashPassword(userCred);
            if (VerifyHashedPassword(userCred, password) == PasswordVerificationResult.Success)
            {
                _sqlservice.StoreUserCred(userCred);
            }
        }

        public string GetSessionExpirationTime(string username)
        {
            string sessionJson = "{}";

            UserSessionObject session = _sqlservice.RetrieveUserIdentitySession(username);

            if (session.Username != null)
            {
                sessionJson = JsonSerializer.Serialize<UserSessionObject>(session, new JsonSerializerOptions() { WriteIndented = formatJson });
            }

            return sessionJson;
        }

        public void UpdateSessionData(UserSessionObject session)
        {
            _sqlservice.UpdateUserIdentitySession(session);
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
