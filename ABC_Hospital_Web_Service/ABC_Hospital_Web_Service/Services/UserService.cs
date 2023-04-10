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
        private JsonSerializerOptions _jsonOptions;

        public UserService(IConfiguration appConfig, bool format_json = true)
        {
            _sqlservice = new SQLInterface(appConfig);
            formatJson = format_json;
            _jsonOptions = new JsonSerializerOptions() { WriteIndented = formatJson };
        }

        public string GetUsers()
        {
            // Get Users from SQL Service
            List<UserObject> users = _sqlservice.RetrieveUsers();

            foreach(UserObject user in users)
            {
                user.FormatPhoneNumbers();
            }

            // Convert Users to JSON
            string usersJson = JsonSerializer.Serialize<List<UserObject>>(users, _jsonOptions);

            return usersJson;
        }

        public string GetUsersByAccountType(string accountType)
        {
            // Ensure character is in correct format
            string fieldName = "Account_Type";
            string filterValue = accountType[0].ToString().ToUpper();

            // Get Users from SQL Service
            List<UserObject> users = _sqlservice.RetrieveUsersFiltered(fieldName, filterValue);

            foreach(UserObject user in users)
            {
                user.FormatPhoneNumbers();
            }

            // Convert Users to JSON
            string usersJson = JsonSerializer.Serialize<List<UserObject>>(users, _jsonOptions);

            return usersJson;
        }

        public string GetUserByUsername(string userName)
        {
            // Prepare filter field and value
            string fieldName = "Username";
            string filterValue = userName.ToLower();

            // Get Users from SQL Service
            List<UserObject> users = _sqlservice.RetrieveUsersFiltered(fieldName, filterValue);

            foreach (UserObject user in users)
            {
                user.FormatPhoneNumbers();
            }

            // Convert Users to JSON
            string userJson = JsonSerializer.Serialize<List<UserObject>>(users, _jsonOptions);

            return userJson;
        }

        public bool CreateUser(UserObject user)
        {
            user.DeformatPhoneNumbers();
            return _sqlservice.CreateUser(user);
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