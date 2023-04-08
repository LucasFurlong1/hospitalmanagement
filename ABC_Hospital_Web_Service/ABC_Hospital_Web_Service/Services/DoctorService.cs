using ABC_Hospital_Web_Service.Controllers;
using ABC_Hospital_Web_Service.Models;
using System.Numerics;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class DoctorService
    {
        private SecurityService _securityService;
        private UserService _userService;
        private SQLInterface _sqlservice;
        private bool formatJson;

        public DoctorService(bool format_json = true)
        {
            _securityService = new SecurityService();
            _userService = new UserService();
            _sqlservice = new SQLInterface();
            formatJson = format_json;
        }

        public string GetDoctorInfo(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Doctor_Username";
            string filterValue = userName.ToLower();

            // Get Doctor from SQL Service
            DoctorObject doctor = _sqlservice.RetrieveDoctorsFiltered(fieldName, filterValue)[0];

            // Get Doctor's user data from SQL Service
            UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", filterValue)[0];

            // Merge Data
            doctor.Account_Type = temp.Account_Type;
            doctor.Name = temp.Name;
            doctor.Birth_Date = temp.Birth_Date;
            doctor.Gender = temp.Gender;
            doctor.Address = temp.Address;
            doctor.Phone_Number = temp.Phone_Number;
            doctor.Email_Address = temp.Email_Address;
            doctor.Emergency_Contact_Name = temp.Emergency_Contact_Name;
            doctor.Emergency_Contact_Number = temp.Emergency_Contact_Number;
            doctor.Date_Created = temp.Date_Created;

            // Convert Doctor to JSON
            string doctorJson = JsonSerializer.Serialize<DoctorObject>(doctor, new JsonSerializerOptions() { WriteIndented = formatJson });

            return doctorJson;
        }

        public string GetAcceptingDoctors()
        {
            // Get Doctors from SQL Service
            List<DoctorObject> doctors = _sqlservice.RetrieveDoctors();

            // Get Doctors' user data from SQL Service
            foreach (DoctorObject doctor in doctors)
            {
                UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", doctor.Username)[0];

                // Merge Data
                doctor.Account_Type = temp.Account_Type;
                doctor.Name = temp.Name;
                doctor.Birth_Date = temp.Birth_Date;
                doctor.Gender = temp.Gender;
                doctor.Address = temp.Address;
                doctor.Phone_Number = temp.Phone_Number;
                doctor.Email_Address = temp.Email_Address;
                doctor.Emergency_Contact_Name = temp.Emergency_Contact_Name;
                doctor.Emergency_Contact_Number = temp.Emergency_Contact_Number;
                doctor.Date_Created = temp.Date_Created;
            }

            // Convert Doctors to JSON
            string doctorJson = JsonSerializer.Serialize<List<DoctorObject>>(doctors, new JsonSerializerOptions() { WriteIndented = formatJson });

            return doctorJson;
        }

        public string CreateDoctor(NewDoctorObject doctor)
        {
            doctor.Username = _userService.GenerateUsername(doctor.Name);

            // Create User record
            _userService.CreateUser(doctor);

            // Store User Credentials
            _securityService.SaveNewCredentials(doctor.Username, doctor.Password);

            // Create Doctor record
            _sqlservice.CreateDoctor(doctor);

            return doctor.Username;
        }
    }
}
