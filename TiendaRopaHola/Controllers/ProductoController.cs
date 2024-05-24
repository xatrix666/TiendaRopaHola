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


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Producto.GetAll();
            return Json(new { data = all });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
