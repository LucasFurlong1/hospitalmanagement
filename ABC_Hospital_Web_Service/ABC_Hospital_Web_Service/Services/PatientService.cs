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

        public PatientService(IConfiguration appConfig, bool format_json = true)
        {
            _securityService = new SecurityService(appConfig);
            _userService = new UserService(appConfig);
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
        }

        public string GetPatientInfo(string userName)
        {
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
                temp.FormatPhoneNumbers();

                // Merge Data
                patient[0] = patient[0] + temp;
            }
            // Convert Patient to JSON
            string patientJson = JsonSerializer.Serialize<List<PatientObject>>(patient, new JsonSerializerOptions() { WriteIndented = formatJson });

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
                temp.FormatPhoneNumbers();

                // Merge Data
                patients[i] = patients[i] + temp;
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
            if(!_userService.CreateUser(patient))
            {
                // If the create failed, return empty string to notify UI
                return "";
            }

            // Store User Credentials
            if(!_securityService.SaveNewCredentials(patient.Username, patient.Password))
            {
                // If the save failed, return empty string to notify UI
                return "";
            }

            // Create Patient record
            if(!_sqlservice.CreatePatient(patient))
            {
                // If the create failed, return empty string to notify UI
                return "";
            }

            // Return Username so UI has access to it
            return patient.Username;
        }
    }
}
