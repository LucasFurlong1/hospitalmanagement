using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class PrescriptionService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;

        public PrescriptionService(IConfiguration appConfig, bool format_json = true)
        {
            _sqlservice = new SQLInterface(appConfig);
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
            List<PrescriptionObject> prescription = _sqlservice.RetrievePrescriptionsFiltered(fieldName, filterValue);

            // Convert Prescription to JSON
            string prescriptionJson = JsonSerializer.Serialize<List<PrescriptionObject>>(prescription, new JsonSerializerOptions() { WriteIndented = formatJson });

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

        public string CreatePrescription(PrescriptionObject prescription)
        {
            // Format DateTimes
            prescription.Prescribed_Date = DateTime.Parse(prescription.Prescribed_Date).ToShortDateString();

            // Generate ID for Prescription
            prescription.Prescription_ID = Guid.NewGuid().ToString();

            // Create new Prescription
            if (!_sqlservice.CreatePrescription(prescription))
            {
                // If the create failed, return empty string to notify UI
                return "";
            }

            // Return Prescription's ID so UI has access to it
            return prescription.Prescription_ID;
        }

        public bool UpdatePrescription(PrescriptionObject prescription)
        {
            // Format DateTimes
            prescription.Prescribed_Date = DateTime.Parse(prescription.Prescribed_Date).ToShortDateString();

            return _sqlservice.UpdatePrescription(prescription);
        }

        public bool DeletePrescription(string prescription_ID)
        {
            return _sqlservice.DeletePrescription(prescription_ID);
        }
    }
}
