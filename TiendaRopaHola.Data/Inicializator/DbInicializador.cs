using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TiendaRopaHola.Data.Data;

namespace TiendaRopaHola.Data.Inicializator
{
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;

        public DbInicializador(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
