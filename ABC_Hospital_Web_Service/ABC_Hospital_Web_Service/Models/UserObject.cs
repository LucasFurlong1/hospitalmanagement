using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class UserObject
    {
        [JsonInclude]
        public string Username { get; set; }
        [JsonInclude]
        public char Account_Type { get; set; }
        [JsonInclude]
        public string Name { get; set; }
        [JsonInclude]
        public string Birth_Date { get; set; }
        [JsonInclude]
        public char Gender { get; set; }
        [JsonInclude]
        public string Address { get; set; }
        [JsonInclude]
        public string Phone_Number { get; set; }
        [JsonInclude]
        public string Email_Address { get; set; }
        [JsonInclude]
        public string Emergency_Contact_Name { get; set; }
        [JsonInclude]
        public string Emergency_Contact_Number { get; set; }
        [JsonInclude]
        public string Date_Created { get; set; }

        public UserObject()
        {
            Username = "";
            Account_Type = ' ';
            Name = "";
            Birth_Date = DateOnly.MinValue.ToShortDateString();
            Gender = ' ';
            Address = "";
            Phone_Number = "";
            Email_Address = "";
            Emergency_Contact_Name = "";
            Emergency_Contact_Number = "";
            Date_Created = DateOnly.MinValue.ToShortDateString();
        }

        public override bool Equals(object obj)
        {
            UserObject user2 = obj as UserObject ?? new UserObject();
            if (user2 != null
                && Username.Equals(user2.Username)
                && Account_Type.Equals(user2.Account_Type)
                && Name.Equals(user2.Name)
                && Birth_Date.Equals(user2.Birth_Date)
                && Gender.Equals(user2.Gender)
                && Address.Equals(user2.Address)
                && Phone_Number.Equals(user2.Phone_Number)
                && Emergency_Contact_Name.Equals(user2.Emergency_Contact_Name)
                && Emergency_Contact_Number.Equals(user2.Emergency_Contact_Number)
                && Date_Created.Equals(user2.Date_Created))
            {
                return true;
            }

            return false;
        }
    }
}
