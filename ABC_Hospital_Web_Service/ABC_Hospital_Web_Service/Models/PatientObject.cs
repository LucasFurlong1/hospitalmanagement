using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class NewPatientObject:PatientObject
    {
        [JsonInclude]
        public string? Password { get; set; }
    }
    public class PatientObject:UserObject
    {
        [JsonInclude]
        public string? Doctor_Username { get; set; }
        [JsonInclude]
        public DateTime? Last_Interacted { get; set; }

        public PatientObject()
        {

        }
        public PatientObject(UserObject userObject)
        {
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
        }
        public static PatientObject operator +(PatientObject p, UserObject u)
        {
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
    }
}
