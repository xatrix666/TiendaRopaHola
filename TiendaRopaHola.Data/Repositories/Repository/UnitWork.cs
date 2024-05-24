using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaRopaHola.Data.Data;
using TiendaRopaHola.Data.Repositories.IRepository;

namespace TiendaRopaHola.Data.Repositories.Repository
{
    public class UnitWork : IUnitWork
    {
        private readonly ApplicationDbContext _db;

        public IProductoRepository Producto { get; private set; }

        public UnitWork(ApplicationDbContext db)
        {
            _db = db;
            Producto = new ProductoRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
