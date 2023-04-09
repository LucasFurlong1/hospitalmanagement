using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class UserSessionObject
    {
        [JsonInclude]
        public string? Username { get; set; }
        [JsonInclude]
        public DateTime? SessionStart { get; set; }
        [JsonInclude]
        public DateTime? SessionExpire { get; set; }

        public UserSessionObject()
        {

        }
        public UserSessionObject(string username, DateTime start, DateTime end)
        {
            Username = username;
            SessionStart = start;
            SessionExpire = end;
        }
    }
}
