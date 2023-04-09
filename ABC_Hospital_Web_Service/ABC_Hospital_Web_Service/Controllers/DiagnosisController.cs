using ABC_Hospital_Web_Service.Models;
using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosisController : ControllerBase
    {
        private DiagnosisService _diagnosisService;

        public DiagnosisController()
        {
            _diagnosisService = new DiagnosisService();
        }

        [HttpGet("GetDiagnoses")]
        public ActionResult<string> GetDiagnoses()
        {
            return _diagnosisService.GetDiagnoses();
        }
        [HttpGet("GetDiagnosisByID")]
        public ActionResult<string> GetDiagnosisByID(string diagnosisId)
        {
            return _diagnosisService.GetDiagnosisByID(diagnosisId);
        }
        [HttpGet("GetDiagnosesByPatient")]
        public ActionResult<string> GetDiagnosesByPatient(string patientUsername)
        {
            return _diagnosisService.GetDiagnosesByPatient(patientUsername);
        }
        [HttpPut("CreateDiagnosis")]
        public ActionResult<string> CreateDiagnosis(DiagnosisObject diagnosis)
        {
            return _diagnosisService.CreateDiagnosis(diagnosis);
        }
        [HttpPut("UpdateDiagnosis")]
        public void UpdateDiagnosis(DiagnosisObject diagnosis)
        {
            _diagnosisService.UpdateDiagnosis(diagnosis);
        }
        [HttpDelete("DeleteDiagnosis")]
        public void DeleteDiagnosis(string diagnosis_ID)
        {
            _diagnosisService.DeleteDiagnosis(diagnosis_ID);
        }
    }
}
