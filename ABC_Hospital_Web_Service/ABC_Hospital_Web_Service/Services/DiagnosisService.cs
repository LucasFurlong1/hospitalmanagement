using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class DiagnosisService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;

        public DiagnosisService(bool format_json = true)
        {
            _sqlservice = new SQLInterface();
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
            string diagnosisJson = "{}";

            // Prepare filter field and value
            string fieldName = "Diagnosis_ID";
            string filterValue = id;

            // Get Diagnosis from SQL Service
            List<DiagnosisObject> diagnosis = _sqlservice.RetrieveDiagnosesFiltered(fieldName, filterValue);

            // If a Diagnosis was returned, then
            if (diagnosis.Count > 0)
            {
                // Convert Diagnosis to JSON
                diagnosisJson = JsonSerializer.Serialize<DiagnosisObject>(diagnosis[0], new JsonSerializerOptions() { WriteIndented = formatJson });
            }

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
            // Generate ID for Diagnosis
            diagnosis.Diagnosis_ID = Guid.NewGuid().ToString();

            // Create new Diagnosis
            _sqlservice.CreateDiagnosis(diagnosis);

            // Return Diagnosis's ID so UI has access to it
            return diagnosis.Diagnosis_ID;
        }

        public void UpdateDiagnosis(DiagnosisObject diagnosis)
        {
            _sqlservice.UpdateDiagnosis(diagnosis);
        }

        public void DeleteDiagnosis(string diagnosis_ID)
        {
            _sqlservice.DeleteDiagnosis(diagnosis_ID);
        }
    }
}
