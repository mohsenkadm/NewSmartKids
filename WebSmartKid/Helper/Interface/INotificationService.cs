using Entity.Entity;
using WebSmartKid.Classes;

namespace WebSmartKid.Helper.Interface
{
    public interface INotificationService
    {
        Task<ResObj> GetNotificationAll(int? Id);
        Task<ResObj> Post(Notification notification);
    }
}
