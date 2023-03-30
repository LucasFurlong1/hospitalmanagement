using ABC_Hospital_Web_Service.Models;
using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private PatientService _patientService;
        private DoctorService _doctorService;
        private PrescriptionService _prescriptionService;
        private DiagnosisService _diagnosisService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _userService = new UserService(logger);
            _patientService = new PatientService(logger);
            _doctorService = new DoctorService(logger);
            _prescriptionService = new PrescriptionService(logger);
            _diagnosisService = new DiagnosisService(logger);
            _logger = logger;
        }

        [HttpPost("LoginRequest")]
        public bool LoginRequest([FromBody] Models.UserCredObject requestObj)
        {
            return false;
        }

        //[HttpGet("CheckUserSession")]


        [HttpGet("GetUser")]
        public string GetUser(string username)
        {
            return _userService.GetUserByUsername(username);
        }
        [HttpGet("GetUsers")]
        public string GetUsers()
        {
            return _userService.GetUsers();
        }
        [HttpGet("GetUsersByAccountType")]
        public string GetUsers(string accountType)
        {
            return _userService.GetUsersByAccountType(accountType);
        }

        [HttpGet("GetPatientInfo")]
        public string GetPatientInfo(string patientUsername)
        {
            return _patientService.GetPatientInfo(patientUsername);
        }
        [HttpGet("GetPatientsByDoctor")]
        public string GetPatientsByDoctor(string doctorUsername)
        {
            return _patientService.GetPatientsByDoctor(doctorUsername);
        }

        [HttpGet("GetDoctorInfo")]
        public string GetDoctorInfo(string doctorUsername)
        {
            return _doctorService.GetDoctorInfo(doctorUsername);
        }

        [HttpGet("GetPrescriptions")]
        public string GetPrescriptions()
        {
            return _prescriptionService.GetPrescriptions();
        }
        [HttpGet("GetPrescriptionByID")]
        public string GetPrescriptionByID(string prescriptionId)
        {
            return _prescriptionService.GetPrescriptionByID(prescriptionId);
        }
        [HttpGet("GetPrescriptionsByPatient")]
        public string GetPrescriptionsByPatient(string patientUsername)
        {
            return _prescriptionService.GetPrescriptionsByPatient(patientUsername);
        }

        [HttpGet("GetDiagnoses")]
        public string GetDiagnoses()
        {
            return _diagnosisService.GetDiagnoses();
        }
        [HttpGet("GetDiagnosisByID")]
        public string GetDiagnosisByID(string diagnosisId)
        {
            return _diagnosisService.GetDiagnosisByID(diagnosisId);
        }
        [HttpGet("GetDiagnosesByPatient")]
        public string GetDiagnosesByPatient(string patientUsername)
        {
            return _diagnosisService.GetDiagnosesByPatient(patientUsername);
        }

    }
}