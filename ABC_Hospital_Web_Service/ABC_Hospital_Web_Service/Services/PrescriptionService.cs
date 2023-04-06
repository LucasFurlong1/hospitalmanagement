using ABC_Hospital_Web_Service.Controllers;
using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class PrescriptionService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;

        public PrescriptionService(bool format_json = true)
        {
            _sqlservice = new SQLInterface();
            formatJson = format_json;
        }

        public string GetPrescriptions()
        {
            // Get Prescriptions from SQL Service
            List<PrescriptionObject> prescriptions = _sqlservice.RetrievePrescriptions();

            // Convert Prescriptions to JSON
            string prescriptionJson = JsonSerializer.Serialize<List<PrescriptionObject>>(prescriptions, new JsonSerializerOptions() { WriteIndented = formatJson });

            return prescriptionJson;
        }

        public string GetPrescriptionByID(string id)
        {
            // Prepare filter field and value
            string fieldName = "Prescription_ID";
            string filterValue = id;

            // Get Prescription from SQL Service
            PrescriptionObject prescription = _sqlservice.RetrievePrescriptionsFiltered(fieldName, filterValue)[0];

            // Convert Prescription to JSON
            string prescriptionJson = JsonSerializer.Serialize<PrescriptionObject>(prescription, new JsonSerializerOptions() { WriteIndented = formatJson });

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
            string prescriptionJson = JsonSerializer.Serialize<List<PrescriptionObject>>(prescriptions, new JsonSerializerOptions() { WriteIndented = formatJson });

            return prescriptionJson;
        }
    }
}
