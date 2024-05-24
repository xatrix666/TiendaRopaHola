namespace TiendaRopaHola.Data.Repositories.IRepository
{
    public interface IUnitWork : IDisposable
    {
        IProductoRepository Producto { get; }
        Task Save();
    }
}
