using ABC_Hospital_Web_Service.Models;
using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private PatientService _patientService;
        //private readonly ILogger<PatientController> _logger;
        public PatientController()//(ILogger<PatientController> logger)
        {
            _patientService = new PatientService();// (logger);
            //_logger = logger;
        }

        [HttpGet("GetPatientInfo")]
        public ActionResult<string> GetPatientInfo(string patientUsername)
        {
            return _patientService.GetPatientInfo(patientUsername);
        }
        [HttpGet("GetPatientsByDoctor")]
        public ActionResult<string> GetPatientsByDoctor(string doctorUsername)
        {
            return _patientService.GetPatientsByDoctor(doctorUsername);
        }
        [HttpPut("CreatePatient")]
        public ActionResult<bool> CreatePatient([FromBody] UserCredObject patientCreds)//, [FromBodyAttribute] PatientObject patient)
        {

            return false;
        }
    }
}
