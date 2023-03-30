using ABC_Hospital_Web_Service.Controllers;
using ABC_Hospital_Web_Service.Models;
using System.Text.Json;

namespace ABC_Hospital_Web_Service.Services
{
    public class UserService
    {
        private SQLInterface _sqlservice;
        private bool formatJson;
        private readonly ILogger<UserController> _logger;

        public UserService(ILogger<UserController> logger, bool format_json = true)
        {
            _sqlservice = new SQLInterface();
            formatJson = format_json;
            _logger = logger;
        }

        public string GetUsers()
        {
            // Get Users from SQL Service
            List<UserObject> users = _sqlservice.RetrieveUsers();

            // Convert Users to JSON
            string usersJson = JsonSerializer.Serialize<List<UserObject>>(users, new JsonSerializerOptions() { WriteIndented = formatJson });

            return usersJson;
        }

        public string GetUsersByAccountType(string accountType)
        {
            // Ensure character is in correct format
            string fieldName = "Account_Type";
            string filterValue = accountType[0].ToString().ToUpper();

            // Get Users from SQL Service
            List<UserObject> users = _sqlservice.RetrieveUsersFiltered(fieldName, filterValue);

            // Convert Users to JSON
            string usersJson = JsonSerializer.Serialize<List<UserObject>>(users, new JsonSerializerOptions() { WriteIndented = formatJson });
            
            return usersJson;
        }

        public string GetUserByUsername(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Username";
            string filterValue = userName.ToLower();

            // Get Users from SQL Service
            UserObject user = _sqlservice.RetrieveUsersFiltered(fieldName, filterValue)[0];

            // Convert Users to JSON
            string userJson = JsonSerializer.Serialize<UserObject>(user, new JsonSerializerOptions() { WriteIndented = formatJson });

            return userJson;
        }
    }

}