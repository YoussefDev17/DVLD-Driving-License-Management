using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Security.Policy;
using System.Diagnostics;

namespace DataBaseTier
{
    public class ClsPersonDB
    {
        static public DataTable GetAllPeople()
        {
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);
            DataTable dt = new DataTable();

            string query = @"select People.PersonID , People.NationalNo , People.FirstName , People.SecondName , People.ThirdName , People.LastName , People.DateOfBirth , 
                            case
                            when People.Gendor = 'M' then 'Male'
                            Else 'Female'
                            End as GendorCaption ,
                            People.Address , People.Phone , People.Email , Countries.CountryName , People.ImagePath 
                            from People inner join Countries on People.NationalityCountryID = Countries.CountryID 
                            ORDER BY People.FirstName;";

            SqlCommand cmd = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);


                }
                reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message , EventLogEntryType.Error);
                return null;

            }
            finally { Connection.Close(); }

            return dt;

        }

        static public bool IsNationaNoExist(string NationalNO)
        {
            bool IsExist = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select Found = 1 from People where NationalNO = @NationalNO";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("NationalNO", NationalNO);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsExist = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }

            finally { Connection.Close(); }

            return IsExist;
        }

        static public bool IsPersonIDExist(int PersonID)
        {
            bool IsExist = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select Found = 1 from People where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsExist = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally { Connection.Close(); }

            return IsExist;
        }

        static public int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                                        DateTime DateOfBirth, char Gendor, string Address, string PhoneNumber, string Email, int NationalCountryID, string ImagePath)
        {
            int PersonID = -1;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Insert Into People " +
                           "values (@NationalNo , @FirstName , @SecondName , @ThirdName , @LastName , @DateOfBirth , @Gendor , @Address , @PhoneNumber , @Email , @NationalCountryID , @ImagePath) " +
                           " SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName != "" && ThirdName != null)
            {
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            if (Email != "" && Email != null)
            {
                Command.Parameters.AddWithValue("@Email", Email);
            }
            else Command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            Command.Parameters.AddWithValue("@NationalCountryID", NationalCountryID);
            if (ImagePath != "" && ImagePath != null)
            {
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }

            }
            catch (Exception ex) { return PersonID; }
            finally { Connection.Close(); }

            return PersonID;
        }

        static public bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                                        DateTime DateOfBirth, char Gendor, string Address, string PhoneNumber, string Email, int NationalCountryID, string ImagePath)
        {
            int RowAffected = 0;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Update People
                            SET NationalNo = @NationalNo, 
                            FirstName = @FirstName ,
                            SecondName = @SecondName ,
                            ThirdName = @ThirdName ,
                            LastName = @LastName ,
                            DateOfBirth = @DateOfBirth ,
                            Gendor = @Gendor ,
                            Address = @Address ,
                            Phone = @PhoneNumber ,
                            Email = @Email ,
                            NationalityCountryID = @NationalCountryID ,
                            ImagePath = @ImagePath 
                            Where PersonID = @PersonID";


            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNO", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName != "" && ThirdName != null)
            {
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);

            }
            else
            {
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value) ;

            }
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            if (Email != "" && Email != null)
            {
                Command.Parameters.AddWithValue("@Email", Email);

            }
            else
            {
                Command.Parameters.AddWithValue("@Email", DBNull.Value);

            }
            Command.Parameters.AddWithValue("@NationalCountryID", NationalCountryID);
            if (ImagePath != "" && ImagePath != null)
            {
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);

            }
            else
            {
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            }

            try
            {
                connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return (RowAffected > 0);
        }
        static public bool FindPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
                                        ref DateTime DateOfBirth, ref char Gendor, ref string Address, ref string PhoneNumber, ref string Email, ref int NationalCountryID, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from People " +
                           "where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    NationalNo = (string)Reader["NationalNo"];
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];
                    ThirdName = (Reader["ThirdName"] == DBNull.Value) ? "" : (string)Reader["ThirdName"];
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = Convert.ToChar(Reader["Gendor"].ToString());
                    Address = (string)Reader["Address"];
                    PhoneNumber = (string)Reader["Phone"];
                    Email = (Reader["Email"] == DBNull.Value) ? "" : (string)Reader["Email"];
                    NationalCountryID = (int)Reader["NationalityCountryID"];
                    ImagePath = (Reader["ImagePath"] == DBNull.Value) ? "" : (string)Reader["ImagePath"];

                    IsFound = true;

                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { connection.Close(); }
            return IsFound;
        }
        static public bool FindPersonByNationalNo(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
                                        ref DateTime DateOfBirth, ref char Gendor, ref string Address, ref string PhoneNumber, ref string Email, ref int NationalCountryID, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from People " +
                           "where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];
                    ThirdName = (Reader["ThirdName"] == DBNull.Value) ? "" : (string)Reader["ThirdName"];
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = Convert.ToChar(Reader["Gendor"].ToString());
                    Address = (string)Reader["Address"];
                    PhoneNumber = (string)Reader["Phone"];
                    Email = (Reader["Email"] == DBNull.Value) ? "" : (string)Reader["Email"];
                    NationalCountryID = (int)Reader["NationalityCountryID"];
                    ImagePath = (Reader["ImagePath"] == DBNull.Value) ? "" : (string)Reader["ImagePath"];

                    IsFound = true;

                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { connection.Close(); }
            return IsFound;
        }
        static public bool DeletePersonByPersonID(int PersonID)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Delete From People where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return RowAffected > 0;

        }

        static public int GetPersonIDByNationalNo(string NationalNo)
        {
            int PersonID = 0;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select PersonID from People 
                            where NationalNo = @NationalNo ;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("NationalNo", NationalNo);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && decimal.TryParse(Result.ToString(), out decimal insertedID))
                {
                    PersonID = Convert.ToInt32(insertedID);
                }

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return PersonID; 
            }
            finally { Connection.Close(); }

            return PersonID;

        }
    }

    public class ClsCountryDB
    {
        static public DataTable GetAllCountriesName()
        {
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);
            DataTable dt = new DataTable();
            string query = "Select CountryName from Countries";
            SqlCommand cmd = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);


                }
                reader.Close();


            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;
        }

        static public bool FindCountryByID(int CountryID ,ref string CountryName)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Countries where CountryID = @CountryID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    CountryName = (string)Reader["CountryName"];

                    IsFound = true;

                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound;
            }
            finally { Connection.Close(); }

            return IsFound;
        }

        static public bool FindCountryByName(string CountryName,ref  int CountryID )
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Countries where CountryName = @CountryName";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    CountryID = (int)Reader["CountryID"];

                    IsFound = true;

                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { Connection.Close(); }

            return IsFound;

        }
    
    }

    public class ClsUserDB
    {
        static public bool IsUserNameAndPasswordExist(string UserName, string Password)
        {
            bool IsUserExist = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Select IsExit = 'Yes' from Users 
                            where UserName = @UserName and Password = @Password";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                IsUserExist = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsUserExist;
            }
            finally { Connection.Close(); }

            return IsUserExist;
        }

        static public bool IsUserNameActive(string UserName)
        {
            bool IsUserActive = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select IsActive from Users 
                            Where UserName = @UserName and IsActive = 1";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsUserActive = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsUserActive; 
            }
            finally { Connection.Close(); }

            return IsUserActive;
        }

        static public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Select Users.UserID , Users.PersonID , FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') + ' ' + People.LastName , Users.UserName ,Users.IsActive
                            from Users INNER JOIN People ON Users.PersonID = People.PersonID   ;";

            SqlCommand cmd = new SqlCommand(query, Connection);

            try
            {

                Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;
        }

        static public bool IsUserExistForPersonID(int PersonID)
        {
            bool IsPersonIDExist = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Found = 1 from Users 
                            where PersonID = @PersonID ;";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = command.ExecuteReader();
                IsPersonIDExist = Reader.HasRows;

                Reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsPersonIDExist; 
            }
            finally { Connection.Close(); }

            return IsPersonIDExist;
        }

        static public int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Insert Into Users
                            Values (@PersonID , @UserName , @Password , @IsActive)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return UserID; 
            }
            finally { Connection.Close(); }

            return UserID;
        }

        static public bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE Users
                            SET PersonID = @PersonID ,
                            UserName = @UserName ,
                            Password = @Password ,
                            IsActive = @IsActive 
                            WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return (RowAffected > 0);
        }

        static public bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Users where UserID = @UserID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];

                    IsFound = true;

                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { Connection.Close(); }

            return IsFound;
        }

        static public bool GetUserInfoByUsernameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Users where UserName = @UserName and Password = @Password";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    PersonID = (int)Reader["PersonID"];
                    UserID = (int)Reader["UserID"];
                    IsActive = (bool)Reader["IsActive"];

                    IsFound = true;

                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { Connection.Close(); }

            return IsFound;

        }

        static public bool DeleteUserByUserID(int UserID)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Delete From Users Where UserID = @UserID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return (RowAffected > 0);
        }

        static public bool IsUserExist(string UserName)
        {
            bool IsUserNameExist = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Found = 1 from Users 
                            where UserName = @UserName ;";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                Connection.Open();

                SqlDataReader Reader = command.ExecuteReader();
                IsUserNameExist = Reader.HasRows;

                Reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsUserNameExist; 
            }
            finally { Connection.Close(); }

            return IsUserNameExist;
        }
  
    }

    public class ClsApplicationTypesDB
    {
        static public DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from ApplicationTypes ";

            SqlCommand command = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);


                }
                reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;
        }

        static public bool FindApplicationTypeByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from ApplicationTypes where ApplicationTypeID = @ApplicationTypeID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationFees = (decimal)Reader["ApplicationFees"];

                }
                Reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return IsFound;
        }

        static public bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            int RowAffected = 0;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE ApplicationTypes
                            SET ApplicationTypeTitle = @ApplicationTypeTitle ,
                                ApplicationFees = @ApplicationFees                               
                            WHERE ApplicationTypeID = @ApplicationTypeID ;";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();
                RowAffected = command.ExecuteNonQuery();

            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return (RowAffected > 0);
        }

        static public int GetApplicationFeesUsingApplicationTypeID(int ApplicationTypeID)
        {
            int ApplicationFees = 0;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select ApplicationFees from ApplicationTypes 
                            where ApplicationTypeID = @ApplicationTypeID ;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && decimal.TryParse(Result.ToString(), out decimal insertedID))
                {
                    ApplicationFees = Convert.ToInt32(insertedID);
                }

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return ApplicationFees; 
            }
            finally { Connection.Close(); }

            return ApplicationFees;
        }
    }

    public class ClsTestTypesDB
    {
        static public DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from TestTypes order by TestTypeID";

            SqlCommand command = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);


                }
                reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;
        }

        static public bool FindTestTypeByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref double TestTypeFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from TestTypes where TestTypeID = @TestTypeID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    TestTypeTitle = (string)Reader["TestTypeTitle"];
                    TestTypeDescription = (string)Reader["TestTypeDescription"];
                    TestTypeFees = (double)(decimal)Reader["TestTypeFees"];

                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }
            finally { connection.Close(); }

            return IsFound;
        }

        static public bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, double TestTypeFees)
        {
            int RowAffected = 0;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE TestTypes
                            SET TestTypeTitle = @TestTypeTitle ,
                                TestTypeDescription = @TestTypeDescription ,
                                TestTypeFees = @TestTypeFees
                            WHERE TestTypeID = @TestTypeID ;";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();
                RowAffected = command.ExecuteNonQuery();

            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }
            finally { Connection.Close(); }

            return (RowAffected > 0);
        }


    }

    public class ClsLicenseClasseDB
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID,
            ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
            ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
            ref string ClassDescription, ref byte MinimumAllowedAge,
           ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    LicenseClassID = (int)reader["LicenseClassID"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }



        public static DataTable GetAllLicenseClasses()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "SELECT * FROM LicenseClasses order by ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Insert Into LicenseClasses 
           (
            ClassName,ClassDescription,MinimumAllowedAge, 
            DefaultValidityLength,ClassFees)
                            Values ( 
            @ClassName,@ClassDescription,@MinimumAllowedAge, 
            @DefaultValidityLength,@ClassFees)
                            where LicenseClassID = @LicenseClassID;
                            SELECT SCOPE_IDENTITY();";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", ClassFees);



            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseClassID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return -1;

            }

            finally
            {
                connection.Close();
            }


            return LicenseClassID;

        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Update  LicenseClasses  
                            set ClassName = @ClassName,
                                ClassDescription = @ClassDescription,
                                MinimumAllowedAge = @MinimumAllowedAge,
                                DefaultValidityLength = @DefaultValidityLength,
                                ClassFees = @ClassFees
                                where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@ClassName", ClassName);
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", ClassFees);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


    
    }

    public class ClsApplicationDB
    {
        static public int AddNewApplication(int ApplicationPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatByUserID)
        {
            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"INSERT INTO Applications
                            VALUES (@ApplicationPersonID , @ApplicationDate , @ApplicationTypeID , @ApplicationStatus , @LastStatusDate , @PaidFees , @CreatByUserID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("ApplicationPersonID", ApplicationPersonID);
            command.Parameters.AddWithValue("ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("PaidFees", PaidFees);
            command.Parameters.AddWithValue("CreatByUserID", CreatByUserID);

            try
            {
                Connection.Open();
                object Result = command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    ApplicationID = insertedID;
                }

            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return ApplicationID;
            }
            finally { Connection.Close(); }

            return ApplicationID;
        }

        static public bool UpdateApplication(int ApplicationID, int ApplicationPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatByUserID)
        {
            int RowAffeted = 0;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE Applications
                             SET ApplicantPersonID = @ApplicationPersonID, 
                            ApplicationDate = @ApplicationDate ,
                            ApplicationTypeID = @ApplicationTypeID ,
                            ApplicationStatus = @ApplicationStatus ,
                            LastStatusDate = @LastStatusDate ,
                            PaidFees = @PaidFees ,
                            CreatedByUserID = @CreatByUserID 
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationPersonID", ApplicationPersonID);
            command.Parameters.AddWithValue("ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("ApplicationStatus", ApplicationStatus); 
            command.Parameters.AddWithValue("LastStatusDate", LastStatusDate); 
            command.Parameters.AddWithValue("PaidFees", PaidFees);
            command.Parameters.AddWithValue("CreatByUserID", CreatByUserID);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);

            try
            { 
                connection.Open();
                RowAffeted = command.ExecuteNonQuery();
            
            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }
            finally { connection.Close(); }

            return (RowAffeted > 0);
        }

        static public bool FindApplicationByID(int ApplicationID,ref int ApplicationPersonID,ref DateTime ApplicationDate,ref int ApplicationTypeID,ref byte ApplicationStatus,ref DateTime LastStatusDate,ref float PaidFees,ref int CreatByUserID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Applications where ApplicationID = @ApplicationID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = (float)(Decimal)Reader["PaidFees"];
                    CreatByUserID= (int)Reader["CreatedByUserID"];
                    

                }
                Reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return IsFound;
        }

        static public bool DeleteApplicationByID(int ApplicationID)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Delete From Applications where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return RowAffected > 0;

        }

        static public int GetActiveApplicationIDForLicenseClass(int PersonID , int ApplicationTypeID , int LicenseClassID)
        {
            int ActiveApplicationID = -1;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            and ApplicationTypeID=@ApplicationTypeID 
							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            and ApplicationStatus=1";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int AppID))
                {
                    ActiveApplicationID = AppID;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return ActiveApplicationID;
            }
            finally
            {
                Connection.Close();
            }

            return ActiveApplicationID;
        }

        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @LastStatusDate
                            where ApplicationID=@ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@NewStatus", NewStatus);
            command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
    }

    public class ClsLocalDrivingLicenseApplicationDB
    {
        static public int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications
                            Values (@ApplicationID , @LicenseClassID)
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return LocalDrivingLicenseApplicationID;
            }
            finally { Connection.Close(); }

            return LocalDrivingLicenseApplicationID;
        }
        static public bool CheckIsPersonApplicationAlreadyHaveLicenseClass(int PersonID, int LicenseClassID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Found = 1 
                            from LocalDrivingLicenseApplications inner join Applications
                            ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID 
                            WHERE ApplicantPersonID = @PersonID and LicenseClassID = @LicenseClassID AND (ApplicationStatus = 1 OR ApplicationStatus = 3); ";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("PersonID", PersonID);
            Command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsExist = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsExist;
            }
            finally { Connection.Close(); }

            return IsExist;
        }
        static public DataTable GetAllLocalDrivingLicensesApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

          

            string query = "Select * from LocalDrivingLicenseApplications_View order by ApplicationDate Desc";


            SqlCommand command = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);


                }
                reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;
        }
        static public bool FindLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID , ref int ApplicationID ,ref int LicenseClassID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];

                }
                Reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return IsFound;
        }
        static public bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Delete From LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return RowAffected > 0;
        }

        static public bool UpdateLocalDrivingLicenseApplication(int  LocalDrivingLicenseApplicationID , int ApplicationID, int LicenseClassID)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE LocalDrivingLicenseApplications
                            SET ApplicationID = @ApplicationID ,
                            LicenseClassID = @LicenseClassID                            
                            Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return (RowAffected > 0);
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {

            bool Result = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @" SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                {
                    Result = returnedResult;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return Result;

        }

        public static bool ThereIsAnActiveTestAppointement(int LocalDrivingLicenseID , int TestTypeID)
        {
            bool IsThereActiveTest = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Found = 1 
                            from TestAppointments inner join LocalDrivingLicenseApplications on TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                            where TestAppointments.TestTypeID = @TestTypeID and TestAppointments.IsLocked = 0 and LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =@LocalDrivingLicenseID 
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query , Connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseID", LocalDrivingLicenseID);

            try
            {
                Connection.Open();
                object result = command.ExecuteScalar();


                if (result != null)
                {
                    IsThereActiveTest = true;
                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }
            finally
            {
                Connection.Close();
            }

            return IsThereActiveTest;
        }

        public static byte TotalTrialPerTest(int LocalDrivingLicenseApplicationID , int TestTypeID)
        {
            byte TotalTrialsPerTest = 0;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select COUNT(TestID) as [Number Trial] from TestAppointments
                             inner join LocalDrivingLicenseApplications on TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             inner join Tests on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestAppointments.TestTypeID = @TestTypeID ;
                            ";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open ();

                //Trials = (byte) command.ExecuteScalar();
                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte Trials))
                {
                    TotalTrialsPerTest = Trials;
                }
            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);
                
            }
            finally
            {
                Connection.Close();
            }

            return TotalTrialsPerTest;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsFound = true;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

            }

            finally
            {
                connection.Close();
            }

            return IsFound;

        }

    }

    public class ClsTestAppointmentsDB
    {
        static public int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID = -1)
        {
            int TestAppointementID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"INSERT INTO TestAppointments
                             VALUES (@TestTypeID , @LocalDrivingLicenseApplicationID , @AppointmentDate , @PaidFees , @CreatedByUserID , @IsLocked , @RetakeTestApplicationID)
                             SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestApplicationID != -1)
            {
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            }
            else Command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    TestAppointementID = insertedID;
                }

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return TestAppointementID; 
            }
            finally { Connection.Close(); }

            return TestAppointementID;
        }

        static public bool FindTestAppointmentByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from TestAppointments where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    TestTypeID = (int)Reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = (decimal)Reader["PaidFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    if (Reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestApplicationID = -1;
                    else
                        RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];
                    IsFound = true;

                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { Connection.Close(); }

            return IsFound;
        }

        static public bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE TestAppointments
                            SET TestTypeID = @TestTypeID ,
                            LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID ,
                            AppointmentDate = @AppointmentDate ,
                            PaidFees = @PaidFees ,
                            CreatedByUserID = @CreatedByUserID ,
                            IsLocked = @IsLocked ,
                            RetakeTestApplicationID = @RetakeTestApplicationID
                            WHERE TestAppointmentID = @TestAppointmentID ;";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestApplicationID == -1)

                Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { Connection.Close(); }

            return (RowAffected > 0);
        }

        static public DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Select 'Appointment ID' = TestAppointments.TestAppointmentID ,
                            'Appointment Date' = TestAppointments.AppointmentDate ,
                            'Paid Fees' = TestAppointments.PaidFees ,
                            'Is Locked' = TestAppointments.IsLocked                        
                            from TestAppointments
                            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID 
                            Order By TestAppointmentID Desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);


                }
                reader.Close();
            }
            catch (System.Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null;
            }
            finally { connection.Close(); }

            return dt;
        }

        static public bool CheckPersonWithLocalDrivingLicenseApplicationIDHasTestAppointment(int LocalDrivingLicenseApplicationID)
        {
            bool HasAppointment = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select 'Found' = 1 from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                HasAppointment = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }

            finally { Connection.Close(); }

            return HasAppointment;
        }
        static public bool CheckPersonWithLocalDrivingLicenseApplicationIDIsLockedTheTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsLocked = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Select 'Found' = 1 From TestAppointments
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID and IsLocked = 1 ; ";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsLocked = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }

            finally { Connection.Close(); }

            return IsLocked;
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return -1;

            }

            finally
            {
                connection.Close();
            }


            return TestID;

        }

    }
       
    public class ClsTestDB
    {
        public static bool GetTestInfoByID(int TestID,
            ref int TestAppointmentID, ref bool TestResult,
            ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"] == DBNull.Value)

                        Notes = "";
                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public int AddNewTestResult(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"INSERT INTO Tests
                             VALUES (@TestAppointmentID , @TestResult , @Notes , @CreatedByUserID ) ;
                             Update TestAppointments 
                             Set IsLocked = 1 where TestAppointmentID = @TestAppointmentID
                             
                             SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            if (Notes != "")
            {
                Command.Parameters.AddWithValue("@Notes", Notes);

            }
            else Command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }


            }
            catch (System.Exception e) { return TestID; }
            finally { Connection.Close(); }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
             string Notes, int CreatedByUserID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Update  Tests  
                            set TestAppointmentID = @TestAppointmentID,
                                TestResult=@TestResult,
                                Notes = @Notes,
                                CreatedByUserID=@CreatedByUserID
                                where TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        static public bool CheckThePersonHasFailedTheTest(int TestAppointmentID)
        {
            bool HasFailed = false;

            SqlConnection connection = new SqlConnection (ConnectionSetting.ConnectionString);

            string query = @"select Tests.TestResult from Tests
                             where TestAppointmentID = @TestAppointmentID and TestResult = 0 ; ";

            SqlCommand Command = new SqlCommand (query, connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                HasFailed = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }
            
            return HasFailed;

        }

        static public byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @" Select PassedTestCount = count(TestResult)
                              from Tests Inner Join TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                              Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                              And TestResult = 1 ;
                             ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

            }

            finally
            {
                connection.Close();
            }

            return PassedTestCount;


        }

        static public bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID , int LicenseClassID , int TestTypeID ,ref int TestID , ref int TestAppointmentID ,
                                                               ref bool TestResult ,ref string Notes ,ref int CreatedByUserID )
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"SELECT  top 1 Tests.TestID, 
                Tests.TestAppointmentID, Tests.TestResult, 
			    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                WHERE        (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND ( TestAppointments.TestTypeID=@TestTypeID)
                ORDER BY Tests.TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;
                    TestID = (int)reader["TestID"];
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"] == DBNull.Value)

                        Notes = "";
                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine(""Error: "" + ex.Message);

                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }
    }

    public class ClsDriversDB
    {
        static public int AddNewDriver(int PersonID , int CreatedByUserID , DateTime CreatedDate)
        {
            int DriverID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Insert Into Drivers
                            Values (@PersonID , @CreatedByUserID , @CreatedDate )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return DriverID; 
            }
            finally { Connection.Close(); }

            return DriverID;

        }

        static public bool CheckThePersonIsADriver(int PersonID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Found = 1 
                             From Drivers WHERE PersonID = @PersonID ;
                            ";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsExist = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsExist; 
            }
            finally { Connection.Close(); }

            return IsExist;


        }
        static public bool GetDriverInfoByPersonID(int PersonID , ref int DriverID , ref int CreatedByUserID , ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Drivers where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];

                    IsFound = true;
                }

            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound;
            }
            finally { Connection.Close(); }

            return IsFound;

        }

        static public bool GetDriverInfoByID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from Drivers where DriverID = @DriverID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    PersonID = (int)Reader["PersonID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];

                    IsFound = true;
                }

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { Connection.Close(); }

            return IsFound;
        }

        static public DataTable GetListOfDrivers()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "SELECT * FROM Drivers_View order by FullName";

                        

            SqlCommand command = new SqlCommand(query, Connection);
           

            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;

        }

    }

    public class ClsLicensesDB
    {
        static public int AddNewLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate,
                                String Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID )
        {
            int LicenseID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Insert Into Licenses " +
                           "values (@ApplicationID , @DriverID , @LicenseClassID , @IssueDate , @ExpirationDate , @Notes , @PaidFees , @IsActive , @IssueReason , @CreatedByUserID ) " +
                           " SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if (Notes != "")
            {
                Command.Parameters.AddWithValue("@Notes", Notes);
            }
            else Command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return LicenseID; }
            finally { Connection.Close(); }

            return LicenseID;

        }

        static public bool GetLicenseByID(int LicenseID ,ref int ApplicationID,ref int DriverID,ref int LicenseClassID,ref DateTime IssueDate,ref DateTime ExpirationDate,
                                ref String Notes,ref decimal PaidFees,ref bool IsActive,ref byte IssueReason,ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from Licenses " +
                           "where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    Notes = (Reader["Notes"] == DBNull.Value) ? "" : (string)Reader["Notes"];
                    PaidFees = (decimal)Reader["PaidFees"];
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

                    IsFound = true;

                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound;
            }
            finally { connection.Close(); }
            return IsFound;
        }

        static public bool GetLicenseByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID, ref int LicenseClassID, ref DateTime IssueDate, ref DateTime ExpirationDate,
                               ref String Notes, ref decimal PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from Licenses " +
                           "where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    LicenseID = (int)Reader["LicenseID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    Notes = (Reader["Notes"] == DBNull.Value) ? "" : (string)Reader["Notes"];
                    PaidFees = (decimal)Reader["PaidFees"];
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

                    IsFound = true;

                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }
            finally { connection.Close(); }
            return IsFound;
        }
        static public bool UpdateLicense(int LicenseID ,int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate,
                                String Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int RowAffected = 0;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Update Licenses
                            SET ApplicationID = @ApplicationID, 
                            DriverID = @DriverID ,
                            LicenseClassID = @LicenseClassID ,
                            IssueDate = @IssueDate ,
                            ExpirationDate = @ExpirationDate ,
                            Notes = @Notes ,
                            PaidFees = @PaidFees ,
                            IsActive = @IsActive ,
                            IssueReason = @IssueReason ,
                            CreatedByUserID = @CreatedByUserID 

                            Where LicenseID = @LicenseID";


            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return (RowAffected > 0);
        }


        static public int GetActiveLicenseIDByPersonID(int PersonID , int LicenseClassID)
        {
            int  LicenseID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Licenses.LicenseID 
                             from Licenses Inner join Drivers on Licenses.DriverID = Drivers.DriverID
                             Where Drivers.PersonID = @PersonID 
                             and Licenses.LicenseClassID = @LicenseClassID
                             and IsActive = 1 ;
                            ";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("PersonID", PersonID);
            Command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
                
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return LicenseID; 
            }
            finally { Connection.Close(); }

            return LicenseID;

        }

        static public DataTable GetLocalLicensesForADriver(int DriverID)    
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"  SELECT Licenses.LicenseID  , Licenses.ApplicationID , LicenseClasses.ClassName  ,
	                           Licenses.IssueDate , Licenses.ExpirationDate , Licenses.IsActive 
	                           From Licenses Inner join LicenseClasses on Licenses.LicenseClassID = LicenseClasses.LicenseClassID 
	                           WHERE Licenses.DriverID = @DriverID ; ";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {

                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;

        }

        static public bool CheckIsLicenseIDExist(int LicenseID)
        {
            bool IsExist = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select Found = 1 from Licenses where LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsExist = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }

            finally { Connection.Close(); }

            return IsExist;
        }

        static public bool CheckTheLicenseIsActive(int LicenseID)
        {
            bool IsActive = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select Found = 1 from Licenses where LicenseID = @LicenseID and IsActive = 1";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsActive = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; }

            finally { Connection.Close(); }

            return IsActive;
        }

        static public DateTime GetExpirationDateOfALicense(int LicenseID) 
        {
            DateTime ExpirationDate = DateTime.MinValue; 

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Select ExpirationDate from Licenses 
                            Where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null)
                {
                    ExpirationDate = (DateTime)result;
                }


            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return ExpirationDate; }
            finally { Connection.Close(); }

            return ExpirationDate;

        }

        static public bool CheckIfTheLicenseIsAClass3_OrdinaryDrivingLicense(int LicenseID)
        {
            bool IsaClass3 = false;
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select Found = 1 from Licenses where LicenseID = @LicenseID and LicenseClassID = 3";
            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsaClass3 = reader.HasRows;
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally { Connection.Close(); }

            return IsaClass3;
        }

        public static bool DeactivateLicense(int LicenseID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE Licenses
                           SET 
                              IsActive = 0
                             
                         WHERE LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
   
}

    public class ClsInternationalLicenseDB
    {
        static public int AddNewInternationaLicense(int ApplicationID, int  DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"
                               Update InternationalLicenses 
                               set IsActive=0
                               where DriverID=@DriverID;

                             INSERT INTO InternationalLicenses
                               (
                                ApplicationID,
                                DriverID,
                                IssuedUsingLocalLicenseID,
                                IssueDate,
                                ExpirationDate,
                                IsActive,
                                CreatedByUserID)
                         VALUES
                               (@ApplicationID,
                                @DriverID,
                                @IssuedUsingLocalLicenseID,
                                @IssueDate,
                                @ExpirationDate,
                                @IsActive,
                                @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return InternationalLicenseID; }
            finally { Connection.Close(); }

            return InternationalLicenseID;

        }

        public static bool GetInternationalLicenseByID(int InternationalLicenseID ,ref int ApplicationID,ref int DriverID,ref int IssuedUsingLocalLicenseID,ref DateTime IssueDate,ref DateTime ExpirationDate,ref bool IsActive,ref int CreatedByUserID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "Select * from InternationalLicenses " +
                           "where InternationalLicenseID = @InternationalLicenseID";

            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    IssuedUsingLocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    IsActive = (bool)Reader["IsActive"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    
                    IsFound = true;

                }

                Reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsFound; 
            }

            finally { connection.Close(); }

            return IsFound;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID , int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int RowAffected = 0;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"Update InternationalLicenses
                            SET ApplicationID = @ApplicationID, 
                            DriverID = @DriverID ,
                            IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID ,
                            IssueDate = @IssueDate ,
                            ExpirationDate = @ExpirationDate ,
                            IsActive = @IsActive ,
                            CreatedByUserID = @CreatedByUserID 
                            Where InternationalLicenseID = @InternationalLicenseID";


            SqlCommand Command = new SqlCommand(query, connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

   

            try
            {
                connection.Open();
                RowAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return (RowAffected > 0);
        }


        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"
	                        SELECT    InternationalLicenseID, ApplicationID,
		                    IssuedUsingLocalLicenseID , IssueDate, 
                           ExpirationDate, IsActive
		                   from InternationalLicenses where DriverID=@DriverID
                           order by ExpirationDate desc ";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {

                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; 
            }
            finally { Connection.Close(); }

            return dt;

        }

        static public DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"
	                        select InternationalLicenses.InternationalLicenseID , InternationalLicenses.ApplicationID ,InternationalLicenses.DriverID ,InternationalLicenses.IssuedUsingLocalLicenseID , InternationalLicenses.IssueDate ,
	                        InternationalLicenses.ExpirationDate , InternationalLicenses.IsActive 
	                        from InternationalLicenses order by IsActive, ExpirationDate desc;";

            SqlCommand Command = new SqlCommand(query, Connection);

            

            try
            {

                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; }
            finally { Connection.Close(); }

            return dt;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                            order by ExpirationDate Desc;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return -1;
            }

            finally
            {
                connection.Close();
            }


            return InternationalLicenseID;
        }

    }

    public class ClsDetainLicenseDB
    {
        public static int AddNewDetainLicense(int LicenseID , DateTime DetainDate , Decimal FineFees , int CreatedByUserID )
        {
            int DetainLicenseID = -1;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"INSERT INTO DetainedLicenses (LicenseID , DetainDate , FineFees , CreatedByUserID , IsReleased)
                            VALUES (@LicenseID , @DetainDate , @FineFees , @CreatedByUserID , 0 )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);
            command.Parameters.AddWithValue("DetainDate", DetainDate);
            command.Parameters.AddWithValue("FineFees", FineFees);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            
            try
            {
                Connection.Open();
                object Result = command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    DetainLicenseID = insertedID;
                }

            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return DetainLicenseID; 
            }
            finally { Connection.Close(); }

            return DetainLicenseID;
        }

        public static bool UpdateDetainLicense(int DetainLicenseID ,int LicenseID, DateTime DetainDate, Decimal FineFees, int CreatedByUserID)
        {
            int RowAffeted = 0;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                             SET LicenseID = @LicenseID, 
                            DetainDate = @DetainDate ,
                            FineFees = @FineFees ,
                            CreatedByUserID = @CreatedByUserID 
                            
                            WHERE DetainID = @DetainLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LicenseID", LicenseID);
            command.Parameters.AddWithValue("DetainDate", DetainDate);
            command.Parameters.AddWithValue("FineFees", FineFees);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("DetainLicenseID", DetainLicenseID);
            

            try
            {
                connection.Open();
                RowAffeted = command.ExecuteNonQuery();

            }
            catch (System.Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return (RowAffeted > 0);
        }

        public static bool FindDetainLicenseByID(int DetainLicenseID , ref int LicenseID, ref DateTime DetainDate, ref Decimal FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleasedDate, ref int ReleasedByUserID , ref int ReleasedApplicationID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from DetainedLicenses where DetainLicense = @DetainLicense ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("DetainLicenseID", DetainLicenseID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;

                    LicenseID = (int)Reader["LicenseID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = (decimal)Reader["FineFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                    ReleasedDate = (DateTime)Reader["ReleasedDate"];
                    ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                    ReleasedApplicationID = (int)Reader["ReleasedApplicationID"];


                }
                Reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return IsFound;
        }

        public static bool IsLicenseDetained(int LicenseID) 
        {
            bool IsDetainedLicense = false;

            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select Found = 1 
                             From DetainedLicenses WHERE LicenseID = @LicenseID And IsReleased = 0 ;
                            ";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                IsDetainedLicense = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return IsDetainedLicense; 
            }
            finally { Connection.Close(); }

            return IsDetainedLicense;

        }

        public static bool FindDetainLicenseByLocalLicenseID(int LicenseID, ref int DetainLicenseID, ref DateTime DetainDate, ref Decimal FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleasedDate, ref int ReleasedByUserID, ref int ReleasedApplicationID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = "select * from DetainedLicenses where LicenseID = @LicenseID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;

                    DetainLicenseID = (int)Reader["DetainID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = (decimal)Reader["FineFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                    if (IsReleased)
                    {
                        ReleasedDate = (DateTime)Reader["ReleaseDate"];
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                        ReleasedApplicationID = (int)Reader["ReleasedApplicationID"];
                    }
                    else
                    {
                        ReleasedDate = DateTime.Now ;
                        ReleasedByUserID = -1;
                        ReleasedApplicationID = -1;
                    }



                }
                Reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false; 
            }
            finally { connection.Close(); }

            return IsFound;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"select DetainedLicenses.DetainID , DetainedLicenses.LicenseID , DetainedLicenses.DetainDate , DetainedLicenses.IsReleased , 
	                        DetainedLicenses.FineFees , DetainedLicenses.ReleaseDate , People.NationalNo , 
	                        People.FirstName + ' ' + People.SecondName + ' ' +ISNULL( People.ThirdName , '' )+ ' ' + People.LastName ,
	                        DetainedLicenses.ReleaseApplicationID 

	                        from DetainedLicenses inner join Licenses On DetainedLicenses.LicenseID = Licenses.LicenseID 
	                        inner join Applications on Applications.ApplicationID = Licenses.ApplicationID 
	                        inner join People on People.PersonID = Applications.ApplicantPersonID
                               ";

            SqlCommand command = new SqlCommand(query, Connection);


            try
            {
                Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();

            }
            catch (Exception ex) 
            {
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return null; }
            finally { Connection.Close(); }

            return dt;

        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionSetting.ConnectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleasedByUserID = @ReleasedByUserID ,
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsUtility.WirteExceptionInEventLog(ex.Message, EventLogEntryType.Error);

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        
    }

}
