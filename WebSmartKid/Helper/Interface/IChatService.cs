using Entity.Entity;
using WebSmartKid.Classes;

namespace WebSmartKid.Helper.Interface
{
    public interface IChatService
    {
        Task<ResObj> Register(Users users);
        Task<ResObj> GetMessageChat(int UserReciverId);
        Task<ResObj> PostMessage(Messages messages);
        Task<ResObj> GetMessageList();
    }
}
