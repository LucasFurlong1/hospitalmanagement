using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class UserObject
    {
        [JsonInclude]
        public string? Username { get; set; }
        [JsonInclude]
        public char? Account_Type { get; set; }
        [JsonInclude]
        public string? Name { get; set; }
        [JsonInclude]
        public DateTime? Birth_Date { get; set; }
        [JsonInclude]
        public char? Gender { get; set; }
        [JsonInclude]
        public string? Address { get; set; }
        [JsonInclude]
        public string? Phone_Number { get; set; }
        [JsonInclude]
        public string? Email_Address { get; set; }
        [JsonInclude]
        public string? Emergency_Contact_Name { get; set; }
        [JsonInclude]
        public string? Emergency_Contact_Number { get; set; }
        [JsonInclude]
        public DateTime? Date_Created { get; set; }
    }
}
