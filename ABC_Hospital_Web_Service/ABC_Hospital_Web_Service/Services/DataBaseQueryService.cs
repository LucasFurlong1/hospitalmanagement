using ABC_Hospital_Web_Service.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;

namespace ABC_Hospital_Web_Service.Services
{
    /* To Do:
     * Add better error handling
     * Make database name and connnect string fetchable from appsettings
     * Add insert statements for objects
     */
    public class SQLInterface
    {
        private string ConnectString;
        OleDbConnection DataBaseConnection;
        //private IConfiguration Configuration;

        public SQLInterface()
        {
            //Configuration = new IConfiguration();
            string databaseName = "ABC_Hospital_Database.accdb";// Configuration.GetSection("ConnectionStrings")["Database"];
            ConnectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=@;Persist Security Info=False;";// Configuration.GetSection("ConnectionStrings")["AccessConnectString"];
            bool found = false;
            string searchForDatabase = System.Reflection.Assembly.GetExecutingAssembly().Location;
            searchForDatabase = System.IO.Path.GetDirectoryName(searchForDatabase);
            while (!System.IO.File.Exists(searchForDatabase + "\\" + databaseName) && searchForDatabase.Length > 4)
            {
                searchForDatabase = System.IO.Path.GetDirectoryName(searchForDatabase);
            }
            if (System.IO.File.Exists(searchForDatabase + "\\" + databaseName))
            {
                found = true;
                searchForDatabase += "\\" + databaseName;
            }
            if (found)
            {
                ConnectString = ConnectString.Replace("@", searchForDatabase);
                DataBaseConnection = new OleDbConnection(ConnectString);
                DataBaseConnection.Open();
            }
            else
            {
                throw new ArgumentException("Database with the name of " + databaseName + " could not be found. Please verify the name is correct in appsettings.json and that it is in the project's directory.");
            }
        }

        public UserCredObject RetrieveUserCred(string username)
        {
            string sqlString =
                "SELECT Password_Hash FROM [User] WHERE Username='" + username + "';";

            OleDbCommand command = new OleDbCommand();

            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    UserCredObject user = new UserCredObject();
                    while (reader.Read())
                    {
                        user.Username = username;
                        user.Password = reader.GetString(0);
                    }
                    reader.Close();
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public void StoreUserCred(UserCredObject user)
        {
            string sqlString =
                "Update [User] SET Password_Hash = '" + user.Password + "' WHERE Username = '" + user.Username + "';";
            OleDbCommand command = new OleDbCommand();

            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                command.ExecuteNonQuery();
                { }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return;
        }

        public void UpdateUserIdentitySession(string username)
        {
            // First Check to see if session data exists
            bool firstTime = false;
            string sqlString =
                "SELECT Username FROM [Identity_Session] WHERE Username = '" + username + "';";

            OleDbCommand command = new OleDbCommand();

            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<string> userSessions = new List<string>();
                    while (reader.Read())
                    {
                        string temp = reader.GetString(0);
                        userSessions.Add(temp);
                    }
                    reader.Close();
                    if (userSessions.Count < 1)
                    {
                        firstTime = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }

            // If it is the user's first time, create a new record
            if (firstTime)
            {
                sqlString =
                    "INSERT INTO [Identity_Session] VALUES ('" + username + "', '"
                    + DateTime.Now + "', '" + DateTime.Now.AddMinutes(30) + "');";
                command = new OleDbCommand(); //Username = '" + username + "', 

                command.Connection = DataBaseConnection;
                command.CommandText = sqlString;

                try
                {
                    command.ExecuteNonQuery();
                    { }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //return ex.Message;
                }

            }

            // Otherwise, update the existing record
            else
            {
                sqlString =
                    "Update [Identity_Session] SET Session_Start = '" + DateTime.Now + "', Session_Expire = '" + DateTime.Now.AddMinutes(30)
                    + "' WHERE Username = '" + username + "';";
                command = new OleDbCommand(); //Username = '" + username + "', 

                command.Connection = DataBaseConnection;
                command.CommandText = sqlString;

                try
                {
                    command.ExecuteNonQuery();
                    { }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //return ex.Message;
                }
            }
            return;
        }

        public List<UserObject> RetrieveUsers()
        {
            string sqlString =
                "SELECT * FROM [User] ORDER BY Username;";

            OleDbCommand command = new OleDbCommand();

            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<UserObject> users = new List<UserObject>();
                    while (reader.Read())
                    {
                        UserObject temp = new UserObject();
                        temp.Username = reader.GetString(0);
                        temp.Account_Type = reader.GetString(1)[0];
                        temp.Name = reader.GetString(3);
                        temp.Birth_Date = reader.GetDateTime(4);
                        temp.Gender = reader.GetString(5)[0];
                        temp.Address = reader.GetString(6);
                        temp.Phone_Number = reader.GetString(7);
                        temp.Email_Address = reader.GetString(8);
                        temp.Emergency_Contact_Name = reader.GetString(9);
                        temp.Emergency_Contact_Number = reader.GetString(10);
                        temp.Date_Created = reader.GetDateTime(11);
                        users.Add(temp);
                    }
                    reader.Close();
                    return users;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;

        }

        public List<UserObject> RetrieveUsersFiltered(string fieldName, string value)
        {
            string sqlString =
                "SELECT * FROM [User] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<UserObject> users = new List<UserObject>();
                    while (reader.Read())
                    {
                        UserObject temp = new UserObject();
                        temp.Username = reader.GetString(0);
                        temp.Account_Type = reader.GetString(1)[0];
                        temp.Name = reader.GetString(3);
                        temp.Birth_Date = reader.GetDateTime(4);
                        temp.Gender = reader.GetString(5)[0];
                        temp.Address = reader.GetString(6);
                        temp.Phone_Number = reader.GetString(7);
                        temp.Email_Address = reader.GetString(8);
                        temp.Emergency_Contact_Name = reader.GetString(9);
                        temp.Emergency_Contact_Number = reader.GetString(10);
                        temp.Date_Created = reader.GetDateTime(11);
                        users.Add(temp);
                    }
                    reader.Close();
                    return users;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<PatientObject> RetrievePatients()
        {
            string sqlString =
                "SELECT * FROM [Patient] ORDER BY Patient_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<PatientObject> patients = new List<PatientObject>();
                    while (reader.Read())
                    {
                        PatientObject temp = new PatientObject();
                        temp.Username = reader.GetString(0);
                        temp.Doctor_Username = reader.GetString(1);
                        temp.Last_Interacted = reader.GetDateTime(2);
                        patients.Add(temp);
                    }
                    reader.Close();
                    return patients;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<PatientObject> RetrievePatientsFiltered(string fieldName, string value)
        {
            string sqlString =
                "SELECT * FROM [Patient] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Patient_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<PatientObject> patients = new List<PatientObject>();
                    while (reader.Read())
                    {
                        PatientObject temp = new PatientObject();
                        temp.Username = reader.GetString(0);
                        temp.Doctor_Username = reader.GetString(1);
                        temp.Last_Interacted = reader.GetDateTime(2);
                        patients.Add(temp);
                    }
                    reader.Close();
                    return patients;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<DoctorObject> RetrieveDoctors()
        {
            string sqlString =
                "SELECT * FROM [Doctor] ORDER BY Doctor_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<DoctorObject> doctors = new List<DoctorObject>();
                    while (reader.Read())
                    {
                        DoctorObject temp = new DoctorObject();
                        temp.Username = reader.GetString(0);
                        temp.Doctor_Department = reader.GetString(1);
                        temp.Is_On_Staff = reader.GetBoolean(2);
                        temp.Doctorate_Degree = reader.GetString(3);
                        doctors.Add(temp);
                    }
                    reader.Close();
                    return doctors;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<DoctorObject> RetrieveDoctorsFiltered(string fieldName, string value)
        {
            string sqlString =
                "SELECT * FROM [Doctor] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Doctor_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<DoctorObject> doctors = new List<DoctorObject>();
                    while (reader.Read())
                    {
                        DoctorObject temp = new DoctorObject();
                        temp.Username = reader.GetString(0);
                        temp.Doctor_Department = reader.GetString(1);
                        temp.Is_On_Staff = reader.GetBoolean(2);
                        temp.Doctorate_Degree = reader.GetString(3);
                        doctors.Add(temp);
                    }
                    reader.Close();
                    return doctors;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<PrescriptionObject> RetrievePrescriptions()
        {
            string sqlString =
                "SELECT * FROM [Prescription] ORDER BY Prescribed_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<PrescriptionObject> prescriptions = new List<PrescriptionObject>();
                    while (reader.Read())
                    {
                        PrescriptionObject temp = new PrescriptionObject();
                        temp.Prescription_ID = reader.GetString(0);
                        temp.Patient_Username = reader.GetString(1);
                        temp.Doctor_Username = reader.GetString(2);
                        temp.Medication_Name = reader.GetString(3);
                        temp.Prescribed_Date = reader.GetDateTime(4);
                        temp.Dosage = reader.GetString(5);
                        temp.Instructions = reader.GetString(6);
                        temp.Is_Filled = reader.GetBoolean(7);
                        prescriptions.Add(temp);
                    }
                    reader.Close();
                    return prescriptions;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<PrescriptionObject> RetrievePrescriptionsFiltered(string fieldName, string value)
        {
            string sqlString =
                "SELECT * FROM [Prescription] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Prescribed_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<PrescriptionObject> doctors = new List<PrescriptionObject>();
                    while (reader.Read())
                    {
                        PrescriptionObject temp = new PrescriptionObject();
                        temp.Prescription_ID = reader.GetString(0);
                        temp.Patient_Username = reader.GetString(1);
                        temp.Doctor_Username = reader.GetString(2);
                        temp.Medication_Name = reader.GetString(3);
                        temp.Prescribed_Date = reader.GetDateTime(4);
                        temp.Dosage = reader.GetString(5);
                        temp.Instructions = reader.GetString(6);
                        temp.Is_Filled = reader.GetBoolean(7);
                        doctors.Add(temp);
                    }
                    reader.Close();
                    return doctors;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<DiagnosisObject> RetrieveDiagnoses()
        {
            string sqlString =
                "SELECT * FROM [Diagnosis] ORDER BY Diagnosis_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<DiagnosisObject> diagnoses = new List<DiagnosisObject>();
                    while (reader.Read())
                    {
                        DiagnosisObject temp = new DiagnosisObject();
                        temp.Diagnosis_ID = reader.GetString(0);
                        temp.Patient_Username = reader.GetString(1);
                        temp.Doctor_Username = reader.GetString(2);
                        temp.Diagnosis_Name = reader.GetString(3);
                        temp.Diagnosis_Date = reader.GetDateTime(4);
                        temp.Diagnosis_Description = reader.GetString(5);
                        temp.Diagnosis_Treatment = reader.GetString(6);
                        temp.Was_Admitted = reader.GetBoolean(7);
                        temp.Is_Resolved = reader.GetBoolean(8);
                        diagnoses.Add(temp);
                    }
                    reader.Close();
                    return diagnoses;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }

        public List<DiagnosisObject> RetrieveDiagnosesFiltered(string fieldName, string value)
        {
            string sqlString =
                "SELECT * FROM [Diagnosis] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Diagnosis_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    List<DiagnosisObject> diagnoses = new List<DiagnosisObject>();
                    while (reader.Read())
                    {
                        DiagnosisObject temp = new DiagnosisObject();
                        temp.Diagnosis_ID = reader.GetString(0);
                        temp.Patient_Username = reader.GetString(1);
                        temp.Doctor_Username = reader.GetString(2);
                        temp.Diagnosis_Name = reader.GetString(3);
                        temp.Diagnosis_Date = reader.GetDateTime(4);
                        temp.Diagnosis_Description = reader.GetString(5);
                        temp.Diagnosis_Treatment = reader.GetString(6);
                        temp.Was_Admitted = reader.GetBoolean(7);
                        temp.Is_Resolved = reader.GetBoolean(8);
                        diagnoses.Add(temp);
                    }
                    reader.Close();
                    return diagnoses;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return null;
        }
    }
}
