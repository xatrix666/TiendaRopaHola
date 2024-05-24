using Moq;
using Microsoft.AspNetCore.Mvc;
using TiendaRopaHola.Controllers;
using TiendaRopaHola.Data.Repositories.IRepository;
using TiendaRopaHola.Models;

namespace TiendaRopaHola.Tests.Controllers
{
    public class ProductoControllerTests
    {
        private readonly Mock<IUnitWork> _mockUnitWork;
        private readonly ProductoController _controller;

        public ProductoControllerTests()
        {
            _mockUnitWork = new Mock<IUnitWork>();
            _controller = new ProductoController(_mockUnitWork.Object);
        }

        [Fact]
        public async Task GetAll_ListOfProductos()
        {
            var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" }
            };
            _mockUnitWork.Setup(repo => repo.Producto.GetAll()).ReturnsAsync(productos);

            var result = await _controller.GetAll();

            var jsonResult = Assert.IsType<JsonResult>(result);
            var jsonValue = jsonResult.Value;
            var dataProperty = jsonValue.GetType().GetProperty("data");
            var dataValue = dataProperty.GetValue(jsonValue, null);
            var productList = Assert.IsAssignableFrom<IEnumerable<Producto>>(dataValue);
            Assert.Equal(productos, productList);
        }

        [Fact]
        public async Task Insert_AddProducto()
        {
            var newProduct = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };
            _mockUnitWork.Setup(repo => repo.Producto.Add(It.IsAny<Producto>())).Returns(Task.CompletedTask);

            var result = await _controller.Insert(newProduct);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var jsonValue = jsonResult.Value;
            var successProperty = jsonValue.GetType().GetProperty("success");
            var successValue = successProperty.GetValue(jsonValue, null);
            Assert.True((bool)successValue);
        }

        [Fact]
        public async Task Update_UpdateProducto()
        {
            var existingProducto = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };

            _mockUnitWork.Setup(repo => repo.Producto.GetById(existingProducto.Id)).ReturnsAsync(existingProducto);
            _mockUnitWork.Setup(repo => repo.Producto.Update(It.IsAny<Producto>())).Verifiable();
            _mockUnitWork.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            var result = await _controller.Update(existingProducto) as JsonResult;

            var jsonValue = result.Value;
            var successProperty = jsonValue.GetType().GetProperty("success");
            var successValue = successProperty.GetValue(jsonValue, null);

            Assert.NotNull(result);
            Assert.True((bool)successValue);
        }


        [Fact]
        public async Task Delete_RemoveProducto()
        {
            var productToDelete = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };

            _mockUnitWork.Setup(repo => repo.Producto.GetById(productToDelete.Id)).ReturnsAsync(productToDelete);

            var result = await _controller.Delete(productToDelete.Id);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var jsonValue = jsonResult.Value;
            var successProperty = jsonValue.GetType().GetProperty("success");
            var successValue = successProperty.GetValue(jsonValue, null);

            Assert.True((bool)successValue);
        }
    }
}
