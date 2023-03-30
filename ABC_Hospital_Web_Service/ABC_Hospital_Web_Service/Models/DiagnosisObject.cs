using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class DiagnosisObject
    {
        [JsonInclude]
        public string? Diagnosis_ID { get; set; }
        [JsonInclude]
        public string? Patient_Username { get; set; }
        [JsonInclude]
        public string? Doctor_Username { get; set; }
        [JsonInclude]
        public string? Diagnosis_Name { get; set; }
        [JsonInclude]
        public DateTime? Diagnosis_Date { get; set; }
        [JsonInclude]
        public string? Diagnosis_Description { get; set; }
        [JsonInclude]
        public string? Diagnosis_Treatment { get; set; }
        [JsonInclude]
        public bool? Was_Admitted { get; set; }
        [JsonInclude]
        public bool? Is_Resolved { get; set; }
    }
}
