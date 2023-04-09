using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
            Birth_Date = DateOnly.MinValue.ToString("yyyy-MM-dd");
            Gender = ' ';
            Address = "";
            Phone_Number = "";
            Email_Address = "";
            Emergency_Contact_Name = "";
            Emergency_Contact_Number = "";
            Date_Created = DateOnly.MinValue.ToString("yyyy-MM-dd");
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

        public void FormatPhoneNumbers()
        {
            if(Phone_Number.Length < 10 || Emergency_Contact_Number.Length < 10)
            {
                return;
            }

            if(Phone_Number.Length > 10)
            {
                Phone_Number = deformatPhoneNum(Phone_Number);
            }
            Phone_Number = formatPhoneNum(Phone_Number);

            if (Emergency_Contact_Number.Length > 10)
            {
                Emergency_Contact_Number = deformatPhoneNum(Emergency_Contact_Number);
            }
            Emergency_Contact_Number = formatPhoneNum(Emergency_Contact_Number);

        }

        private string formatPhoneNum(string num)
        {
            num = num.Insert(6, "-");
            num = num.Insert(3, ") ");
            num = num.Insert(0, "(");
            return num;
        }

        public void DeformatPhoneNumbers()
        {
            Phone_Number = Regex.Replace(Phone_Number, "[^.0-9]", "");
            Emergency_Contact_Number = Regex.Replace(Emergency_Contact_Number, "[^.0-9]", "");
        }

        private string deformatPhoneNum(string num)
        {
            return Regex.Replace(num, "[^.0-9]", "");
        }
    }
}
