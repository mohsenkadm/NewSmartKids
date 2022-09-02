using Entity.Entity;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;

namespace WebSmartKid.Helper.Repository
{
    public class NotificationService    : INotificationService
    {
        private readonly DB_Context _context;

        public NotificationService(
            DB_Context context)
        {
            _context = context;
        }

        public async Task<ResObj> Post(Notification notification)
        {
            notification.DateInsert = Key.DateTimeIQ;
            
            await _context.Notification.AddAsync(notification);
            await _context.SaveChangesAsync();
            return Result.Return(true, "تم الحفظ بنجاح");
        }


    }
}
