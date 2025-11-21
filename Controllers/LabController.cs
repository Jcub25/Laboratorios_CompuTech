using Microsoft.AspNetCore.Mvc;

namespace Laboratorios_CompuTech.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
