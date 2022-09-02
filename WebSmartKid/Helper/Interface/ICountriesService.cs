using WebSmartKid.Classes;       
using System.Threading.Tasks;
using Entity.Entity;

namespace WebSmartKid.Helper.Interface
{
    public interface ICountriesService
    {
        Task<ResObj> GetAll();
        Task<ResObj> Post(Countries Countries); 
        Task<ResObj> Update(Countries Countries);
        Task<ResObj> Delete(int Id);
        Task<Countries> GetCountriesById(int Id);
        Task<ResObj> GetById(int Id);
    }
}
