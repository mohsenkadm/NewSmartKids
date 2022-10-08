using WebSmartKid.Classes;       
using System.Threading.Tasks;
using Entity.Entity;

namespace WebSmartKid.Helper.Interface
{
    public interface ITblAgesService
    {
        Task<ResObj> GetAll();
        Task<ResObj> Post(TblAges TblAges); 
        Task<ResObj> Update(TblAges TblAges);
        Task<ResObj> Delete(int Id);
        Task<TblAges> GetTblAgesById(int Id);
        Task<ResObj> GetById(int Id);
        Task<ResObj> SetProductAndAge(ProductAndAge productAndAge);
        Task<ResObj> SetSalactAllProductAndAge(ProductAndAge productAndAge);
        Task<ResObj> GetProductAndAge(int id);
    }
}
