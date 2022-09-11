using WebSmartKid.Classes;       
using System.Threading.Tasks;
using Entity.Entity;

namespace WebSmartKid.Helper.Interface
{
    public interface ITypeDiscountService
    {
        Task<ResObj> GetAll(string? NameDis);
        Task<ResObj> Post(TypeDiscount TypeDiscount); 
        Task<ResObj> Update(TypeDiscount TypeDiscount);
        Task<ResObj> Delete(int Id);
        Task<TypeDiscount> GetTypeDiscountById(int Id);
        Task<ResObj> GetById(int Id);
    }
}
