using Entity.Entity;
using WebSmartKid.Classes;
using System.Threading.Tasks;

namespace WebSmartKid.Helper.Interface
{
    public interface IPromoCodeService
    {
        Task<ResObj> GetAll(string? name, bool? isActive, int index);
        Task<ResObj> GetById(int id);
        Task<ResObj> Post(PromoCode promoCode);
        Task<ResObj> Update(PromoCode promoCode);
        Task<ResObj> Delete(int id);
        Task<ResObj> ToggleActive(int id, bool isActive);
        Task<ResObj> UsePromoCode(int userId, string promoCodeName);
        Task<ResObj> GetPromoCodeUsage(int? promoCodeId);
        Task<ResObj> CanUsePromoCode(int userId, string promoCodeName);
        Task<ResObj> ValidatePromoCodeForOrder(int userId, string promoCodeName);
    }
}
