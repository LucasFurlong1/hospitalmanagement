using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class PrescriptionObject
    {
        [JsonInclude]
        public string Prescription_ID { get; set; }
        [JsonInclude]
        public string Patient_Username { get; set; }
        [JsonInclude]
        public string Doctor_Username { get; set; }
        [JsonInclude]
        public string Medication_Name { get; set; }
        [JsonInclude]
        public string Prescribed_Date { get; set; }
        [JsonInclude]
        public string Dosage { get; set; }
        [JsonInclude]
        public string Instructions { get; set; }
        [JsonInclude]
        public bool Is_Filled { get; set; }


        public PrescriptionObject()
        {
            Prescription_ID = "";
            Patient_Username = "";
            Doctor_Username = "";
            Medication_Name = "";
            Prescribed_Date = DateTime.MinValue.ToShortDateString();
            Dosage = "";
            Instructions = "";
            Is_Filled = false;
        }

        public override bool Equals(object obj)
        {
            PrescriptionObject presc2 = obj as PrescriptionObject ?? new PrescriptionObject();
            if (presc2 != null
                && Prescription_ID.Equals(presc2.Prescription_ID)
                && Patient_Username.Equals(presc2.Patient_Username)
                && Doctor_Username.Equals(presc2.Doctor_Username)
                && Medication_Name.Equals(presc2.Medication_Name)
                && Prescribed_Date.Equals(presc2.Prescribed_Date)
                && Dosage.Equals(presc2.Dosage)
                && Instructions.Equals(presc2.Instructions)
                && Is_Filled.Equals(presc2.Is_Filled))
            {
                return true;
            }

            return false;
        }
    }
}
