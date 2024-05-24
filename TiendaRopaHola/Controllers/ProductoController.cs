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

        public async Task<IActionResult> Update(int? id)
        {
            if (id != null)
            {
                Producto producto = await _unitWork.Producto.GetById(id.GetValueOrDefault());
                if (producto != null)
                {
                    return View(producto);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Producto producto)
        {
            if (ModelState.IsValid)
            {
                var existingProducto = await _unitWork.Producto.GetById(producto.Id);
                if (existingProducto != null)
                {
                    _unitWork.Producto.Update(producto);
                    await _unitWork.Save();
                    return Json(new { success = true, message = "Producto actualizado correctamente" });
                }
            }
            return Json(new { success = false, message = "Error al crear Producto" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unitWork.Producto.GetById(id);
            if (productoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Producto" });
            }

            _unitWork.Producto.Remove(productoDb);
            await _unitWork.Save();
            return Json(new { success = true, message = "Producto borrado correctamente" });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
