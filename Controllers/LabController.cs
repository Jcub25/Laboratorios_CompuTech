using CampusTech.Data;
using CampusTech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CampusTech.Controllers
{
    public class LabController : Controller
    {
        private static readonly string[] Labs = new[] { "Lab-01", "Lab-02", "Lab-03", "Lab-Redes", "Lab-IA" };

        // Para mostrar las listas de reservas
        public IActionResult Index()
        {
            var reservas = LabRepo.ObtenerTodos();
            return View(reservas);
        }

        // Para mostrar el formulario para agregar una reserva
        public IActionResult Create()
        {
            ViewBag.Labs = new SelectList(Labs);
            return View(new Lab { FechaReserva = DateTime.Today });
        }

        // Post para recibir el formulario completado
        [HttpPost]
        [ValidateAntiForgeryToken] //Para agregar algo de seguridad
        public IActionResult Create(Lab model)
        {
            ViewBag.Labs = new SelectList(Labs, model?.Laboratorio);

            // Validaciones Manuales

            // HoraFin debe ser mayor que HoraInicio
            if (model.HoraFin <= model.HoraInicio)
            {
                ModelState.AddModelError(nameof(model.HoraFin),
                    "La hora de finalizacion debe ser mayor que la hora de inicio.");
            }

            // Para validar la duracion
            var duracion = model.HoraFin - model.HoraInicio;
            if (duracion <= TimeSpan.Zero)
            {
                ModelState.AddModelError(nameof(model.HoraFin),
                    "La reserva debe tener una duración positiva.");
            }

            // Fecha ya validada por [FechaNoPasada], pero revisamos null/valores por seguridad
            if (model.FechaReserva == default)
            {
                ModelState.AddModelError(nameof(model.FechaReserva), "Fecha inválida.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Para intentar agregar la informacion en el repo de memoria o devolverle al usuario los errores
            var (ok, error) = LabRepo.TryAgregar(model);
            if (!ok)
            {
                ModelState.AddModelError(nameof(model.CodigoReserva), error);
                return View(model);
            }

            TempData["Mensaje"] = $"Reserva '{model.CodigoReserva}' registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
