using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [Route("Producto/GetAll")]
        [SwaggerOperation(Summary = "Obtiene todos los productos", Description = "Obtener todos los productos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto/s listados correctamente.")]

        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Producto> all = await _unitWork.Producto.GetAll();
            return Ok(new { data = all });
        }

        [HttpGet]
        public IActionResult Insert()
        {
            return View(new Producto());
        }

        [HttpPost]
        [Route("Producto/Insert")]
        [SwaggerOperation(Summary = "CrearProducto", Description = "Crea un nuevo producto.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto creado.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Modelo inválido.")]
        public async Task<IActionResult> Insert([FromBody] Producto producto)
        {
            if (producto == null || !ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Error al crear Producto" });
            }

            await _unitWork.Producto.Add(producto);
            await _unitWork.Save();
            return Ok(new { success = true, message = "Producto creado correctamente" });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Producto producto = await _unitWork.Producto.GetById(id.GetValueOrDefault());
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPut]
        [Route("Producto/Update")]
        [SwaggerOperation(Summary = "ActualizarProducto", Description = "Actualiza un producto.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto actualizado.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Modelo inválido.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado.")]
        public async Task<IActionResult> Update([FromBody] Producto producto)
        {
            if (producto == null || !ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Error al actualizar Producto" });
            }

            var existingProducto = await _unitWork.Producto.GetById(producto.Id);
            if (existingProducto == null)
            {
                return NotFound(new { success = false, message = "Producto no encontrado." });
            }

            _unitWork.Producto.Update(producto);
            await _unitWork.Save();
            return Ok(new { success = true, message = "Producto actualizado correctamente" });
        }

        [HttpDelete]
        [Route("Producto/Delete/{id}")]
        [SwaggerOperation(Summary = "EliminarProducto", Description = "Elimina un producto.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto eliminado.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Modelo inválido.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado.")]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unitWork.Producto.GetById(id);
            if (productoDb == null)
            {
                return NotFound(new { success = false, message = "Producto no encontrado." });
            }

            _unitWork.Producto.Remove(productoDb);
            await _unitWork.Save();
            return Ok(new { success = true, message = "Producto borrado correctamente" });
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
