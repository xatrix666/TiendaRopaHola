using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TiendaRopaHola.Data.Repositories.IRepository;
using TiendaRopaHola.Models;

namespace TiendaRopaHola.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IUnitWork _unitWork;

        public ProductoController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Insert()
        {
            return View(new Producto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Producto.GetAll();
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Producto producto)
        {
            if (ModelState.IsValid)
            {
                if (producto != null)
                {
                    await _unitWork.Producto.Add(producto);
                    await _unitWork.Save();
                    return Json(new { success = true, message = "Producto creado correctamente" });
                }
            }
            return Json(new { success = false, message = "Error al crear Producto" });

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
