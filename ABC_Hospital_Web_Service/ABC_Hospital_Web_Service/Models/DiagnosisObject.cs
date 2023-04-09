using System.Reflection;
using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class DiagnosisObject
    {
        [JsonInclude]
        public string Diagnosis_ID { get; set; }
        [JsonInclude]
        public string Patient_Username { get; set; }
        [JsonInclude]
        public string Doctor_Username { get; set; }
        [JsonInclude]
        public string Diagnosis_Name { get; set; }
        [JsonInclude]
        public string Diagnosis_Date { get; set; }
        [JsonInclude]
        public string Diagnosis_Description { get; set; }
        [JsonInclude]
        public string Diagnosis_Treatment { get; set; }
        [JsonInclude]
        public bool Was_Admitted { get; set; }
        [JsonInclude]
        public bool Is_Resolved { get; set; }
        public DiagnosisObject()
        {
            Diagnosis_ID = "";
            Patient_Username = "";
            Doctor_Username = "";
            Diagnosis_Name = "";
            Diagnosis_Date = DateTime.MinValue.ToShortDateString();
            Diagnosis_Description = "";
            Diagnosis_Treatment = "";
            Was_Admitted = false;
            Is_Resolved = false;
        }

        public override bool Equals(object obj)
        {
            DiagnosisObject diag2 = obj as DiagnosisObject ?? new DiagnosisObject();
            if(diag2 != null
                && Diagnosis_ID.Equals(diag2.Diagnosis_ID)
                && Patient_Username.Equals(diag2.Patient_Username)
                && Doctor_Username.Equals(diag2.Doctor_Username)
                && Diagnosis_Name.Equals(diag2.Diagnosis_Name)
                && Diagnosis_Date.Equals(diag2.Diagnosis_Date)
                && Diagnosis_Description.Equals(diag2.Diagnosis_Description)
                && Diagnosis_Treatment.Equals(diag2.Diagnosis_Treatment)
                && Was_Admitted.Equals(diag2.Was_Admitted)
                && Is_Resolved.Equals(diag2.Is_Resolved))
            {
                return true;
            }

            return false;
        }
    }
}
