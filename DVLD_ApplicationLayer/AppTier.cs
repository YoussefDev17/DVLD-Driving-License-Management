using DataBaseTier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationTier
{
    public class ClsPerson
    {
        enum enMode  {AddNewPerson = 0 , UpdatePerson = 1}
        private enMode Mode = enMode.AddNewPerson;
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public String ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName  { get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }
        public DateTime DateOfBirth { get; set; }
        public char Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public int NationalCountryID { get; set; }
        public string ImagePath { get; set; }

        public ClsCountry CountryInfo;
        private bool _AddNewPerson()
        {
            this.PersonID = ClsPersonDB.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName,
                                                 this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone,
                                                 this.Gmail, this.NationalCountryID, this.ImagePath);
            return ( this.PersonID != -1);
        }

        private bool _UpdatePersonInformation()
        {
            return ClsPersonDB.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                                           this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Gmail, this.NationalCountryID, this.ImagePath);
        }
        public ClsPerson() 
        {
            this.PersonID = 0;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = ' ';
            this.Address = "";
            this.Phone = "";
            this.Gmail = "";
            this.NationalCountryID = 0;
            this.ImagePath = "";

            Mode = enMode.AddNewPerson;
        
        }
        private ClsPerson( int personID, string nationalNo, string firstName, string secondName, string thirdName, string lastName, DateTime dateOfBirth, char gendor, string address, string phone, string gmail, int nationalCountryID, string imagePath)
        {
            
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gendor = gendor;
            Address = address;
            Phone = phone;
            Gmail = gmail;
            NationalCountryID = nationalCountryID;
            this.CountryInfo = ClsCountry.FindCountryByID(nationalCountryID);
            ImagePath = imagePath;

            Mode = enMode.UpdatePerson;
        }

        public bool Save()
        {
            switch (Mode) 
            {
                case enMode.AddNewPerson:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.UpdatePerson;
                        return true;
                    }
                    else { return false; }

                case enMode.UpdatePerson:
                    return _UpdatePersonInformation();

                default: return false;

            }
        }
        static public DataTable GetAllPeople()
        {
            return ClsPersonDB.GetAllPeople();
        }

        static public DataTable GetAllCountriesName()
        {
            return ClsCountryDB.GetAllCountriesName();
        }

        static public bool IsNationaNoExist(string NationaNo)
        {
            return ClsPersonDB.IsNationaNoExist(NationaNo);
        }
        static public bool IsPersonIDExist(int PersonId)
        {
            return ClsPersonDB.IsPersonIDExist(PersonId);
        }

        static public ClsPerson FindPersonByPersonID(int PersonID)
        {
            string NationalNo = "";
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            char Gendor = ' ';
            string Address = "";
            string Phone = "";
            string Email = "";
            int NationalCountryID = 0;
            string ImagePath = "";

            if (ClsPersonDB.FindPersonByID(PersonID ,ref NationalNo ,ref FirstName ,ref SecondName ,ref ThirdName ,ref LastName,
                                         ref DateOfBirth ,ref Gendor ,ref Address ,ref Phone ,ref Email ,ref NationalCountryID ,ref ImagePath ))
            {
                    return new ClsPerson(PersonID , NationalNo , FirstName , SecondName , ThirdName , LastName , DateOfBirth , Gendor , Address ,Phone ,Email ,NationalCountryID ,ImagePath);     
            }

            return null;
        }

        static public ClsPerson FindPersonByNationalNo(string NationalNo)
        {
            int PersonID = 0;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            char Gendor = ' ';
            string Address = "";
            string Phone = "";
            string Email = "";
            int NationalCountryID = 0;
            string ImagePath = "";

            if (ClsPersonDB.FindPersonByNationalNo(ref PersonID,  NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                                         ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalCountryID, ref ImagePath))
            {
                return new ClsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalCountryID, ImagePath);
            }

            return null;
        }

        static public bool DeletePersonByPersonID( int PersonID ) 
        {
            return ClsPersonDB.DeletePersonByPersonID(PersonID);
        }

        static public int GetPersonIDByNationalNo(string NationalNo)
        {
            return ClsPersonDB.GetPersonIDByNationalNo(NationalNo);
        }

    }

    public class ClsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        private ClsCountry(int CountryID , string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;

        }
        static public ClsCountry FindCountryByID(int CountryID) 
        {
            string CountryName = "";

            if (ClsCountryDB.FindCountryByID(CountryID, ref CountryName))
            {
                return new ClsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        static public ClsCountry FindCountryByName(string CountryName)
        {
            int CountryID = -1;

            if (ClsCountryDB.FindCountryByName(CountryName, ref CountryID))
            {
                return new ClsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
    }

    public class ClsUser
    {
        enum enMode {AddNewUser = 0, UpdateUser = 1}

        private enMode Mode = enMode.AddNewUser;

        private ClsUser(int UserID , int PersonID , string UserName , string Password , bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            PersonInfo = ClsPerson.FindPersonByPersonID(PersonID);

            Mode = enMode.UpdateUser;
        }
        private bool _AddNewUser()
        {
            this.UserID = ClsUserDB.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);
            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return ClsUserDB.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ClsPerson PersonInfo;

        public ClsUser()
        {
            this.UserID = 0;
            this.PersonID = 0;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            this.Mode = enMode.AddNewUser;
        }

        public bool Save()
        {
            switch (Mode) 
            {            
              case enMode.AddNewUser:
                    if (_AddNewUser())
                    {
                        Mode = enMode.UpdateUser;
                        return true;
                    }
                    else
                    {
                        return false; 
                    }

                case enMode.UpdateUser:

                    return _UpdateUser();

                default: return false;
            }
        }

        static public ClsUser FindUserByUserID(int UserID)
        {
            int PersonID = 0;
            string UserName = "";
            string Password = "";
            bool IsActive = false;

            if (ClsUserDB.GetUserInfoByUserID(UserID ,ref PersonID ,ref UserName ,ref Password ,ref IsActive))
            {
                return new ClsUser(UserID , PersonID , UserName , Password , IsActive);
            }

            return null;
        }
        static public ClsUser FindUserByUserNameAndPassword(string UserName , string Password)
        {
            int UserID = 0;
            int PersonID = 0;
            bool IsActive = false;

            if (ClsUserDB.GetUserInfoByUsernameAndPassword(UserName , Password ,ref UserID , ref PersonID , ref IsActive ))
            {
                return new ClsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            return null;
        }

        static public bool IsUserNameAndPasswordExist(string UserName , string Password)
        {
            return ClsUserDB.IsUserNameAndPasswordExist (UserName , Password);
        }

        static public bool IsUserNameActive(string UserName)
        {
            return ClsUserDB.IsUserNameActive(UserName);
        }

        static public DataTable GetAllUsers() 
        { 
           return ClsUserDB.GetAllUsers();
        }

        static public bool IsUserExistForPersonID(int PersonID) 
        {
           return ClsUserDB.IsUserExistForPersonID (PersonID);
        }

        static public bool DeleteUserByUserID(int UserID)
        {
            return ClsUserDB.DeleteUserByUserID (UserID);
        }

        static public bool IsUserExist(string UserName)
        {
            return ClsUserDB.IsUserExist(UserName);
        }

    }

    public class ClsApplicationTypes
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }

        private ClsApplicationTypes(int ApplicationID , string ApplicationTypeTitle , decimal ApplicationFees) 
        {
           this.ApplicationTypeID = ApplicationID;
           this.ApplicationTypeTitle = ApplicationTypeTitle;  
           this.ApplicationFees = ApplicationFees;
        }
        static public DataTable GetAllApplicationType() 
        {
           return ClsApplicationTypesDB.GetAllApplicationTypes();
        }

        public bool UpdateApplicationType()
        {
            return ClsApplicationTypesDB.UpdateApplicationType(this.ApplicationTypeID , this.ApplicationTypeTitle , this.ApplicationFees);
        }
        static public ClsApplicationTypes FindApplicationTypeById(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            decimal ApplicationTypeFees = 0;

            if (ClsApplicationTypesDB.FindApplicationTypeByID(ApplicationTypeID ,ref ApplicationTypeTitle ,ref ApplicationTypeFees))
            {
                return new ClsApplicationTypes(ApplicationTypeID , ApplicationTypeTitle , ApplicationTypeFees);
            }

            return null;
        }

        static public int GetApplicationFees(int ApplicationTypeID)
        {
            return ClsApplicationTypesDB.GetApplicationFeesUsingApplicationTypeID(ApplicationTypeID);
        }
    }

    public class ClsTestTypes
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        private ClsTestTypes(ClsTestTypes.enTestType TestTypeID , string TestTypeTitle, string TestTypeDescription , double TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeFees = TestTypeFees;
            this.TestTypeDescription = TestTypeDescription;
        }

        public ClsTestTypes.enTestType TestTypeID { get; set; }
        public string TestTypeTitle {  get; set; }
        public string TestTypeDescription { get; set; }
        public double TestTypeFees {  get; set; }

        static public DataTable GetAllTestTypes() 
        {
            return ClsTestTypesDB.GetAllTestTypes();
        }

        static public ClsTestTypes FindTestTypeByID(ClsTestTypes.enTestType TestType)
        {
            string TestTypeTitle = "";
            string TestTypeDescription = "";
            double TestTypeFees = 0;
            if (ClsTestTypesDB.FindTestTypeByID((int)TestType , ref TestTypeTitle , ref TestTypeDescription , ref TestTypeFees))
            {
                return new ClsTestTypes(TestType , TestTypeTitle , TestTypeDescription , TestTypeFees);
            }

            return null;
        }

        public bool UpdateTestType()
        {
            return ClsTestTypesDB.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }

       

    }

    public class ClsLicenseClasse
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float ClassFees { set; get; }

        public ClsLicenseClasse()

        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;

            Mode = enMode.AddNew;

        }

        public ClsLicenseClasse(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)

        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
            Mode = enMode.Update;
        }

        private bool _AddNewLicenseClass()
        {
            //call DataAccess Layer 

            this.LicenseClassID = ClsLicenseClasseDB.AddNewLicenseClass(this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);


            return (this.LicenseClassID != -1);
        }

        private bool _UpdateLicenseClass()
        {
            //call DataAccess Layer 

            return ClsLicenseClasseDB.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }

        public static ClsLicenseClasse Find(int LicenseClassID)
        {
            string ClassName = ""; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (ClsLicenseClasseDB.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new ClsLicenseClasse(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;

        }

        public static ClsLicenseClasse Find(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (ClsLicenseClasseDB.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new ClsLicenseClasse(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;
        }

        static public DataTable GetAllLicenseClassesName()
        {
            return ClsLicenseClasseDB.GetAllLicenseClasses();
        }

        
    }

    public class ClsApplication
    {
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 8
        };

        public enum enApplicationStatus {NewApplication = 1 , CanceledApplication = 2 , CompletedApplication = 3 }

        public enum enMode {AddNewApplication = 1 , UpdateApplication = 2 }

        public enMode Mode = enMode.AddNewApplication;

        private bool _AddNewApplication()
        {
            this.ApplicationID = ClsApplicationDB.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,(byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }

        private bool _UpdateAplication()
        {
            return ClsApplicationDB.UpdateApplication(this.ApplicationID , this.ApplicantPersonID , this.ApplicationDate,this.ApplicationTypeID,(byte)this.ApplicationStatus , this.LastStatusDate , this.PaidFees , this.CreatedByUserID);
        }

        public int ApplicationID { get; set; }

        public int ApplicantPersonID {  get; set; }

        public ClsPerson PersonInfo { get; set; }

        public string ApplicantFullName
        {
            get
            {
                return ClsPerson.FindPersonByPersonID(ApplicantPersonID).FullName;
            }
        }

        public DateTime ApplicationDate {  get; set; }

        public int ApplicationTypeID { get; set; }

        public ClsApplicationTypes ApplicationTypeInfo;

        public enApplicationStatus ApplicationStatus { get; set; }

        public string ApplicationStatuText
        {
            get 
            {
                switch(ApplicationStatus)
                {
                    case enApplicationStatus.NewApplication:
                        return "New";

                    case enApplicationStatus.CanceledApplication:
                        return "Cancel";

                    case enApplicationStatus.CompletedApplication:

                        return "Completed";

                    default:
                        return "Unknown";
                }
            
            }
        }

        public DateTime LastStatusDate { get; set; }

        public float PaidFees { get; set; }

        public int CreatedByUserID { get; set; }

        public ClsUser CreatedUserInfo;

        public ClsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.NewApplication;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNewApplication;

        }

        private ClsApplication( int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID, enApplicationStatus applicationStatus, DateTime lastStatusDate, float paidFees, int creatByUserID)
        {
            
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.PersonInfo = ClsPerson.FindPersonByPersonID(applicantPersonID);
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeInfo = ClsApplicationTypes.FindApplicationTypeById(applicationTypeID);
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = creatByUserID;
            this.ApplicationTypeInfo = ClsApplicationTypes.FindApplicationTypeById(applicationTypeID);
            this.CreatedUserInfo = ClsUser.FindUserByUserID(creatByUserID);
            

            Mode = enMode.UpdateApplication;
        }

        static public ClsApplication FindApplicationByID(int ApplicationID)
        {
            int ApplicationPersonID = 0;
            DateTime applicationDate = DateTime.Now;
            int applicationTypeID = 0;
            byte applicationStatus = 0;
            DateTime lastStatusDate = DateTime.Now;
            float paidFees = 0;
            int creatByUserID = -1;

            if (ClsApplicationDB.FindApplicationByID(ApplicationID , ref ApplicationPersonID , ref applicationDate , ref applicationTypeID , ref applicationStatus , ref lastStatusDate , ref paidFees , ref creatByUserID))
            {
                return new ClsApplication(ApplicationID , ApplicationPersonID , applicationDate , applicationTypeID ,(enApplicationStatus) applicationStatus , lastStatusDate , paidFees , creatByUserID);
            }
            return null;
        }
        public bool Save()
        {
            switch (Mode) 
            {
                case enMode.AddNewApplication:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.UpdateApplication;
                        return true;
                    }
                    else return false;
                case enMode.UpdateApplication:
                    return _UpdateAplication();

                default : return false;

            }
        }

        
        static public bool DeleteApplicationByID(int ApplicationID)
        {
            return ClsApplicationDB.DeleteApplicationByID(ApplicationID);
        }

        static public int GetActiveApplicationIDForLicenseClass(int PersonID, ClsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return ClsApplicationDB.GetActiveApplicationIDForLicenseClass(PersonID,(int)ApplicationTypeID, LicenseClassID);
        }

        public bool Delete()
        {
            return ClsApplication.DeleteApplicationByID(this.ApplicationID);
        }

        public bool Cancel()
        {
            return ClsApplicationDB.UpdateStatus(this.ApplicationID, 2);
        }

        public bool SetComplet()
        {
            return ClsApplicationDB.UpdateStatus(this.ApplicationID, 3);
        }
    }

    public class ClsLocalDrivingLicenseApplication : ClsApplication
    {
        public enum enMode {AddNewMode = 1 , UpdateMode = 2 }
        public enMode Mode = enMode.AddNewMode;

        public int LocalDrivingLicenseApplicationID { get; set; }

        public int LicenseClassID { get; set;}

        public ClsLicenseClasse LicenseClassInfo;

        public string PersonFullName
        {
            get
            {
                return base.PersonInfo.FullName;
            }
        }

        public ClsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.AddNewMode;
        }
        private ClsLocalDrivingLicenseApplication(int ApplicationID , int ApplicantPersonID , DateTime ApplicationDate ,int ApplicationTypeID , enApplicationStatus  ApplicationStatus , DateTime LastStausDate , float PaidFees , int CreatedByUserID  ,  int LocalDrivingLicenseApplicationID  , int LicensesClassID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.PersonInfo = ClsPerson.FindPersonByPersonID(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = ClsApplicationTypes.FindApplicationTypeById(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStausDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedUserInfo = ClsUser.FindUserByUserID(CreatedByUserID);

            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID ;
            this.LicenseClassID = LicensesClassID ;
            this.LicenseClassInfo = ClsLicenseClasse.Find(LicenseClassID);
            Mode = enMode.UpdateMode;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {

            this.LocalDrivingLicenseApplicationID = ClsLocalDrivingLicenseApplicationDB.AddNewLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return ClsLocalDrivingLicenseApplicationDB.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }

        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.

            base.Mode =(ClsApplication.enMode) Mode;

            if (!base.Save())
            {
                return false;
            }

            //After we save the main application now we save the sub application.
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else return false;

                case enMode.UpdateMode:

                    return _UpdateLocalDrivingLicenseApplication();

                default : return false;

            }
        }

        static public bool CheckIsPersonIDAlreadyHaveLicenseClass(int PersonID , int LicenseClassID)
        {
            return ClsLocalDrivingLicenseApplicationDB.CheckIsPersonApplicationAlreadyHaveLicenseClass(PersonID , LicenseClassID);
        }

        static public DataTable GetAllLocalDrivingLicensesApplications()
        {
            return ClsLocalDrivingLicenseApplicationDB.GetAllLocalDrivingLicensesApplications();
        }
        
        static public ClsLocalDrivingLicenseApplication FindLocalDrivingLicenseApplicantionByID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1;
            int LicensesClassID = -1;

            bool IsFound = ClsLocalDrivingLicenseApplicationDB.FindLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicensesClassID);

            if (IsFound)
            {
                //Find Application Info (Base Class)
                ClsApplication Application = ClsApplication.FindApplicationByID(ApplicationID);

                return new ClsLocalDrivingLicenseApplication(Application.ApplicationID ,Application.ApplicantPersonID , Application.ApplicationDate , Application.ApplicationTypeID , Application.ApplicationStatus , Application.LastStatusDate , Application.PaidFees , Application.CreatedByUserID , LocalDrivingLicenseApplicationID, LicensesClassID);


            }

            return null;
        }

        

        static public bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return ClsLocalDrivingLicenseApplicationDB.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        public bool Delete()
        {
            bool IsLocalDrivingLicenseApplicationIDdeleted = ClsLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);

            if (!IsLocalDrivingLicenseApplicationIDdeleted)
            {
                return false;
            }

            //Delete The Base Application ID
            bool IsApplicationIDdeleted = base.Delete();
            
            return IsApplicationIDdeleted;
        }

        public  bool DoesPassTestType(ClsTestTypes.enTestType TestTypeID )
        {
            return ClsLocalDrivingLicenseApplicationDB.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public int GetActiveLicenseID()
        {//this will get the license id that belongs to this application
            return ClsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }

        public bool IsLicenseIsuued()
        {
            return (GetActiveLicenseID() != -1);
        }

        public byte GetPassedTest()
        {
            return ClsTest.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }

        public bool IsPassedAllTest()
        {
            return GetPassedTest() == 3;
        }

        public  bool ThereIsAnActiveTestAppointement(ClsTestTypes.enTestType TestType)
        {
            return ClsLocalDrivingLicenseApplicationDB.ThereIsAnActiveTestAppointement(this.LocalDrivingLicenseApplicationID, (int)TestType);
        }

        public ClsTest GetLastTestperTestType(ClsTestTypes.enTestType TestType)
        {
            return ClsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestType);
        }

        public bool Cancel()
        {
            return base.Cancel();
        }

        public byte GetTotalTrialsPerTest(ClsTestTypes.enTestType TestType)
        {
            return ClsLocalDrivingLicenseApplicationDB.TotalTrialPerTest(this.LocalDrivingLicenseApplicationID , (int)TestType);
        }

        public  bool DoesAttendTestType(ClsTestTypes.enTestType TestType)
        {
            return ClsLocalDrivingLicenseApplicationDB.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestType);
                       
        }

        
        public int IssueLicenseForTheFirstTime(string Notes , int CreatedByUserID)
        {

            ClsDriver Driver = ClsDriver.FindDriverByPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                Driver = new ClsDriver();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                Driver.CreatedDate = DateTime.Now;
                if (!Driver.Save())
                {
                    return -1;
                }
            }

            ClsLicense License = new ClsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = Driver.DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = (decimal)this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = ClsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                this.SetComplet();
                return License.LicenseID;
            }
            else
            {
                return -1; 
            }
        }
        
    }

    public class ClsTestAppointment
    {
        private enum enMode {AddNewTestAppointment = 0 , UpdateTestAppointment = 1 };
        private enMode _Mode = enMode.AddNewTestAppointment;

        private bool _AddNewTestAppointment()
        {
            this.TestAppointementID = ClsTestAppointmentsDB.AddNewTestAppointment(this.TestTypeID , this.LocalDrivingLicenseApplicationID , this.AppointmentDate , this.PaidFees , this.CreatedByUserID , this.IsLocked , this.RetakeTestApplicationID);

            return (this.TestAppointementID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            return ClsTestAppointmentsDB.UpdateTestAppointment(this.TestAppointementID ,this.TestTypeID ,this.LocalDrivingLicenseApplicationID ,this.AppointmentDate , this.PaidFees ,this.CreatedByUserID , this.IsLocked , this.RetakeTestApplicationID);
        }

        private ClsTestAppointment(int testAppointementID, int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, decimal paidFees, int createdByUserID, bool isLocked , int RetakeTestApplicationID)
        {
            
            this.TestAppointementID = testAppointementID;
            this.TestTypeID = testTypeID;
            this.LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.IsLocked = isLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;

            _Mode = enMode.UpdateTestAppointment;
        }


        public int TestAppointementID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public ClsApplication RetakeTestAppInfo { get; set; }

        public int TestID
        {
            get
            {
                return GetTestID();
            }
        }

        public ClsTestAppointment()
        {
            this.TestAppointementID = -1;
            this.TestTypeID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;

            _Mode = enMode.AddNewTestAppointment;
        }

        static public ClsTestAppointment FindTestAppointmentByID(int TestAppointementID)
        {
            int TestTypeID = 0;
            int LocalDrivingLicenseApplicationID = 0;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            int CreatedByUserID = 0;
            bool IsLocked = false;
            int RetakeTestApplicationID = -1;

            if (ClsTestAppointmentsDB.FindTestAppointmentByID(TestAppointementID ,ref TestTypeID ,ref LocalDrivingLicenseApplicationID ,ref AppointmentDate ,ref PaidFees ,ref CreatedByUserID ,ref IsLocked , ref RetakeTestApplicationID))
            {
                return new ClsTestAppointment(TestAppointementID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked , RetakeTestApplicationID);
            }
            return null;
        }
        public bool Save()
        {
            switch (_Mode) 
            {
                case enMode.AddNewTestAppointment:
                    if (_AddNewTestAppointment())
                    {
                        _Mode = enMode.UpdateTestAppointment;
                        return true;
                    }
                    else return false;

                case enMode.UpdateTestAppointment:

                    return _UpdateTestAppointment();

                default: return false;

            }
        }

        static public DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID , int TestTypeID)
        {
            return ClsTestAppointmentsDB.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        static public bool CheckPersonWithLocalDrivingLicenseApplicationIDHasTestAppointment(int LocalDrivingLicenseApplicationID)
        {
            return ClsTestAppointmentsDB.CheckPersonWithLocalDrivingLicenseApplicationIDHasTestAppointment (LocalDrivingLicenseApplicationID);
        }
        static public bool CheckPersonWithLocalDrivingLicenseApplicationIDIsLockedTheTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return ClsTestAppointmentsDB.CheckPersonWithLocalDrivingLicenseApplicationIDIsLockedTheTest(LocalDrivingLicenseApplicationID , TestTypeID);
        }

        public int GetTestID()
        {
            return ClsTestAppointmentsDB.GetTestID(this.TestAppointementID);
        }
        
    }

    public class ClsTest
    {
        private enum enMode {AddNewTest = 1 , UpdateTest = 2 }
        private enMode _Mode = enMode.AddNewTest;

        private bool _AddNewTest()
        {
            this.TestID = ClsTestDB.AddNewTestResult(this.TestAppointmentID , this.TestResult , this.Notes , this.CreatedByUserID);

            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return ClsTestDB.UpdateTest(this.TestID , this.TestAppointmentID , this.TestResult , this.Notes , this.CreatedByUserID);
        }

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public ClsTestAppointment TestAppointmentInfo;

        public ClsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;

            _Mode = enMode.AddNewTest;

        }

        public ClsTest(int TestID, int TestAppointmentID,
            bool TestResult, string Notes, int CreatedByUserID)

        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = ClsTestAppointment.FindTestAppointmentByID(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            _Mode = enMode.UpdateTest;
        }

        static public ClsTest FindTestByID(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (ClsTestDB.GetTestInfoByID(TestID , ref TestAppointmentID , ref TestResult , ref Notes , ref CreatedByUserID))
            {
                return new ClsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }

            return null;
        }

        static public ClsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, ClsTestTypes.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (ClsTestDB.GetLastTestByPersonAndTestTypeAndLicenseClass
                (PersonID, LicenseClassID, (int)TestTypeID, ref TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))

                return new ClsTest(TestID,
                        TestAppointmentID, TestResult,
                        Notes, CreatedByUserID);
            else
                return null;
        }
        public bool Save()
        {
            switch (this._Mode) 
            {
                case enMode.AddNewTest:

                    if (_AddNewTest())
                    {
                        _Mode = enMode.UpdateTest;
                        return true;

                    }
                    else return false;

                case enMode.UpdateTest:

                    return _UpdateTest();

                default : return false;


            }
        }

        //static public bool CheckThePersonHasFailedTheTest(int TestAppointmentID)
        //{
        //    return ClsTestDB.CheckThePersonHasFailedTheTest(TestAppointmentID);
        //}

        static public byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return ClsTestDB.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        static public bool IsPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }

    public class ClsDriver
    {
        enum enMode {AddNewDriverMode = 1 , UpdateDriverMode = 2 }
        enMode _Mode = enMode.AddNewDriverMode;

        private bool _AddNewDriver()
        {
            this.DriverID = ClsDriversDB.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return (this.DriverID != -1);
        }

        private ClsDriver(int DriverID , int PersonID , int CreatedByUserID , DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.PersonInfo = ClsPerson.FindPersonByPersonID(PersonID);
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;

            _Mode = enMode.UpdateDriverMode;
        }

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public ClsPerson PersonInfo;

        public ClsDriver()
        {
            this.PersonID = 0;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now ;

            _Mode = enMode.AddNewDriverMode;
           
        }
         
        public bool Save()
        {
            switch (_Mode) 
            {
                case enMode.AddNewDriverMode:

                    if (_AddNewDriver())
                    {
                        _Mode = enMode.UpdateDriverMode;
                        return true;
                    }
                    else return false;

                default : return false;

            }
        }

        static public bool CheckThePersonIsADriver(int PersonID)
        {
            return ClsDriversDB.CheckThePersonIsADriver(PersonID);
        }

        static public ClsDriver FindDriverByPersonID(int PersonID) 
        {
            int DriverID = 0;
            int CreatedByUserID = 0;
            DateTime CreatedDate = DateTime.Now;

            if (ClsDriversDB.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))
            {
                return new ClsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }

            return null;

        }

        static public ClsDriver FindDriverByID(int DriverID)
        {
            int PersonID = 0;
            int CreatedByUserID = 0;
            DateTime CreatedDate = DateTime.Now;

            if (ClsDriversDB.GetDriverInfoByID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new ClsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }

            return null;

        }

        static public DataTable GetListOfDrivers()
        {
            return ClsDriversDB.GetListOfDrivers();
        }

        static public DataTable GetLocalLicenses(int DriverID)
        {
            return ClsLicense.GetLocalLicensesForADriver(DriverID);
        }

        static public DataTable GetInternationalLicenses(int DriverID)
        {
            return ClsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }
    }

    public class ClsLicense
    {
        enum enMode { AddNewLicenseMode, UpdateLicenseMode }
        enMode _Mode = enMode.AddNewLicenseMode;
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        private bool _AddNewLicense()
        {
            this.LicenseID = ClsLicensesDB.AddNewLicense(this.ApplicationID , this.DriverID , this.LicenseClassID , this.IssueDate , 
                             this.ExpirationDate ,this.Notes , this.PaidFees , this.IsActive ,(byte)this.IssueReason , this.CreatedByUserID );
            return (this.LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            return ClsLicensesDB.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate,
                             this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive,(byte)this.IssueReason, this.CreatedByUserID);
        }

        private ClsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate,
                                String Notes, decimal PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.DriverInfo = ClsDriver.FindDriverByID(DriverID);
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = ClsLicenseClasse.Find(LicenseClassID);
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            if (IsDetained)
            {
                this.DetainedLicenseInfo = ClsDetainLicense.FindDetainLicenseByLocalLicenseID(this.LicenseID);
            }
            _Mode = enMode.UpdateLicenseMode;

        }
 
        
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }

        public ClsDriver DriverInfo;
        public int LicenseClassID { get; set; }

        public ClsLicenseClasse LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }

        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public int CreatedByUserID { get; set; }
        
        public bool IsDetained
        {
            get
            {
                return ClsDetainLicense.IsLicenseDetained(this.LicenseID);
            }
        }

        public ClsDetainLicense DetainedLicenseInfo { get; set; }
        public ClsLicense() 
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = false;
            this.IssueReason = 0;
            this.CreatedByUserID = -1;

            _Mode = enMode.AddNewLicenseMode;

        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNewLicenseMode:

                    if (_AddNewLicense())
                    {
                        _Mode = enMode.UpdateLicenseMode;
                        return true;

                    }
                    else return false;

                case enMode.UpdateLicenseMode:

                    return _UpdateLicense();

                default: return false;
            }
        }

        static public ClsLicense FindLicenseByID(int LicenseID)
        {
          
            int ApplicationID = 0;
            int DriverID = 0;
            int LicenseClassID = 0;
            DateTime IssueDate = DateTime.Now ;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = 0;

            if (ClsLicensesDB.GetLicenseByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate,
                                         ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new ClsLicense(LicenseID, ApplicationID,DriverID ,LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive,(enIssueReason) IssueReason, CreatedByUserID);
            }

            return null;
        }

        static public ClsLicense FindLicenseByApplicationID(int ApplicationID)
        {
            int LicenseID = 0;
            int DriverID = 0;
            int LicenseClassID = 0;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = 0;

            if (ClsLicensesDB.GetLicenseByApplicationID(ApplicationID,  ref LicenseID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate,
                                         ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new ClsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive,(enIssueReason) IssueReason, CreatedByUserID);
            }

            return null;
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }
        static public int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return ClsLicensesDB.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }

        static public DataTable GetLocalLicensesForADriver(int DriverID)
        {
            return ClsLicensesDB.GetLocalLicensesForADriver(DriverID);
        }

        static public bool CheckIsLicenseIDExist(int LicenseID)
        {
            return ClsLicensesDB.CheckIsLicenseIDExist(LicenseID);
        }

        static public bool CheckTheLicenseIsActive(int LicenseID)
        {
            return ClsLicensesDB.CheckTheLicenseIsActive(LicenseID);
        }

        static public DateTime GetExpirationDateOfALicense(int LicenseID)
        {
            return ClsLicensesDB.GetExpirationDateOfALicense(LicenseID);
        }

        static public bool CheckIfTheLicenseIsAClass3_OrdinaryDrivingLicense(int LicenseID)
        {
            return ClsLicensesDB.CheckIfTheLicenseIsAClass3_OrdinaryDrivingLicense(LicenseID);
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
          
        }

        public bool DeactivateLicense()
        {
            return ClsLicensesDB.DeactivateLicense(this.LicenseID);
        }

        public ClsLicense RenewLicense(int CreatedByUserID , string Notes)
        {
            ClsApplication ApplicationForRenewal = new ClsApplication();

            ApplicationForRenewal.ApplicantPersonID = this.DriverInfo.PersonID;
            ApplicationForRenewal.ApplicationDate = DateTime.Now;
            ApplicationForRenewal.ApplicationTypeID = (int)ClsApplication.enApplicationType.RenewDrivingLicense;
            ApplicationForRenewal.ApplicationStatus = ClsApplication.enApplicationStatus.CompletedApplication;
            ApplicationForRenewal.LastStatusDate = DateTime.Now;
            ApplicationForRenewal.PaidFees =(float)ClsApplicationTypes.FindApplicationTypeById((int)ClsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            ApplicationForRenewal.CreatedByUserID = CreatedByUserID;

            if (!ApplicationForRenewal.Save())
            {
                return null;
            }

            ClsLicense NewLicense = new ClsLicense();

            NewLicense.ApplicationID = ApplicationForRenewal.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            //summing Total Fees
            NewLicense.PaidFees =(decimal) (this.LicenseClassInfo.ClassFees + ApplicationForRenewal.PaidFees);
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {
                return null;
            }

            // we deactivated Old License
            if (!DeactivateLicense())
            {
                return null;
            }

            return NewLicense;
        }

        public ClsLicense Replace(enIssueReason IssueReason , int CreatedByUserID)
        {
            ClsApplication Application = new ClsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?(int) ClsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
            (int)ClsApplication.enApplicationType.ReplaceLostDrivingLicense;
            Application.ApplicationStatus = ClsApplication.enApplicationStatus.CompletedApplication;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees =(float)ClsApplicationTypes.FindApplicationTypeById(Application.ApplicationTypeID).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            ClsLicense NewLicense = new ClsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = this.PaidFees +(decimal)Application.PaidFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {
                return null;
            }

            DeactivateLicense();

            return NewLicense;
        }

        public int Detain(decimal FineFees , int CreatedByUserID)
        {
            ClsDetainLicense DetainLicense = new ClsDetainLicense();

            DetainLicense.LicenseID = this.LicenseID;
            DetainLicense.DetainDate = DateTime.Now;
            DetainLicense.FineFees = FineFees;
            DetainLicense.CreatedByUserID = CreatedByUserID;

            if (DetainLicense.Save())
            {
                return DetainLicense.DetainLicenseID;
            }
            else
            {
                return -1;
            }
        }

        public bool Release(int ReleasedByUserID ,ref int ApplicationID)
        {
            ClsApplication Application = new ClsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)ClsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatus = ClsApplication.enApplicationStatus.CompletedApplication;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees =(float) ClsApplicationTypes.FindApplicationTypeById((int)ClsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;
            return this.DetainedLicenseInfo.ReleaseDetainedLicense(ReleasedByUserID, ApplicationID);
        }
    }

    public class ClsInternationalLicense : ClsApplication
    {
        private enum enMode {enAddNewMode = 1 , enUpdateMode = 2 }
        private enMode _Mode = enMode.enAddNewMode;

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = ClsInternationalLicenseDB.AddNewInternationaLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssuedDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            return ClsInternationalLicenseDB.UpdateInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssuedDate, ExpirationDate, IsActive, CreatedByUserID);
        }
        private ClsInternationalLicense( int ApplicantPersonID ,DateTime ApplicationDate,ClsApplication.enApplicationStatus ApplicationStatus , DateTime LastStatusDate , float PaidFees, int InternationalLicenseID  , int ApplicationID , int DriverID , int IssuedUsingLocalLicenseID , DateTime IssuedDate , DateTime ExpirationDate , bool IsActive , int CreatedByUserID)
        {
            //this is for the base clase
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)ClsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.DriverInfo = ClsDriver.FindDriverByID(DriverID);
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssuedDate = IssuedDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            this._Mode = enMode.enUpdateMode;
        }
        public int InternationalLicenseID { get; set; }

        //public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public ClsDriver DriverInfo { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }


        public ClsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssuedDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MinValue;
            this.IsActive = false;
            this.CreatedByUserID = -1;

            _Mode = enMode.enAddNewMode;
        }

        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode =(ClsApplication.enMode) _Mode;

            if (!base.Save())
            {
                return false;
            }

            switch (this._Mode)
            {
                case enMode.enAddNewMode:
                    if (_AddNewInternationalLicense())
                    {
                        this._Mode = enMode.enUpdateMode;
                        return true;
                    }
                    else return false;

                case enMode.enUpdateMode:

                    return _UpdateInternationalLicense();

                default : return false;
            }
        }

        static public ClsInternationalLicense FindInternationalLicenseByID(int InternationalLicenseID)
        {
            
            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssuedDate = DateTime.MinValue;
            DateTime ExpirationDateTime = DateTime.MinValue;
            bool IsActive = false;
            int CreatedByUserID = -1;

            if (ClsInternationalLicenseDB.GetInternationalLicenseByID(InternationalLicenseID , ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID, ref IssuedDate, ref ExpirationDateTime, ref IsActive , ref CreatedByUserID))
            {
                ClsApplication Application = ClsApplication.FindApplicationByID(ApplicationID);

                return new ClsInternationalLicense(Application.ApplicantPersonID , Application.ApplicationDate , Application.ApplicationStatus,Application.LastStatusDate , Application.PaidFees 
                                                 ,InternationalLicenseID , ApplicationID , DriverID , IssuedUsingLocalLicenseID , IssuedDate , ExpirationDateTime , IsActive , CreatedByUserID);
            }

            return null;
        }
  

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return ClsInternationalLicenseDB.GetDriverInternationalLicenses(DriverID) ;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return ClsInternationalLicenseDB.GetAllInternationalLicenses();
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return ClsInternationalLicenseDB.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
    }

    public class ClsDetainLicense
    {
        private enum enMode {enAddNewDetainLicenseMode = 1 , enUpdateDetainLicenseMode = 2 }
        enMode _Mode = enMode.enAddNewDetainLicenseMode;

        private bool _AddNewDetainLicense()
        {
            this.DetainLicenseID = ClsDetainLicenseDB.AddNewDetainLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);

            return (this.DetainLicenseID != -1);
        }

        private bool _UpdateDetainLicense()
        {
            return ClsDetainLicenseDB.UpdateDetainLicense(this.DetainLicenseID , this.LicenseID , this.DetainDate , this.FineFees , this.CreatedByUserID );
        }

        private ClsDetainLicense(int DetainLicenseID, int LicenseID, DateTime DetainDate, Decimal FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleasedDate, int ReleasedByUserID, int ReleaseByApplicationID)
        {
            this.DetainLicenseID = DetainLicenseID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedUserInfo = ClsUser.FindUserByUserID(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleasedDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.RelesedUserInfo = ClsUser.FindUserByUserID(ReleasedByUserID);
            this.ReleaseByApplicationID = ReleaseByApplicationID;

            this._Mode = enMode.enUpdateDetainLicenseMode;
        }

        public int DetainLicenseID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }

        public ClsUser CreatedUserInfo;
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }

        public ClsUser RelesedUserInfo;
        public int ReleaseByApplicationID { get; set; }


        public ClsDetainLicense()
        {
            this.DetainLicenseID = -1;
            this.LicenseID = 0;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = 0;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleaseByApplicationID = -1;

            this._Mode = enMode.enAddNewDetainLicenseMode;
        }

        public ClsDetainLicense FindDetainLicenseByID(int DetainLicenseID)
        {
            int LicenseID = 0;
            DateTime DetainDate = DateTime.Now;
            decimal FineFees = 0;
            int CreatedByUserID = 0;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = 0;
            int ReleaseByApplicationID = 0;

            if (ClsDetainLicenseDB.FindDetainLicenseByID(DetainLicenseID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseByApplicationID))
            {
                return new ClsDetainLicense(DetainLicenseID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseByApplicationID);
            }

            else return null;
        }

        public static ClsDetainLicense FindDetainLicenseByLocalLicenseID(int LicenseID)
        {
            int DetainLicenseID = 0;
            DateTime DetainDate = DateTime.Now;
            decimal FineFees = 0;
            int CreatedByUserID = 0;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = 0;
            int ReleaseByApplicationID = 0;

            if (ClsDetainLicenseDB.FindDetainLicenseByLocalLicenseID(LicenseID, ref DetainLicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseByApplicationID))
            {
                return new ClsDetainLicense(DetainLicenseID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseByApplicationID);
            }

            else return null;
        }

        public bool Save()
        {
            switch (this._Mode) 
            {
                case enMode.enAddNewDetainLicenseMode:

                    if (this._AddNewDetainLicense())
                    {
                        _Mode = enMode.enUpdateDetainLicenseMode;

                        return true;
                    }
                    else return false;

                case enMode.enUpdateDetainLicenseMode:

                    return _UpdateDetainLicense();

                default : return false;

            }
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return ClsDetainLicenseDB.IsLicenseDetained(LicenseID);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return ClsDetainLicenseDB.GetAllDetainedLicenses();
        }

        public  bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return ClsDetainLicenseDB.ReleaseDetainedLicense(this.DetainLicenseID,
                   ReleasedByUserID, ReleaseApplicationID);
        }

    }
}
    

