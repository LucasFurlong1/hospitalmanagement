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
        public PatientController(IConfiguration appConfig)
        {
            _patientService = new PatientService(appConfig);
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
        public ActionResult<string> CreatePatient([FromBody] NewPatientObject patient)
        {
            return _patientService.CreatePatient(patient);
        }
    }
}
