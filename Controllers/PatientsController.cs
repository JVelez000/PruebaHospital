using Microsoft.AspNetCore.Mvc;
using PruebaHospital.Models;
using PruebaHospital.Services;

namespace PruebaHospital.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var patients = _service.GetAll();
            return View(patients);
        }

        public IActionResult Details(int id)
        {
            var patient = _service.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid) return View(patient);
            _service.Create(patient);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var patient = _service.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient patient)
        {
            if (!ModelState.IsValid) return View(patient);
            _service.Update(patient);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var patient = _service.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}