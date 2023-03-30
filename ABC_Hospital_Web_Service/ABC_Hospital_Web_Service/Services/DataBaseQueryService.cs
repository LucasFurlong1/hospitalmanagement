using ABC_Hospital_Web_Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace ABC_Hospital_Web_Service.Services
{
    public class SQLInterface
    {
        private string ConnectString;
        OleDbConnection DataBaseConnection;
        //private OleDbConnection conn;
        //ConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=C:\\Users\\Matt W\\Desktop\\ABC_Hospital_Database.accdb; Uid = Admin; Pwd =; ",
        public SQLInterface()
        {
            ConnectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\Users\\Matt W\\Desktop\\ABC_Hospital_Database.accdb;Persist Security Info=False;";
            DataBaseConnection = new OleDbConnection(ConnectString);
            DataBaseConnection.Open();
        }

        public List<UserObject> RetrieveUsers()
        {
            string queryString =
                "SELECT * FROM [User] ORDER BY Username;";

            OleDbCommand command = new OleDbCommand();

            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [User] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Patient] ORDER BY Patient_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Patient] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Patient_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Doctor] ORDER BY Doctor_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Doctor] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Doctor_Username;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Prescription] ORDER BY Prescribed_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Prescription] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Prescribed_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Diagnosis] ORDER BY Diagnosis_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
            string queryString =
                "SELECT * FROM [Diagnosis] WHERE " + fieldName + "=\"" + value + "\" ORDER BY Diagnosis_Date;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = queryString;

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
