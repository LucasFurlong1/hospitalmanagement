using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class NewPatientObject : PatientObject
    {
        [JsonInclude]
        public string Password { get; set; }

        public NewPatientObject()
        {
            Password = "";
        }
    }
    public class PatientObject : UserObject
    {
        [JsonInclude]
        public string Doctor_Username { get; set; }
        [JsonInclude]
        public string Last_Interacted { get; set; }

        public PatientObject()
        {
            Doctor_Username = "";
            Last_Interacted = DateTime.MinValue.ToShortDateString();
        }
        public PatientObject(UserObject userObject)
        {
            this.Username = userObject.Username;
            this.Account_Type = userObject.Account_Type;
            this.Name = userObject.Name;
            this.Birth_Date = userObject.Birth_Date;
            this.Gender = userObject.Gender;
            this.Address = userObject.Address;
            this.Phone_Number = userObject.Phone_Number;
            this.Email_Address = userObject.Email_Address;
            this.Emergency_Contact_Name = userObject.Emergency_Contact_Name;
            this.Emergency_Contact_Number = userObject.Emergency_Contact_Number;
            this.Date_Created = userObject.Date_Created;
            Doctor_Username = "";
            Last_Interacted = DateTime.MinValue.ToShortDateString();
        }
        public static PatientObject operator +(PatientObject p, UserObject u)
        {
            p.Username = u.Username;
            p.Account_Type = u.Account_Type;
            p.Name = u.Name;
            p.Birth_Date = u.Birth_Date;
            p.Gender = u.Gender;
            p.Address = u.Address;
            p.Phone_Number = u.Phone_Number;
            p.Email_Address = u.Email_Address;
            p.Emergency_Contact_Name = u.Emergency_Contact_Name;
            p.Emergency_Contact_Number = u.Emergency_Contact_Number;
            p.Date_Created = u.Date_Created;
            return p;
        }

        public override bool Equals(object obj)
        {
            PatientObject patient2 = obj as PatientObject ?? new PatientObject();
            if (patient2 != null
                && Username.Equals(patient2.Username)
                && Doctor_Username.Equals(patient2.Doctor_Username)
                && Last_Interacted.Equals(patient2.Last_Interacted))
            {
                return true;
            }

            return false;
        }
    }
}
