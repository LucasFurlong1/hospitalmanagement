using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class DoctorService
    {
        private SecurityService _securityService;
        private UserService _userService;
        private SQLInterface _sqlservice;
        private bool formatJson;

        public DoctorService(IConfiguration appConfig, bool format_json = true)
        {
            _securityService = new SecurityService(appConfig);
            _userService = new UserService(appConfig);
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
        }

        public string GetDoctorInfo(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Doctor_Username";
            string filterValue = userName.ToLower();

            // Get Doctor from SQL Service
            List<DoctorObject> doctor = _sqlservice.RetrieveDoctorsFiltered(fieldName, filterValue);

            // If Doctor was found, then
            if (doctor.Count > 0)
            {
                // Get Doctor's user data from SQL Service
                UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", filterValue)[0];
                temp.FormatPhoneNumbers();

                // Merge Data
                doctor[0] = doctor[0] + temp;
            }

            // Convert Doctor to JSON
            string doctorJson = JsonSerializer.Serialize<List<DoctorObject>>(doctor, new JsonSerializerOptions() { WriteIndented = formatJson });

            return doctorJson;
        }

        public string GetAcceptingDoctors()
        {
            // Get Doctors from SQL Service
            List<DoctorObject> doctors = _sqlservice.RetrieveDoctors();

            // For each Doctor,
            for(int i = 0; i < doctors.Count; i++)
            {
                // Get Doctor's user data from SQL Service
                UserObject temp = _sqlservice.RetrieveUsersFiltered("Username", doctors[i].Username)[0];
                temp.FormatPhoneNumbers();

                // Merge Data
                doctors[i] = doctors[i] + temp;
            }

            // Convert Doctors to JSON
            string doctorJson = JsonSerializer.Serialize<List<DoctorObject>>(doctors, new JsonSerializerOptions() { WriteIndented = formatJson });

            return doctorJson;
        }

        public string CreateDoctor(NewDoctorObject doctor)
        {
            // Generate Username for Doctor
            doctor.Username = _userService.GenerateUsername(doctor.Name);

            // Create User record
            if(!_userService.CreateUser(doctor))
            {
                // If the create failed, return empty string to notify UI
                return "";
            }

            // Store User Credentials
            if(!_securityService.SaveNewCredentials(doctor.Username, doctor.Password))
            {
                // If the save failed, return empty string to notify UI
                return "";
            }


            // Create Doctor record
            if (!_sqlservice.CreateDoctor(doctor))
            {
                // If the create failed, return empty string to notify UI
                return "";
            }

            // Return Username so UI has access to it
            return doctor.Username;
        }
    }
}
