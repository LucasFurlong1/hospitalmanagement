using ABC_Hospital_Web_Service.Controllers;
using ABC_Hospital_Web_Service.Models;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace ABC_Hospital_Web_Service.Services
{
    public class SecurityService
    {
        private SQLInterface _sqlservice;

        public SecurityService(bool format_json = true)
        {
            _sqlservice = new SQLInterface();
        }

        public bool CheckUserCredentials(string username, string password)
        {
            bool success = false;
            UserCredObject userCredsStored = GetUserCreds(username);
            if (VerifyHashedPassword(userCredsStored, password) == PasswordVerificationResult.Success)
            {
                success = true;
                _sqlservice.UpdateUserIdentitySession(username);
            }
            return success;
        }

        public void SaveNewCredentials(string username, string password)
        {
            UserCredObject userCred = new UserCredObject(username, password);
            userCred = HashPassword(userCred);
            if (VerifyHashedPassword(userCred, password) == PasswordVerificationResult.Success)
            {
                PutUserCreds(userCred);
            }
        }

        private UserCredObject GetUserCreds(string username)
        {
            return _sqlservice.RetrieveUserCred(username);
        }

        private void PutUserCreds(UserCredObject user)
        {
            _sqlservice.StoreUserCred(user);
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
