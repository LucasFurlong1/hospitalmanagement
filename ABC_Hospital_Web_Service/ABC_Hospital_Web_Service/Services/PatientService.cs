using ABC_Hospital_Web_Service.Controllers;
using ABC_Hospital_Web_Service.Models;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class PatientService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;

        public PatientService(bool format_json = true)
        {
            _sqlservice = new SQLInterface();
            formatJson = format_json;
        }

        public string GetPatientInfo(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Patient_Username";
            string filterValue = userName.ToLower();

            // Get Patient from SQL Service
            PatientObject patient = _sqlservice.RetrievePatientsFiltered(fieldName, filterValue)[0];

            // Get Patient's user data from SQL Service
            UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", filterValue)[0];

            // Merge Data
            patient.Account_Type = temp.Account_Type;
            patient.Name = temp.Name;
            patient.Birth_Date = temp.Birth_Date;
            patient.Gender = temp.Gender;
            patient.Address = temp.Address;
            patient.Phone_Number = temp.Phone_Number;
            patient.Email_Address = temp.Email_Address;
            patient.Emergency_Contact_Name = temp.Emergency_Contact_Name;
            patient.Emergency_Contact_Number = temp.Emergency_Contact_Number;
            patient.Date_Created = temp.Date_Created;

            // Convert Patient to JSON
            string patientJson = JsonSerializer.Serialize<PatientObject>(patient, new JsonSerializerOptions() { WriteIndented = formatJson });

            return patientJson;
        }

        public string GetPatientsByDoctor(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Doctor_Username";
            string filterValue = userName.ToLower();

            // Get Patients from SQL Service
            List<PatientObject> patients = _sqlservice.RetrievePatientsFiltered(fieldName, filterValue);

            foreach (PatientObject patient in patients)
            {
                // Get Patients' user data from SQL Service
                UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", patient.Username)[0];

                // Merge Data
                patient.Account_Type = temp.Account_Type;
                patient.Name = temp.Name;
                patient.Birth_Date = temp.Birth_Date;
                patient.Gender = temp.Gender;
                patient.Address = temp.Address;
                patient.Phone_Number = temp.Phone_Number;
                patient.Email_Address = temp.Email_Address;
                patient.Emergency_Contact_Name = temp.Emergency_Contact_Name;
                patient.Emergency_Contact_Number = temp.Emergency_Contact_Number;
                patient.Date_Created = temp.Date_Created;
            }

            // Convert Patients to JSON
            string patientsJson = JsonSerializer.Serialize<List<PatientObject>>(patients, new JsonSerializerOptions() { WriteIndented = formatJson });

            return patientsJson;
        }
    }
}
