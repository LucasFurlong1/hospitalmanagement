using ABC_Hospital_Web_Service.Models;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace ABC_Hospital_Web_Service.Services
{
    /* To Do:
     * Check all updates and creates sqls for single quotes in field values
     */
    public class SQLInterface
    {
        private string ConnectString;
        OleDbConnection DataBaseConnection;
        private IConfiguration _appConfig;

        public SQLInterface(IConfiguration appConfig)
        {
            _appConfig = appConfig;
            string databaseName = _appConfig.GetValue<string>("AppSettings:DataBaseName");
            ConnectString = _appConfig.GetValue<string>("AppSettings:AccessConnectString");
            bool found = false;
            string searchForDatabase = System.Reflection.Assembly.GetExecutingAssembly().Location;
            searchForDatabase = Path.GetDirectoryName(searchForDatabase) ?? "";
            while (!System.IO.File.Exists(searchForDatabase + "\\" + databaseName) && searchForDatabase.Length > 3)
            {
                searchForDatabase = Path.GetDirectoryName(searchForDatabase) ?? "";
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
            }
            return null;
        }

        public bool StoreUserCred(UserCredObject user)
        {
            string sqlString =
                "UPDATE [User] SET Password_Hash = '" + user.Password + "' WHERE Username = '" + user.Username + "';";

            if(RunNonQuerySQL(sqlString))
            {
                if(user.Password == RetrieveUserCred(user.Username).Password)
                {
                    return true;
                }
            }
            return false;
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

        public bool UpdateUserIdentitySession(UserSessionObject session)
        {
            UserSessionObject temp;
            string sqlString;

            // First check to see if username is valid
            List<UserObject> user = RetrieveUsersFiltered("Username", session.Username);
            
            // If user doesn't exist, no update is needed
            if(user.Count == 0)
            {
                return false;
            }


            // Next See if session data exists
            bool firstTime = false;
            temp = RetrieveUserIdentitySession(session.Username);
            if (temp == null || temp.Username == null)
            {
                firstTime = true;
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

            // Verify identity session was updated
            temp = RetrieveUserIdentitySession(session.Username);
            if (temp.Equals(session))
            {
                return true;
            }

            return false;
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
            }
            return "";
        }

        public bool CreateUser(UserObject user)
        {
            string sqlString = "INSERT INTO [User] VALUES ('" + user.Username + "', '" + user.Account_Type + "', '" + "" + "', '" +
                user.Name + "', '" + DateTime.Parse(user.Birth_Date) + "', '" + user.Gender + "', '" +
                user.Address + "', '" + user.Phone_Number + "', '" + user.Email_Address + "', '" + user.Emergency_Contact_Name +
                "', '" + user.Emergency_Contact_Number + "', '" + DateTime.Parse(user.Date_Created) + "');";

            if (RunNonQuerySQL(sqlString))
            {
                List<UserObject> temp = RetrieveUsersFiltered("Username", user.Username);
                if (temp.Count > 0 && temp[0].Equals(user))
                {
                    return true;
                }
            }

            return false;
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
                        temp.Birth_Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
                        temp.Gender = reader.GetString(5)[0];
                        temp.Address = reader.GetString(6);
                        temp.Phone_Number = reader.GetString(7);
                        temp.Email_Address = reader.GetString(8);
                        temp.Emergency_Contact_Name = reader.GetString(9);
                        temp.Emergency_Contact_Number = reader.GetString(10);
                        temp.Date_Created = reader.GetDateTime(11).ToString("yyyy-MM-dd");
                        users.Add(temp);
                    }
                    reader.Close();
                    return users;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<UserObject>();

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
                        temp.Birth_Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
                        temp.Gender = reader.GetString(5)[0];
                        temp.Address = reader.GetString(6);
                        temp.Phone_Number = reader.GetString(7);
                        temp.Email_Address = reader.GetString(8);
                        temp.Emergency_Contact_Name = reader.GetString(9);
                        temp.Emergency_Contact_Number = reader.GetString(10);
                        temp.Date_Created = reader.GetDateTime(11).ToString("yyyy-MM-dd");
                        users.Add(temp);
                    }
                    reader.Close();
                    return users;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<UserObject>();
        }


        public bool CreatePatient(PatientObject patient)
        {
            string sqlString = "INSERT INTO [Patient] VALUES ('" + patient.Username + "', '" +
                patient.Doctor_Username + "', '" + patient.Last_Interacted + "');";

            if (RunNonQuerySQL(sqlString))
            {
                List<PatientObject> temp = RetrievePatientsFiltered("Patient_Username", patient.Username);
                if (temp.Count > 0 && temp[0].Equals(patient))
                {
                    return true;
                }
            }

            return false;
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
                        temp.Last_Interacted = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                        patients.Add(temp);
                    }
                    reader.Close();
                    return patients;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<PatientObject>();
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
                        temp.Last_Interacted = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                        patients.Add(temp);
                    }
                    reader.Close();
                    return patients;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<PatientObject>();
        }


        public bool CreateDoctor(DoctorObject doctor)
        {
            string sqlString = "INSERT INTO [Doctor] VALUES ('" + doctor.Username + "', '" +
                doctor.Doctor_Department + "', " + doctor.Is_On_Staff + ", '" + doctor.Doctorate_Degree + "');";

            if (RunNonQuerySQL(sqlString))
            {
                List<DoctorObject> temp = RetrieveDoctorsFiltered("Doctor_Username", doctor.Username);
                if (temp.Count > 0 && temp[0].Equals(doctor))
                {
                    return true;
                }
            }

            return false;
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
            }
            return new List<DoctorObject>();
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
            }
            return new List<DoctorObject>();
        }


        public bool CreatePrescription(PrescriptionObject prescription)
        {
            // Escape any single quotes in text fields
            string med_name = prescription.Medication_Name.Replace("'", "''");
            string dosage = prescription.Dosage.Replace("'", "''");
            string instructions = prescription.Instructions.Replace("'", "''");

            string sqlString = "INSERT INTO [Prescription] VALUES ('" + prescription.Prescription_ID + "', '" +
                prescription.Patient_Username + "', '" + prescription.Doctor_Username + "', '" + med_name +
                "', '" + DateTime.Parse(prescription.Prescribed_Date) + "', '" + dosage + "', '" + instructions +
                "', " + prescription.Is_Filled + ");";

            if (RunNonQuerySQL(sqlString))
            {
                List<PrescriptionObject> temp = RetrievePrescriptionsFiltered("Prescription_ID", prescription.Prescription_ID);
                if (temp.Count > 0 && temp[0].Equals(prescription))
                {
                    return true;
                }
            }

            return false;
        }

        public bool UpdatePrescription(PrescriptionObject prescription)
        {
            // Escape any single quotes in text fields
            string med_name = prescription.Medication_Name.Replace("'", "''");
            string dosage = prescription.Dosage.Replace("'", "''");
            string instructions = prescription.Instructions.Replace("'", "''");

            string sqlString = "UPDATE [Prescription] SET Medication_Name = '" + med_name +
                "', Prescribed_Date = '" + DateTime.Parse(prescription.Prescribed_Date) + "', Dosage = '" + dosage +
                "', Instructions = '" + instructions + "', Is_Filled = " + prescription.Is_Filled +
                " WHERE Prescription_ID = '" + prescription.Prescription_ID + "';";

            if (RunNonQuerySQL(sqlString))
            {
                List<PrescriptionObject> temp = RetrievePrescriptionsFiltered("Prescription_ID", prescription.Prescription_ID);
                if (temp.Count > 0 && temp[0].Equals(prescription))
                {
                    return true;
                }
            }

            return false;
        }

        public bool DeletePrescription(string prescription_ID)
        {
            string sqlString = "DELETE FROM [Prescription] WHERE Prescription_ID = '" + prescription_ID + "';";

            if (RunNonQuerySQL(sqlString))
            {
                List<PrescriptionObject> temp = RetrievePrescriptionsFiltered("Prescription_ID", prescription_ID);
                if (temp.Count == 0)
                {
                    return true;
                }
            }

            return false;
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
                        temp.Prescribed_Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
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
            }
            return new List<PrescriptionObject>();
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
                        temp.Prescribed_Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
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
            }
            return new List<PrescriptionObject>();
        }

        public bool CreateDiagnosis(DiagnosisObject diagnosis)
        {
            // Escape any single quotes in text fields
            string diag_name = diagnosis.Diagnosis_Name.Replace("'", "''");
            string diag_desc = diagnosis.Diagnosis_Description.Replace("'", "''");
            string diag_treatment = diagnosis.Diagnosis_Treatment.Replace("'", "''");

            string sqlString = "INSERT INTO [Diagnosis] VALUES ('" + diagnosis.Diagnosis_ID + "', '" +
                diagnosis.Patient_Username + "', '" + diagnosis.Doctor_Username + "', '" + diag_name + "', '" +
                DateTime.Parse(diagnosis.Diagnosis_Date) + "', '" + diag_desc + "', '" + diag_treatment +
                "', " + diagnosis.Was_Admitted + ", " + diagnosis.Is_Resolved + ");";

            if(RunNonQuerySQL(sqlString))
            {
                List<DiagnosisObject> temp = RetrieveDiagnosesFiltered("Diagnosis_ID", diagnosis.Diagnosis_ID);
                if(temp.Count > 0 && temp[0].Equals(diagnosis))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateDiagnosis(DiagnosisObject diagnosis)
        {
            // Escape any single quotes in text fields
            string diag_name = diagnosis.Diagnosis_Name.Replace("'", "''");
            string diag_desc = diagnosis.Diagnosis_Description.Replace("'", "''");
            string diag_treatment = diagnosis.Diagnosis_Treatment.Replace("'", "''");

            string sqlString = "UPDATE [Diagnosis] SET Diagnosis_Name = '" + diag_name +
                "', Diagnosis_Date = '" + DateTime.Parse(diagnosis.Diagnosis_Date) + "', Diagnosis_Description = '" +
                diag_desc + "', Diagnosis_Treatment = '" + diag_treatment +
                "', Was_Admitted = " + diagnosis.Was_Admitted + ", Is_Resolved = " + diagnosis.Is_Resolved +
                " WHERE Diagnosis_ID = '" + diagnosis.Diagnosis_ID + "';";

            if (RunNonQuerySQL(sqlString))
            {
                List<DiagnosisObject> temp = RetrieveDiagnosesFiltered("Diagnosis_ID", diagnosis.Diagnosis_ID);
                if (temp.Count > 0 && temp[0].Equals(diagnosis))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteDiagnosis(string diagnosis_ID)
        {
            string sqlString = "DELETE FROM [Diagnosis] WHERE Diagnosis_ID = '" + diagnosis_ID + "';";

            if (RunNonQuerySQL(sqlString))
            {
                List<DiagnosisObject> temp = RetrieveDiagnosesFiltered("Diagnosis_ID", diagnosis_ID);
                if (temp.Count == 0)
                {
                    return true;
                }
            }
            return false;
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
                        temp.Diagnosis_Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
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
            }
            return new List<DiagnosisObject>();
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
                        temp.Diagnosis_Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
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
            }
            return new List<DiagnosisObject>();
        }

        private bool RunNonQuerySQL(string sqlString)
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
                return false;
            }
            return true;
        }
    }
}
