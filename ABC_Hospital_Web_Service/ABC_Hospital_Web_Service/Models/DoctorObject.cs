using System;
using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class NewDoctorObject : DoctorObject
    {
        [JsonInclude]
        public string Password { get; set; }

        public NewDoctorObject()
        {
            Password = "";
        }
    }
    public class DoctorObject : UserObject
    {
        [JsonInclude]
        public string Doctor_Department { get; set; }
        [JsonInclude]
        public bool Is_On_Staff { get; set; }
        [JsonInclude]
        public string Doctorate_Degree { get; set; }

        public DoctorObject()
        {
            Doctor_Department = "";
            Is_On_Staff = false;
            Doctorate_Degree = "";
        }
        public DoctorObject(UserObject userObject)
        {
            Username = userObject.Username;
            Account_Type = userObject.Account_Type;
            Name = userObject.Name;
            Birth_Date = userObject.Birth_Date;
            Gender = userObject.Gender;
            Address = userObject.Address;
            Phone_Number = userObject.Phone_Number;
            Email_Address = userObject.Email_Address;
            Emergency_Contact_Name = userObject.Emergency_Contact_Name;
            Emergency_Contact_Number = userObject.Emergency_Contact_Number;
            Date_Created = userObject.Date_Created;
            Doctor_Department = "";
            Is_On_Staff = false;
            Doctorate_Degree = "";
        }

        public static DoctorObject operator +(DoctorObject d, UserObject u)
        {
            d.Username = u.Username;
            d.Account_Type = u.Account_Type;
            d.Name = u.Name;
            d.Birth_Date = u.Birth_Date;
            d.Gender = u.Gender;
            d.Address = u.Address;
            d.Phone_Number = u.Phone_Number;
            d.Email_Address = u.Email_Address;
            d.Emergency_Contact_Name = u.Emergency_Contact_Name;
            d.Emergency_Contact_Number = u.Emergency_Contact_Number;
            d.Date_Created = u.Date_Created;
            return d;
        }

        public override bool Equals(object obj)
        {
            DoctorObject doc2 = obj as DoctorObject ?? new DoctorObject();
            if (doc2 != null
                && Doctor_Department.Equals(doc2.Doctor_Department)
                && Is_On_Staff.Equals(doc2.Is_On_Staff)
                && Doctorate_Degree.Equals(doc2.Doctorate_Degree)
                && Username.Equals(doc2.Username))
            {
                return true;
            }

            return false;
        }
    }
}
