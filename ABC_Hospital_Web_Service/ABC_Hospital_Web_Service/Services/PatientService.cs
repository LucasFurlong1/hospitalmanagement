using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class PatientService
    {
        private SecurityService _securityService;
        private UserService _userService;
        private SQLInterface _sqlservice;
        private bool formatJson;

        public PatientService(bool format_json = true)
        {
            _securityService = new SecurityService();
            _userService = new UserService();
            _sqlservice = new SQLInterface();
            formatJson = format_json;
        }

        public string GetPatientInfo(string userName)
        {
            string patientJson = "{}";

            // Prepare filter field and value
            string fieldName = "Patient_Username";
            string filterValue = userName.ToLower();

            // Get Patient from SQL Service
            List<PatientObject> patient = _sqlservice.RetrievePatientsFiltered(fieldName, filterValue);

            // If Patient was found, then
            if (patient.Count > 0)
            {
                // Get Patient's user data from SQL Service
                UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", filterValue)[0];

                // Merge Data
                patient[0] = patient[0] + temp;

                // Convert Patient to JSON
                patientJson = JsonSerializer.Serialize<PatientObject>(patient[0], new JsonSerializerOptions() { WriteIndented = formatJson });
            }

            return patientJson;
        }

        public string GetPatientsByDoctor(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Doctor_Username";
            string filterValue = userName.ToLower();

            // Get Patients from SQL Service
            List<PatientObject> patients = _sqlservice.RetrievePatientsFiltered(fieldName, filterValue);

            // For each Patient
            for(int i = 0; i < patients.Count; i++)
            {
                // Get Patient's user data from SQL Service
                UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", patients[i].Username)[0];

                // Merge Data
                patients[i] = patients[0] + temp;
            }

            // Convert Patients to JSON
            string patientsJson = JsonSerializer.Serialize<List<PatientObject>>(patients, new JsonSerializerOptions() { WriteIndented = formatJson });

            return patientsJson;
        }

        public string CreatePatient(NewPatientObject patient)
        {
            // Generate Username for Patient
            patient.Username = _userService.GenerateUsername(patient.Name);

            // Create User record
            _userService.CreateUser(patient);

            // Store User Credentials
            _securityService.SaveNewCredentials(patient.Username, patient.Password);

            // Create Patient record
            _sqlservice.CreatePatient(patient);

            // Return Username so UI has access to it
            return patient.Username;
        }
    }
}
