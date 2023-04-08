using ABC_Hospital_Web_Service.Models;
using ABC_Hospital_Web_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Hospital_Web_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private PrescriptionService _prescriptionService;

        public PrescriptionController()
        {
            _prescriptionService = new PrescriptionService();
        }

        [HttpGet("GetPrescriptions")]
        public ActionResult<string> GetPrescriptions()
        {
            return _prescriptionService.GetPrescriptions();
        }
        [HttpGet("GetPrescriptionByID")]
        public ActionResult<string> GetPrescriptionByID(string prescriptionId)
        {
            return _prescriptionService.GetPrescriptionByID(prescriptionId);
        }
        [HttpGet("GetPrescriptionsByPatient")]
        public ActionResult<string> GetPrescriptionsByPatient(string patientUsername)
        {
            return _prescriptionService.GetPrescriptionsByPatient(patientUsername);
        }
        [HttpPut("CreatePrescription")]
        public ActionResult<string> CreatePrescription(PrescriptionObject prescription)
        {
            return _prescriptionService.CreatePrescription(prescription);
        }
        [HttpPut("UpdatePrescription")]
        public void UpdatePrescription(PrescriptionObject prescription)
        {
            _prescriptionService.UpdatePrescription(prescription);
        }
        [HttpDelete("DeletePrescription")]
        public void DeletePrescription(string prescription_ID)
        {
            _prescriptionService.DeletePrescription(prescription_ID);
        }
    }
}
