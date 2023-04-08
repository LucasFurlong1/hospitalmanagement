using System;
using System.Text.Json.Serialization;

namespace ABC_Hospital_Web_Service.Models
{
    public class NewDoctorObject : DoctorObject
    {
        [JsonInclude]
        public string? Password { get; set; }
    }
    public class DoctorObject : UserObject
    {
        [JsonInclude]
        public string? Doctor_Department { get; set; }
        [JsonInclude]
        public bool? Is_On_Staff { get; set; }
        [JsonInclude]
        public string? Doctorate_Degree { get; set; }

        public DoctorObject()
        {

        }
        public DoctorObject(UserObject userObject)
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

        public static DoctorObject operator +(DoctorObject d, UserObject u)
        {
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
    }
}
