using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class PrescriptionObject
    {
        [JsonInclude]
        public string? Prescription_ID { get; set; }
        [JsonInclude]
        public string? Patient_Username { get; set; }
        [JsonInclude]
        public string? Doctor_Username { get; set; }
        [JsonInclude]
        public string? Medication_Name { get; set; }
        [JsonInclude]
        public DateTime? Prescribed_Date { get; set; }
        [JsonInclude]
        public string? Dosage { get; set; }
        [JsonInclude]
        public string? Instructions { get; set; }
        [JsonInclude]
        public bool? Is_Filled { get; set; }
    }
}
