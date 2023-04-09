using ABC_Hospital_Web_Service.Models;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace ABC_Hospital_Web_Service.Services
{
    /* To Do:
     * Add better error handling
     * Make database name and connnect string fetchable from appsettings
     * Check all updates and creates sqls for single quotes in field values
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
            searchForDatabase = Path.GetDirectoryName(searchForDatabase);
            while (!System.IO.File.Exists(searchForDatabase + "\\" + databaseName) && searchForDatabase.Length > 4)
            {
                searchForDatabase = Path.GetDirectoryName(searchForDatabase);
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
                    user.Username = username;
                    if (reader.Read())
                    {
                        user.Password = reader.GetString(0);
                    }
                    else
                    {
                        user.Password = "";
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
                "UPDATE [User] SET Password_Hash = '" + user.Password + "' WHERE Username = '" + user.Username + "';";

            RunNonQuerySQL(sqlString);
        }

        public UserSessionObject RetrieveUserIdentitySession(string username)
        {
            string sqlString =
                "SELECT * FROM [Identity_Session] WHERE Username='" + username + "';";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    UserSessionObject session = new UserSessionObject();
                    if (reader.Read())
                    {
                        session.Username = reader.GetString(0);
                        session.SessionStart = reader.GetDateTime(1);
                        session.SessionExpire = reader.GetDateTime(2);
                    }
                    reader.Close();
                    return session;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public void UpdateUserIdentitySession(UserSessionObject session)
        {
            // First check to see if username is valid
            List<UserObject> user = RetrieveUsersFiltered("Username", session.Username);
            
            // If user doesn't exist, no update is needed
            if(user.Count == 0)
            {
                return;
            }


            // Next See if session data exists
            bool firstTime = false;
            string sqlString =
                "SELECT Username FROM [Identity_Session] WHERE Username = '" + session.Username + "';";

            OleDbCommand command = new OleDbCommand();

            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;

            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    string userSessions = "";
                    if (reader.Read())
                    {
                        userSessions = reader.GetString(0);
                    }
                    reader.Close();
                    if (userSessions == "")
                    {
                        firstTime = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // If it is the user's first time, create a new record
            if (firstTime)
            {
                sqlString =
                    "INSERT INTO [Identity_Session] VALUES ('" + session.Username + "', '"
                    + session.SessionStart + "', '" + session.SessionExpire + "');";

                RunNonQuerySQL(sqlString);

            }

            // Otherwise, update the existing record
            else
            {
                sqlString =
                    "UPDATE [Identity_Session] SET Session_Start = '" + session.SessionStart + "', Session_Expire = '" + session.SessionExpire
                    + "' WHERE Username = '" + session.Username + "';";

                RunNonQuerySQL(sqlString);
            }
            return;
        }


        public string CreateNewUsername(string partialUsername)
        {
            string sqlString =
                "SELECT Username FROM [User] WHERE Username LIKE '" + partialUsername + "%' ORDER BY Username DESC;";

            OleDbCommand command = new OleDbCommand();
            command.Connection = DataBaseConnection;
            command.CommandText = sqlString;
            try
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    string digit = "1";
                    if (reader.Read())
                    {
                        string temp = reader.GetString(0);
                        temp = Regex.Replace(temp, "[^.0-9]", "");
                        digit = (int.Parse(temp) + 1).ToString();
                    }
                    reader.Close();
                    return partialUsername + digit;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "";
        }

        public void CreateUser(UserObject user)
        {
            string sqlString = "INSERT INTO [User] VALUES ('" + user.Username + "', '" + user.Account_Type + "', '" + "" + "', '" +
                user.Name + "', '" + user.Birth_Date + "', '" + user.Gender + "', '" +
                user.Address + "', '" + user.Phone_Number + "', '" + user.Email_Address + "', '" + user.Emergency_Contact_Name +
                "', '" + user.Emergency_Contact_Number + "', '" + user.Date_Created + "');";

            RunNonQuerySQL(sqlString);
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


        public void CreatePatient(PatientObject patient)
        {
            string sqlString = "INSERT INTO [Patient] VALUES ('" + patient.Username + "', '" +
                patient.Doctor_Username + "', '" + patient.Last_Interacted + "');";

            RunNonQuerySQL(sqlString);
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


        public void CreateDoctor(DoctorObject doctor)
        {
            string sqlString = "INSERT INTO [Doctor] VALUES ('" + doctor.Username + "', '" +
                doctor.Doctor_Department + "', " + doctor.Is_On_Staff + ", '" + doctor.Doctorate_Degree + "');";

            RunNonQuerySQL(sqlString);
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


        public void CreatePrescription(PrescriptionObject prescription)
        {
            // Escape any single quotes in text fields
            prescription.Medication_Name = prescription.Medication_Name.Replace("'", "''");
            prescription.Dosage = prescription.Dosage.Replace("'", "''");
            prescription.Instructions = prescription.Instructions.Replace("'", "''");

            string sqlString = "INSERT INTO [Prescription] VALUES ('" + prescription.Prescription_ID + "', '" +
                prescription.Patient_Username + "', '" + prescription.Doctor_Username + "', '" + prescription.Medication_Name +
                "', '" + prescription.Prescribed_Date + "', '" + prescription.Dosage + "', '" + prescription.Instructions +
                "', " + prescription.Is_Filled + ");";

            RunNonQuerySQL(sqlString);
        }

        public void UpdatePrescription(PrescriptionObject prescription)
        {
            // Escape any single quotes in text fields
            prescription.Medication_Name = prescription.Medication_Name.Replace("'", "''");
            prescription.Dosage = prescription.Dosage.Replace("'", "''");
            prescription.Instructions = prescription.Instructions.Replace("'", "''");

            string sqlString = "UPDATE [Prescription] SET Medication_Name = '" + prescription.Medication_Name +
                "', Prescribed_Date = '" + prescription.Prescribed_Date + "', Dosage = '" + prescription.Dosage +
                "', Instructions = '" + prescription.Instructions + "', Is_Filled = " + prescription.Is_Filled +
                " WHERE Prescription_ID = '" + prescription.Prescription_ID + "';";

            RunNonQuerySQL(sqlString);
        }

        public void DeletePrescription(string prescription_ID)
        {
            string sqlString = "DELETE FROM [Prescription] WHERE Prescription_ID = '" + prescription_ID + "';";

            RunNonQuerySQL(sqlString);
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

        public void CreateDiagnosis(DiagnosisObject diagnosis)
        {
            // Escape any single quotes in text fields
            diagnosis.Diagnosis_Name = diagnosis.Diagnosis_Name.Replace("'", "''");
            diagnosis.Diagnosis_Description = diagnosis.Diagnosis_Description.Replace("'", "''");
            diagnosis.Diagnosis_Treatment = diagnosis.Diagnosis_Treatment.Replace("'", "''");

            string sqlString = "INSERT INTO [Diagnosis] VALUES ('" + diagnosis.Diagnosis_ID + "', '" +
                diagnosis.Patient_Username + "', '" + diagnosis.Doctor_Username + "', '" + diagnosis.Diagnosis_Name + "', '" +
                diagnosis.Diagnosis_Date + "', '" + diagnosis.Diagnosis_Description + "', '" + diagnosis.Diagnosis_Treatment +
                "', " + diagnosis.Was_Admitted + ", " + diagnosis.Is_Resolved + ");";

            RunNonQuerySQL(sqlString);
        }

        public void UpdateDiagnosis(DiagnosisObject diagnosis)
        {
            // Escape any single quotes in text fields
            diagnosis.Diagnosis_Name = diagnosis.Diagnosis_Name.Replace("'", "''");
            diagnosis.Diagnosis_Description = diagnosis.Diagnosis_Description.Replace("'", "''");
            diagnosis.Diagnosis_Treatment = diagnosis.Diagnosis_Treatment.Replace("'", "''");

            string sqlString = "UPDATE [Diagnosis] SET Diagnosis_Name = '" + diagnosis.Diagnosis_Name +
                "', Diagnosis_Date = '" + diagnosis.Diagnosis_Date + "', Diagnosis_Description = '" +
                diagnosis.Diagnosis_Description + "', Diagnosis_Treatment = '" + diagnosis.Diagnosis_Treatment +
                "', Was_Admitted = " + diagnosis.Was_Admitted + ", Is_Resolved = " + diagnosis.Is_Resolved +
                " WHERE Diagnosis_ID = '" + diagnosis.Diagnosis_ID + "';";

            RunNonQuerySQL(sqlString);
        }

        public void DeleteDiagnosis(string diagnosis_ID)
        {
            string sqlString = "DELETE FROM [Diagnosis] WHERE Diagnosis_ID = '" + diagnosis_ID + "';";

            RunNonQuerySQL(sqlString);
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

        private void RunNonQuerySQL(string sqlString)
        {

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
            }
        }
    }
}
