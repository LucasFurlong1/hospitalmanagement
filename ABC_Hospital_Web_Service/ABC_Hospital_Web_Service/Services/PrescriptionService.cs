using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class PrescriptionService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;
        private JsonSerializerOptions _jsonOptions;

        public PrescriptionService(IConfiguration appConfig, bool format_json = true)
        {
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
            _jsonOptions = new JsonSerializerOptions() { WriteIndented = formatJson };
        }

        public string GetPrescriptions()
        {
            // Get Prescriptions from SQL Service
            List<PrescriptionObject> prescriptions = _sqlservice.RetrievePrescriptions();

            // Convert Prescriptions to JSON
            string prescriptionJson = JsonSerializer.Serialize<List<PrescriptionObject>>(prescriptions, _jsonOptions);

            return prescriptionJson;
        }

        public string GetPrescriptionByID(string id)
        {
            // Prepare filter field and value
            string fieldName = "Prescription_ID";
            string filterValue = id;

            // Get Prescription from SQL Service
            List<PrescriptionObject> prescription = _sqlservice.RetrievePrescriptionsFiltered(fieldName, filterValue);

            // Convert Prescription to JSON
            string prescriptionJson = JsonSerializer.Serialize<List<PrescriptionObject>>(prescription, _jsonOptions);

            return prescriptionJson;
        }

        public string GetPrescriptionsByPatient(string patientUsername)
        {
            // Prepare filter field and value
            string fieldName = "Patient_Username";
            string filterValue = patientUsername.ToLower();

            // Get Prescriptions from SQL Service
            List<PrescriptionObject> prescriptions = _sqlservice.RetrievePrescriptionsFiltered(fieldName, filterValue);

            // Convert Prescriptions to JSON
            string prescriptionJson = JsonSerializer.Serialize<List<PrescriptionObject>>(prescriptions, _jsonOptions);

            return prescriptionJson;
        }

        public string CreatePrescription(PrescriptionObject prescription)
        {
            List<ReturnStringObject> id = new List<ReturnStringObject>();

            // Format DateTimes
            prescription.Prescribed_Date = DateTime.Parse(prescription.Prescribed_Date).ToString("yyyy-MM-dd");

            // Generate ID for Prescription
            prescription.Prescription_ID = Guid.NewGuid().ToString();

            // Save Prescription's ID
            id.Add(new ReturnStringObject(prescription.Prescription_ID));

            // Create new Prescription
            if (!_sqlservice.CreatePrescription(prescription))
            {
                // If the create failed, replace ID with "" to notify UI there was an error
                id[0].str = "";
            }

            // Return Prescription's ID or Empty so UI has access to it
            return JsonSerializer.Serialize<List<ReturnStringObject>>(id, _jsonOptions);
        }

        public string UpdatePrescription(PrescriptionObject prescription)
        {
            List<ReturnBoolObject> success = new List<ReturnBoolObject>();

            // Format DateTimes
            prescription.Prescribed_Date = DateTime.Parse(prescription.Prescribed_Date).ToString("yyyy-MM-dd");

            success.Add(new ReturnBoolObject(_sqlservice.UpdatePrescription(prescription)));
            return JsonSerializer.Serialize<List<ReturnBoolObject>>(success, _jsonOptions);
        }

        public string DeletePrescription(string prescription_ID)
        {
            List<ReturnBoolObject> success = new List<ReturnBoolObject>();

            success.Add(new ReturnBoolObject(_sqlservice.DeletePrescription(prescription_ID)));
            return JsonSerializer.Serialize<List<ReturnBoolObject>>(success, _jsonOptions);
        }
    }
}
