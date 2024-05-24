using Microsoft.EntityFrameworkCore;
using Moq;
using TiendaRopaHola.Data.Data;
using TiendaRopaHola.Data.Repositories.IRepository;
using TiendaRopaHola.Data.Repositories.Repository;
using TiendaRopaHola.Models;

namespace TiendaRopaHola.Tests.Repositories
{
    public class ProductoRepositoryTests
    {
        private readonly Mock<IUnitWork> _mockUnitWork;
        private readonly ApplicationDbContext _context;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ProductoRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "test_database")
                 .Options;

            _context = new ApplicationDbContext(_options);
            _mockUnitWork = new Mock<IUnitWork>();
        }

        [Fact]
        public async Task GetById_ProductoExists()
        {
            await ClearDBMemory();

            var expectedProduct = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };
            _context.Productos.Add(expectedProduct);
            await _context.SaveChangesAsync();

            var repository = new ProductoRepository(_context);
            _mockUnitWork.Setup(u => u.Producto).Returns(repository);

            var actualProduct = await _mockUnitWork.Object.Producto.GetById(1);

            Assert.Equal(1, actualProduct.Id);
            Assert.Equal("Camiseta", actualProduct.Nombre);
        }

        [Fact]
        public async Task GetAll_AllProductos()
        {
            await ClearDBMemory();

            var expectedProducts = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" },
                new Producto { Id = 2, Nombre = "Pantalon", Talla = 8, Color = "Azul", Precio = 120, Descripcion = "Pantalon corto" }
            };

            _context.Productos.AddRange(expectedProducts);
            _context.SaveChanges();

            var repository = new ProductoRepository(_context);
            _mockUnitWork.Setup(u => u.Producto).Returns(repository);

            var actualProducts = await _mockUnitWork.Object.Producto.GetAll();

            Assert.Equal(expectedProducts.Count, actualProducts.Count());
            foreach (var expectedProduct in expectedProducts)
            {
                Assert.Contains(actualProducts, p => p.Id == expectedProduct.Id);
            }
        }

        [Fact]
        public async Task Add_AddProducto()
        {
            await ClearDBMemory();

            var newProduct = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };
            var repository = new ProductoRepository(_context);
            _mockUnitWork.Setup(u => u.Producto).Returns(repository);

            await _mockUnitWork.Object.Producto.Add(newProduct);
            await _mockUnitWork.Object.Save();

            var productInDb = await _context.Productos.FindAsync(newProduct.Id);
            Assert.NotNull(productInDb);
            Assert.Equal(newProduct.Nombre, productInDb.Nombre);
        }

        [Fact]
        public async Task Update_UpdateProducto()
        {
            await ClearDBMemory();

            var existingProduct = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };
            _context.Productos.Add(existingProduct);
            _context.SaveChanges();

            var repository = new ProductoRepository(_context);
            _mockUnitWork.Setup(u => u.Producto).Returns(repository);
            var updatedProduct = new Producto { Id = 1, Nombre = "Producto Actualizado", Talla = 12, Color = "Azul", Precio = 120, Descripcion = "Descripción actualizada" };

            _mockUnitWork.Object.Producto.Update(updatedProduct);
            await _mockUnitWork.Object.Save();

            var productInDb = await _context.Productos.FindAsync(updatedProduct.Id);
            Assert.NotNull(productInDb);
            Assert.Equal(updatedProduct.Nombre, productInDb.Nombre);
            Assert.Equal(updatedProduct.Talla, productInDb.Talla);
            Assert.Equal(updatedProduct.Color, productInDb.Color);
            Assert.Equal(updatedProduct.Precio, productInDb.Precio);
            Assert.Equal(updatedProduct.Descripcion, productInDb.Descripcion);
        }

        [Fact]
        public async Task Remove_DeleteProducto()
        {
            await ClearDBMemory();

            var productToDelete = new Producto { Id = 1, Nombre = "Camiseta", Talla = 10, Color = "Rojo", Precio = 100, Descripcion = "Camiseta de tirantes" };
            _context.Productos.Add(productToDelete);
            await _context.SaveChangesAsync();

            var repository = new ProductoRepository(_context);
            _mockUnitWork.Setup(u => u.Producto).Returns(repository);

            var product = await _mockUnitWork.Object.Producto.GetById(1);
            _mockUnitWork.Object.Producto.Remove(product);
            _mockUnitWork.Setup(u => u.Save()).Callback(() => _context.SaveChanges());
            await _mockUnitWork.Object.Save();

            var productInDb = await _context.Productos.FindAsync(1);
            Assert.Null(productInDb);
        }

        #region Private Methods

        private async Task ClearDBMemory()
        {
            _context.Productos.RemoveRange(_context.Productos);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
