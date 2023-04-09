using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class UserCredObject
    {
        [JsonInclude]
        public string Username { get; set; }
        [JsonInclude]
        public string Password { get; set; }

        public UserCredObject()
        {
            Username = "";
            Password = "";
        }
        public UserCredObject(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            UserCredObject cred2 = obj as UserCredObject ?? new UserCredObject();
            if (cred2 != null
                && Username.Equals(cred2.Username)
                && Password.Equals(cred2.Password))
            {
                return true;
            }

            return false;
        }
    }
}
