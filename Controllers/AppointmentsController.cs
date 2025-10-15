using Microsoft.AspNetCore.Mvc;
using PruebaHospital.Models;
using PruebaHospital.Services;

namespace PruebaHospital.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

        public AppointmentsController(AppointmentService appointmentService, PatientService patientService, DoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public IActionResult Index(string filter = "all")
        {
            IEnumerable<Appointment> appointments;

            switch (filter.ToLower())
            {
                case "today":
                    appointments = _appointmentService.GetTodayAppointments();
                    ViewBag.FilterMessage = "Citas de Hoy";
                    break;
                case "upcoming":
                    appointments = _appointmentService.GetUpcomingAppointments();
                    ViewBag.FilterMessage = "Próximas Citas (7 días)";
                    break;
                case "pending":
                    appointments = _appointmentService.GetByStatus(AppointmentStatus.Pendiente);
                    ViewBag.FilterMessage = "Citas Pendientes";
                    break;
                case "confirmed":
                    appointments = _appointmentService.GetByStatus(AppointmentStatus.Confirmada);
                    ViewBag.FilterMessage = "Citas Confirmadas";
                    break;
                default:
                    appointments = _appointmentService.GetAll();
                    ViewBag.FilterMessage = "Todas las Citas";
                    break;
            }

            ViewBag.Statistics = _appointmentService.GetAppointmentStatistics();
            return View(appointments);
        }
        
        public IActionResult ByPatient(int patientId)
        {
            var appointments = _appointmentService.GetByPatientId(patientId);
            var patient = _patientService.GetById(patientId);
            
            ViewBag.FilterMessage = patient != null ? $"Citas del Paciente: {patient.FullName}" : "Citas del Paciente";
            ViewBag.Statistics = _appointmentService.GetAppointmentStatistics();
            
            return View("Index", appointments);
        }

        public IActionResult ByDoctor(int doctorId)
        {
            var appointments = _appointmentService.GetByDoctorId(doctorId);
            var doctor = _doctorService.GetById(doctorId);
            
            ViewBag.FilterMessage = doctor != null ? $"Citas del Médico: {doctor.Name}" : "Citas del Médico";
            ViewBag.Statistics = _appointmentService.GetAppointmentStatistics();
            
            return View("Index", appointments);
        }

        public IActionResult Details(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        public IActionResult Create()
        {
            ViewBag.Patients = _patientService.GetAll();
            ViewBag.Doctors = _doctorService.GetAll();
            ViewBag.MinDate = DateTime.Today.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment appointment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _appointmentService.Create(appointment);
                    TempData["SuccessMessage"] = "Cita creada exitosamente. Se envió un email de confirmación.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Patients = _patientService.GetAll();
            ViewBag.Doctors = _doctorService.GetAll();
            ViewBag.MinDate = DateTime.Today.ToString("yyyy-MM-dd");
            return View(appointment);
        }

        public IActionResult Edit(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            ViewBag.Patients = _patientService.GetAll();
            ViewBag.Doctors = _doctorService.GetAll();
            ViewBag.MinDate = DateTime.Today.ToString("yyyy-MM-dd");
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Appointment appointment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _appointmentService.Update(appointment);
                    TempData["SuccessMessage"] = "Cita actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Patients = _patientService.GetAll();
            ViewBag.Doctors = _doctorService.GetAll();
            ViewBag.MinDate = DateTime.Today.ToString("yyyy-MM-dd");
            return View(appointment);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsAttended(int id)
        {
            try
            {
                _appointmentService.MarkAsAttended(id);
                TempData["SuccessMessage"] = "Cita marcada como atendida.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id, string reason)
        {
            try
            {
                _appointmentService.Cancel(id, reason);
                TempData["SuccessMessage"] = "Cita cancelada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(int id)
        {
            try
            {
                _appointmentService.Confirm(id);
                TempData["SuccessMessage"] = "Cita confirmada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsNoShow(int id)
        {
            try
            {
                _appointmentService.MarkAsNoShow(id);
                TempData["SuccessMessage"] = "Cita marcada como no asistió.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _appointmentService.Delete(id);
                TempData["SuccessMessage"] = "Cita eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar cita: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
        
        public JsonResult GetAvailableTimeSlots(int doctorId, string date)
        {
            try
            {
                var appointmentDate = DateTime.Parse(date);
                var timeSlots = _appointmentService.GetAvailableTimeSlots(doctorId, appointmentDate);
                return Json(new { success = true, timeSlots });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReminder(int id)
        {
            try
            {
                var (success, message) = await _appointmentService.SendReminderAsync(id);
        
                if (success)
                {
                    TempData["SuccessMessage"] = "Recordatorio enviado exitosamente al paciente";
                }
                else
                {
                    TempData["ErrorMessage"] = message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
    
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmation(int id)
        {
            try
            {
                var (success, message) = await _appointmentService.ResendConfirmationAsync(id);
        
                if (success)
                {
                    TempData["SuccessMessage"] = "Confirmación reenviada exitosamente al paciente";
                }
                else
                {
                    TempData["ErrorMessage"] = message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
    
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}