using System.Threading.Tasks;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.DataAccess
{
    public interface IDataAccess<T> where T : IModelElement
    {
        Task<T> Create(T model);
        Task<T> Update(T model);
        Task<T> Delete(T model);
    }
}
