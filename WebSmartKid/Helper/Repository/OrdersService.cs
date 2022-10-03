using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;
using Entity.Entity;
using WebSmartKid.Model.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSmartKid.Helper.Interface;

namespace WebSmartKid.Helper.Repository
{
    public class OrdersService : IOrdersService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;
        private readonly IDapperRepository<Orders> _OrdersRepository;
        private readonly IDapperRepository<OrderDetail> _OrderDetailRepository;

        public OrdersService(
            DB_Context context,
            IDapperRepository<Orders> orderRepository,
            IDapperRepository<OrderDetail> OrderDetailRepository
            )
        {
            _context = context;
            _OrdersRepository = orderRepository;
            _OrderDetailRepository = OrderDetailRepository;
        }

          
        public async Task<ResObj> Delete(int Id)
        {
            Orders Orders1 = await GetOrdersById(Id);
            _context.Entry(Orders1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            List<OrderDetail> orderDetail = await GetOrdersDetailById(Id);
            _context.Entry(orderDetail).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }
        // delete detail row by id
        public async Task<ResObj> DeleteDetails(int Id)
        {
            OrderDetail orderDetail = await GetOrdersDetailByOrderDetailId(Id);
            _context.Entry(orderDetail).State = EntityState.Deleted;
            await _context.SaveChangesAsync(); 

            return Result.Return(true, "تم حذف بنجاح");
        }

        // GetOrdersDetailById only Detail
        private async Task<List<OrderDetail>> GetOrdersDetailById(int Id)
        {
            return await _context.OrderDetail.Where(i => i.OrderId == Id).ToListAsync() ; 
        }

        // get orders detail only row
        private async Task<OrderDetail> GetOrdersDetailByOrderDetailId(int Id)
        {
            return await _context.OrderDetail.Where(i => i.OrderDetailId == Id).FirstOrDefaultAsync();  
        }

        // get orders only master
        private async Task<Orders> GetOrdersById(int Id)
        {
            return await _context.Orders.Where(i => i.OrderId == Id).FirstOrDefaultAsync();

        }

        // get orders only master  for control
        public async Task<ResObj> GetById(int Id)
        {
            Orders Orders = await GetOrdersById(Id);
            return Result.Return(true, Orders);
        }

        public async Task<ResObj> SetIsApporve(int id)
        { 
            var orders = await GetOrdersById(id);
            if(orders.IsApporve)
                return Result.Return(false,"تمت الموافقة عليها سابقا");
            orders.IsApporve = true;
            orders.IsCancel = false;

            _context.Entry(orders).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Return(true, "تمت الموافقة",orders);
        }

        public async Task<ResObj> SetIsCancel(int id)
        {
            var orders = await GetOrdersById(id);
            if (orders.IsCancel)
                return Result.Return(false, "تمت الغاء الطلب سابقا");
            orders.IsCancel = true;
            orders.IsApporve = false;
            orders.IsDone = false;

            _context.Entry(orders).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Return(true, "تمت الغاء الموافقة",orders);
        }

        public async Task<ResObj> SetIsDone(int id)
        {
            var orders = await GetOrdersById(id);
            if (orders.IsDone)
                return Result.Return(false, "الطلب تمت وضعه في حالة الانتهاء");
            orders.IsDone = true;
            orders.IsApporve = true;
            orders.IsCancel = false;

            _context.Entry(orders).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Return(true, "تم وضع الطلب في حالة الانتهاء",orders);
        }

        // get all orders only master
        public async Task<ResObj> GetAll(string OrderNo, string? UserName, DateTime datefrom, DateTime dateto)
        {
            List<Orders> data = await _OrdersRepository.GetEntityListAsync("dbo.GetOrderAll", 
                new { OrderNo,UserName,datefrom,dateto });
            return Result.Return(true, data);
        }

        // get orders with master and detail all
        public async Task<ResObj> GetOrdersWithDetailAll(int Id)
        {
            List<OrderDetail> data= await _OrderDetailRepository.GetEntityListAsync("dbo.GetOrdersWithDetailAll",
                new { Id });
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(List<OrderDetail> orderDetail)
        {
            var firstrow = orderDetail.FirstOrDefault();
            int UserId=firstrow.UserId;
            if (UserId == 0)
            {
                Users users = new()
                {
                    Name = firstrow.Name,
                    Details = firstrow.Detail,
                    Phone = firstrow.Phone
                    ,CountryId = firstrow.CountryId  
                    ,Address = firstrow.Address
                };
                await _context.Users.AddAsync(users);
                await _context.SaveChangesAsync();
                firstrow.UserId=users.UserId;   
            }
            else  if(UserId>0)
            {
                Users users = await _context.Users.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
                users.Name = firstrow.Name;
                users.Details = firstrow.Detail;
                users.Phone = firstrow.Phone;
                users.CountryId = firstrow.CountryId;
                users.Address = firstrow.Address;
                _context.Entry(users).State = EntityState.Modified;
                await _context.SaveChangesAsync();                                                                      
                firstrow.UserId = users.UserId;
            }
            Orders orders = new Orders
            {
                OrderDate = Key.DateTimeIQ,
                UserId =firstrow.UserId
                ,  IsApporve = false, IsCancel = false, IsDone = false
                , Total = firstrow.Total  ,NetAmount=firstrow.NetAmount,TotalDiscount=firstrow.TotalDiscount           
            };

            await _context.Orders.AddAsync(orders);
            await _context.SaveChangesAsync();

            orderDetail.ForEach(x => x.OrderId = orders.OrderId);
             
            await _context.OrderDetail.AddRangeAsync(orderDetail);
            await _context.SaveChangesAsync();
            orders.Token = JsonWebToken.GenerateToken(new UserManager() { Id = firstrow.UserId, Name = firstrow.Name });

            return Result.Return(true, "تم حفظ الطلب بنجاح",orders);
        }

        public async Task<ResObj> GetOrdersByOrderNo(string OrderNo, int? Id)
        {
            List<Orders> data;
            if(OrderNo.IsEmpty())
              data=  await _context.Orders.Where(i => i.UserId == Id).ToListAsync();
            else
              data=  await _context.Orders.Where(i => i.UserId == Id && i.OrderNo.Equals(OrderNo)).ToListAsync();
            return Result.Return(true, data);
        }

        
    }
}
