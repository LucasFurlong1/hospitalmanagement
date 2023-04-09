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
            string prescriptionJson = "{}";

            // Prepare filter field and value
            string fieldName = "Prescription_ID";
            string filterValue = id;

            // Get Prescription from SQL Service
            List<PrescriptionObject> prescription = _sqlservice.RetrievePrescriptionsFiltered(fieldName, filterValue);

            // If the Prescsription was found, then
            if (prescription.Count > 0)
            {
                // Convert Prescription to JSON
                prescriptionJson = JsonSerializer.Serialize<PrescriptionObject>(prescription[0], new JsonSerializerOptions() { WriteIndented = formatJson });
            }

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
            // Generate ID for Prescription
            prescription.Prescription_ID = Guid.NewGuid().ToString();

            // Create new Prescription
            _sqlservice.CreatePrescription(prescription);

            // Return Prescription's ID so UI has access to it
            return prescription.Prescription_ID;
        }

        public void UpdatePrescription(PrescriptionObject prescription)
        {
            _sqlservice.UpdatePrescription(prescription);
        }

        public void DeletePrescription(string prescription_ID)
        {
            _sqlservice.DeletePrescription(prescription_ID);
        }
    }
}
