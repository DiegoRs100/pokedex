namespace Pokedex.Business.Core.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task SaveChanges();
    }
}