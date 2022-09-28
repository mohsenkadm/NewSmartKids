using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;

namespace WebSmartKid.Helper.Repository
{
    public class NotificationService    : INotificationService,IRegisterScopped
    {
        private readonly DB_Context _context;

        public NotificationService(
            DB_Context context)
        {
            _context = context;
        }

        public async Task<ResObj> GetNotificationAll(int? Id)
        {
            List<Notification> data = await _context.Notification.Where(i=>i.UserId==Id || i.UserId==0).
                OrderByDescending(x=>x.NotificationId).Take(30).ToListAsync();
            return Result.Return(true, data);
        }
        public async Task<ResObj> Post(Notification notification)
        {
            notification.DateInsert = Key.DateTimeIQ;
            notification.UserId = 0;
            await _context.Notification.AddAsync(notification);
            await _context.SaveChangesAsync();
            return Result.Return(true, "تم الحفظ بنجاح");
        }


    }
}
