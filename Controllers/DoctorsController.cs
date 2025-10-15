using Microsoft.AspNetCore.Mvc;
using PruebaHospital.Models;
using PruebaHospital.Services;

namespace PruebaHospital.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly DoctorService _doctorService;

        public DoctorsController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        
        public IActionResult Index(string? speciality = null)
        {
            IEnumerable<Doctor> doctors;
    
            if (!string.IsNullOrEmpty(speciality))
            {
                doctors = _doctorService.GetBySpeciality(speciality);
                ViewBag.FilterMessage = $"Filtrando por: {speciality}";
            }
            else
            {
                doctors = _doctorService.GetAll();
            }

            ViewBag.Specialities = _doctorService.GetSpecialities();
            return View(doctors);
        }
        
        public IActionResult Filter()
        {
            ViewBag.Specialities = _doctorService.GetSpecialities();
            return View(new List<Doctor>());
        }

        [HttpPost]
        public IActionResult Filter(string speciality, string name)
        {
            IEnumerable<Doctor> doctors = _doctorService.GetAll();

            if (!string.IsNullOrEmpty(speciality))
            {
                doctors = doctors.Where(d => d.Speciality.ToLower().Contains(speciality.ToLower()));
            }

            if (!string.IsNullOrEmpty(name))
            {
                doctors = doctors.Where(d => d.Name.ToLower().Contains(name.ToLower()));
            }

            ViewBag.Specialities = _doctorService.GetSpecialities();
            ViewBag.SelectedSpeciality = speciality;
            ViewBag.SelectedName = name;
            
            return View(doctors);
        }

        public IActionResult Details(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _doctorService.Create(doctor);
                    TempData["SuccessMessage"] = "Médico creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(doctor);
        }

        public IActionResult Edit(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _doctorService.Update(doctor);
                    TempData["SuccessMessage"] = "Médico actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(doctor);
        }

        public IActionResult Delete(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _doctorService.Delete(id);
                TempData["SuccessMessage"] = "Médico eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar médico: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}