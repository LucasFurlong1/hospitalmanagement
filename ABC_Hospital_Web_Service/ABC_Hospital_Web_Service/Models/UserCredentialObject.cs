using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class UserCredObject
    {
        [JsonInclude]
        public string? Username { get; set; }
        [JsonInclude]
        public string? Password { get; set; }

        public UserCredObject()
        {
            Username = null;
            Password = null;
        }
        public UserCredObject(string? username, string? password)
        {
            Username = username;
            Password = password;
        }
    }
}
