using TiendaRopaHola.Data.Data;
using TiendaRopaHola.Data.Repositories.IRepository;
using TiendaRopaHola.Models;

namespace TiendaRopaHola.Data.Repositories.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Producto producto)
        {
            var productoBD = _db.Productos.FirstOrDefault(p => p.Id == producto.Id);
            if (productoBD != null)
            {
                productoBD.Nombre = producto.Nombre;
                productoBD.Talla = producto.Talla;
                productoBD.Color = producto.Color;
                productoBD.Precio = producto.Precio;
                productoBD.Descripcion = producto.Descripcion;
                _db.SaveChanges();
            }
        }
    }
}
