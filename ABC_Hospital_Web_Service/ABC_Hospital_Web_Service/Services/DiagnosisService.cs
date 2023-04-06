using ABC_Hospital_Web_Service.Controllers;
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
            // Prepare filter field and value
            string fieldName = "Diagnosis_ID";
            string filterValue = id;

            // Get Diagnosis from SQL Service
            DiagnosisObject diagnosis = _sqlservice.RetrieveDiagnosesFiltered(fieldName, filterValue)[0];

            // Convert Diagnosis to JSON
            string diagnosisJson = JsonSerializer.Serialize<DiagnosisObject>(diagnosis, new JsonSerializerOptions() { WriteIndented = formatJson });

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
    }
}
