using ABC_Hospital_Web_Service.Controllers;
using ABC_Hospital_Web_Service.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ABC_Hospital_Web_Service.Services
{
    /* To Do:
     * Need to get username generation working to accept last names like Van Dyke or Smith-Jones
     */
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
        public UserService(bool format_json = true)
        {
            _sqlservice = new SQLInterface();
            formatJson = format_json;
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
            string userJson = "{}";

            // Prepare filter field and value
            string fieldName = "Username";
            string filterValue = userName.ToLower();

            // Get Users from SQL Service
            List<UserObject> user = _sqlservice.RetrieveUsersFiltered(fieldName, filterValue);

            // If User was found, then
            if (user.Count > 0)
            {
                // Convert Users to JSON
                userJson = JsonSerializer.Serialize<UserObject>(user[0], new JsonSerializerOptions() { WriteIndented = formatJson });
            }

            return userJson;
        }

        public void CreateUser(UserObject user)
        {
            _sqlservice.CreateUser(user);
        }

        public string GenerateUsername(string userFullName)
        {
            string username = "";

            // Verify that the name is in a valid format
            if (Regex.IsMatch(userFullName, @"(\w+\s(\w+\.?\s)*\w\w+)"))
            {
                // Split User's full name
                string[] temp = userFullName.Split(" ");

                // Get first character of User's first name
                username = temp.First()[0].ToString();

                // Get up to 7 characters from last name
                if (temp.Last().Length > 7)
                {
                    username += temp.Last().Substring(0, 7);
                }
                else
                {
                    username += temp.Last();
                }
                username = username.ToLower();

                // Find out what digit is needed to make Username unique
                username = _sqlservice.CreateNewUsername(username);
            }

            return username;
        }
    }

}