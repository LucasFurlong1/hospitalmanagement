using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class DoctorObject : UserObject
    {
        [JsonInclude]
        public string? Doctor_Department { get; set; }
        [JsonInclude]
        public bool? Is_On_Staff { get; set; }
        [JsonInclude]
        public string? Doctorate_Degree { get; set; }
    }
}
