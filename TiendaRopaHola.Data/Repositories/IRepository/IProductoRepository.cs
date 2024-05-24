using TiendaRopaHola.Models;

namespace TiendaRopaHola.Data.Repositories.IRepository
{
    public interface IProductoRepository : IRepository<Producto>
    {
        void Update(Producto producto);
    }
}
