using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class DiagnosisService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;
        private JsonSerializerOptions _jsonOptions;

        public DiagnosisService(IConfiguration appConfig, bool format_json = true)
        {
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
            _jsonOptions = new JsonSerializerOptions() { WriteIndented = formatJson };
        }

        public string GetDiagnoses()
        {
            // Get Diagnoses from SQL Service
            List<DiagnosisObject> diagnoses = _sqlservice.RetrieveDiagnoses();

            // Convert Diagnoses to JSON
            string diagnosesJson = JsonSerializer.Serialize<List<DiagnosisObject>>(diagnoses, _jsonOptions);

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
            string diagnosisJson = JsonSerializer.Serialize<List<DiagnosisObject>>(diagnosis, _jsonOptions);

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
            string diagnosesJson = JsonSerializer.Serialize<List<DiagnosisObject>>(diagnoses, _jsonOptions);

            return diagnosesJson;
        }

        public string CreateDiagnosis(DiagnosisObject diagnosis)
        {
            List<ReturnStringObject> id = new List<ReturnStringObject>();

            // Format DateTimes
            diagnosis.Diagnosis_Date = DateTime.Parse(diagnosis.Diagnosis_Date).ToString("yyyy-MM-dd");

            // Generate ID for Diagnosis
            diagnosis.Diagnosis_ID = Guid.NewGuid().ToString();

            // Create new Diagnosis
            if(!_sqlservice.CreateDiagnosis(diagnosis))
            {
                // If the create failed, return empty string to notify UI
                id.Add(new ReturnStringObject(""));
            }
            else
            {
                id.Add(new ReturnStringObject(diagnosis.Diagnosis_ID));
            }

            // Return Diagnosis's ID so UI has access to it
            return JsonSerializer.Serialize<List<ReturnStringObject>>(id, _jsonOptions);
        }

        public string UpdateDiagnosis(DiagnosisObject diagnosis)
        {
            List<ReturnBoolObject> success = new List<ReturnBoolObject>();

            // Format DateTimes
            diagnosis.Diagnosis_Date = DateTime.Parse(diagnosis.Diagnosis_Date).ToString("yyyy-MM-dd");

            success.Add(new ReturnBoolObject(_sqlservice.UpdateDiagnosis(diagnosis)));
            return JsonSerializer.Serialize<List<ReturnBoolObject>>(success, _jsonOptions);
        }

        public string DeleteDiagnosis(string diagnosis_ID)
        {
            List<ReturnBoolObject> success = new List<ReturnBoolObject>();

            success.Add(new ReturnBoolObject(_sqlservice.DeleteDiagnosis(diagnosis_ID)));
            return JsonSerializer.Serialize<List<ReturnBoolObject>>(success, _jsonOptions);
        }
    }
}
