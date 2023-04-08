using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class NewPatientObject:PatientObject
    {
        [JsonInclude]
        public string? Password { get; set; }
    }
    public class PatientObject:UserObject
    {
        [JsonInclude]
        public string? Doctor_Username { get; set; }
        [JsonInclude]
        public DateTime? Last_Interacted { get; set; }
    }
}
