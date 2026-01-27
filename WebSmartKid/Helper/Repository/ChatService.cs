using Entity.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;
using WebSmartKid.Model.General;

namespace WebSmartKid.Helper.Repository
{
    public class ChatService: IChatService  , IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;
        private readonly IDapperRepository<Users> _usersRepository;
        private readonly IDapperRepository<Messages> _messageRepository;

        public ChatService(
            DB_Context context,
            IDapperRepository<Users> orderRepository,
            IDapperRepository<Messages> OrderDetailRepository
            )
        {
            _context = context;
            _usersRepository = orderRepository;
            _messageRepository = OrderDetailRepository;
        }

        public async Task<ResObj> Register(Users users)
        {
            Users _users = new()
            {
                Name = users.Name,
                Details = users.Details,
                Phone = users.Phone
        ,         CountryId = users.CountryId
            };
            await _context.Users.AddAsync(_users);
            await _context.SaveChangesAsync();
            
            return Result.Return(true, "تم حفظ معلوماتك بنجاح", _users);
        }
                                                                         
        public async Task<ResObj> GetMessageChat(int UserReciverId)
        {      
                List<Messages> messages = await _messageRepository.GetEntityListAsync("dbo.GetMessageChat", new { UserReciverId }); 
                return Result.Return(true, messages);
            
        }                                                              
        public async Task<ResObj> GetMessageList()
        {      
                List<Messages> messages = await _messageRepository.GetEntityListAsync("dbo.GetMessageList", new {  }); 
                return Result.Return(true, messages);
            
        }                                                
        public async Task<ResObj> PostMessage(Messages messages)
        {        
                messages.Date = Key.DateTimeIQ;
                await _context.AddAsync(messages);
                await _context.SaveChangesAsync();
                return Result.Return(true, messages);
        }
    }
}
