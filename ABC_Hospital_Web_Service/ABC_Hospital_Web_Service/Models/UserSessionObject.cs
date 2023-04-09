using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class UserSessionObject
    {
        [JsonInclude]
        public string Username { get; set; }
        [JsonInclude]
        public DateTime SessionStart { get; set; }
        [JsonInclude]
        public DateTime SessionExpire { get; set; }

        public UserSessionObject()
        {
            Username = "";
            SessionStart = DateTime.MinValue;
            SessionExpire = DateTime.MinValue;
        }
        public UserSessionObject(string username, DateTime start, DateTime end)
        {
            Username = username;
            SessionStart = start;
            SessionExpire = end;
        }

        public override bool Equals(object obj)
        {
            UserSessionObject sess2 = obj as UserSessionObject ?? new UserSessionObject();
            if (sess2 != null
                && Username.Equals(sess2.Username)
                && TrimToSecond(SessionStart).Equals(TrimToSecond(sess2.SessionStart))
                && TrimToSecond(SessionExpire).Equals(TrimToSecond(sess2.SessionExpire)))
            {
                return true;
            }

            return false;
        }

        public DateTime TrimToSecond(DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }
}
