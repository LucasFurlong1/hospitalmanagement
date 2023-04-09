using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class DiagnosisService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;

        public DiagnosisService(IConfiguration appConfig, bool format_json = true)
        {
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
        }

        public string GetDiagnoses()
        {
            // Get Diagnoses from SQL Service
            List<DiagnosisObject> diagnoses = _sqlservice.RetrieveDiagnoses();

            // Convert Diagnoses to JSON
            string diagnosesJson = JsonSerializer.Serialize<List<DiagnosisObject>>(diagnoses, new JsonSerializerOptions() { WriteIndented = formatJson });

            return diagnosesJson;
        }

        public string GetDiagnosisByID(string id)
        {
            // Prepare filter field and value
            string fieldName = "Diagnosis_ID";
            string filterValue = id;

            // Get Diagnosis from SQL Service
            List<DiagnosisObject> diagnosis = _sqlservice.RetrieveDiagnosesFiltered(fieldName, filterValue);

            // Convert Diagnosis to JSON
            string diagnosisJson = JsonSerializer.Serialize<List<DiagnosisObject>>(diagnosis, new JsonSerializerOptions() { WriteIndented = formatJson });

            return diagnosisJson;
        }

        public string GetDiagnosesByPatient(string patientUsername)
        {
            // Prepare filter field and value
            string fieldName = "Patient_Username";
            string filterValue = patientUsername.ToLower();

            // Get Diagnoses from SQL Service
            List<DiagnosisObject> diagnoses = _sqlservice.RetrieveDiagnosesFiltered(fieldName, filterValue);

            // Convert Diagnoses to JSON
            string diagnosesJson = JsonSerializer.Serialize<List<DiagnosisObject>>(diagnoses, new JsonSerializerOptions() { WriteIndented = formatJson });

            return diagnosesJson;
        }

        public string CreateDiagnosis(DiagnosisObject diagnosis)
        {
            // Format DateTimes
            diagnosis.Diagnosis_Date = DateTime.Parse(diagnosis.Diagnosis_Date).ToShortDateString();

            // Generate ID for Diagnosis
            diagnosis.Diagnosis_ID = Guid.NewGuid().ToString();

            // Create new Diagnosis
            if(!_sqlservice.CreateDiagnosis(diagnosis))
            {
                // If the create failed, return empty string to notify UI
                return "";
            }

            // Return Diagnosis's ID so UI has access to it
            return diagnosis.Diagnosis_ID;
        }

        public bool UpdateDiagnosis(DiagnosisObject diagnosis)
        {
            // Format DateTimes
            diagnosis.Diagnosis_Date = DateTime.Parse(diagnosis.Diagnosis_Date).ToShortDateString();

            return _sqlservice.UpdateDiagnosis(diagnosis);
        }

        public bool DeleteDiagnosis(string diagnosis_ID)
        {
            return _sqlservice.DeleteDiagnosis(diagnosis_ID);
        }
    }
}
