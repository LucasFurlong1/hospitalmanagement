using ABC_Hospital_Web_Service.Models;
using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private DoctorService _doctorService;

        public DoctorController(IConfiguration appConfig)
        {
            _doctorService = new DoctorService(appConfig);
        }

        [HttpGet("GetDoctorInfo")]
        public ActionResult<string> GetDoctorInfo(string doctorUsername)
        {
            return _doctorService.GetDoctorInfo(doctorUsername);
        }
        [HttpGet("GetDoctorsAcceptingPatients")]
        public ActionResult<string> GetDoctorsAcceptingPatients()
        {
            return _doctorService.GetAcceptingDoctors();
        }
        [HttpPut("CreateDoctor")]
        public ActionResult<string> CreatePatient([FromBody] NewDoctorObject doctor)
        {
            return _doctorService.CreateDoctor(doctor); ;
        }
    }
}
