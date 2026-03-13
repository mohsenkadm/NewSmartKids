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
            foreach (var item in orderDetail)
            {
                _context.Entry(item).State = EntityState.Deleted;
            }
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
            int UserId = firstrow.UserId;
            Users user = null;

            // Handle User Creation/Update
            if (UserId == 0)
            {
                user = new Users
                {
                    Name = firstrow.Name,
                    Details = firstrow.Detail,
                    Phone = firstrow.Phone,
                    CountryId = firstrow.CountryId,
                    Address = firstrow.Address,
                    AccountBalance = 0
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                firstrow.UserId = user.UserId;
                UserId = user.UserId;
            }
            else if (UserId > 0)
            {
                user = await _context.Users.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
                if (user == null)
                    return Result.Return(false, "المستخدم غير موجود");

                user.Name = firstrow.Name;
                user.Details = firstrow.Detail;
                user.Phone = firstrow.Phone;
                user.CountryId = firstrow.CountryId;
                user.Address = firstrow.Address;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            // Handle Promo Code
            PromoCode promoCode = null;
            decimal promoAmount = 0;

            if (!string.IsNullOrEmpty(firstrow.PromoCode))
            {
                // Check if promo code exists and is active
                promoCode = await _context.PromoCode
                    .Where(p => p.Name == firstrow.PromoCode && p.IsActive)
                    .FirstOrDefaultAsync();

                //if (promoCode == null)
                //{
                //    return Result.Return(false, "كود الخصم غير صحيح أو غير نشط");
                //}

                // Check if user already used this promo code
                //var alreadyUsed = await _context.UserPromoCode
                //    .AnyAsync(up => up.UserId == UserId && up.PromoCodeId == promoCode.PromoCodeId);

                //if (alreadyUsed)
                //{
                //    return Result.Return(false, "لقد استخدمت هذا الكود من قبل");
                //}

                //promoAmount = promoCode.Amount;

                //// Add promo amount to user balance
                //user.AccountBalance += promoAmount;
                //_context.Entry(user).State = EntityState.Modified;
                //await _context.SaveChangesAsync();

                //// Record promo code usage
                //var userPromoCode = new UserPromoCode
                //{
                //    UserId = UserId,
                //    PromoCodeId = promoCode.PromoCodeId,
                //    UsedDate = Key.DateTimeIQ
                //};
                //await _context.UserPromoCode.AddAsync(userPromoCode);
                //await _context.SaveChangesAsync();
            }

            // Calculate order amounts
            decimal orderTotal = firstrow.Total;
            decimal orderNetAmount = firstrow.NetAmount;
            decimal orderTotalDiscount = firstrow.TotalDiscount;
            
            // Calculate how much balance to use
            decimal usedBalance = 0;
            decimal finalAmount = orderNetAmount;

            if (user.AccountBalance > 0)
            {
                if (user.AccountBalance >= orderNetAmount)
                {
                    // User has enough balance to cover entire order
                    usedBalance = orderNetAmount;
                    finalAmount = 0;
                }
                else
                {
                    // Use all available balance
                    usedBalance = user.AccountBalance;
                    finalAmount = orderNetAmount - usedBalance;
                }

                // Deduct used balance from user account
                user.AccountBalance -= usedBalance;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            // Generate OrderNo
            var maxOrderNo = await _context.Orders
                .Where(o => o.OrderNo != null)
                .MaxAsync(o => (int?)o.OrderNo);
            
            int newOrderNo;
            if (maxOrderNo == null || maxOrderNo == 0)
            {
                // First order - start from 1000
                newOrderNo = 1000;
            }
            else
            {
                // Increment from max
                newOrderNo = maxOrderNo.Value + 1;
            }

            // Create Order
            Orders orders = new Orders
            {
                OrderNo = newOrderNo,
                OrderDate = Key.DateTimeIQ,
                UserId = UserId,
                IsApporve = false,
                IsCancel = false,
                IsDone = false,
                Total = orderTotal,
                NetAmount = orderNetAmount,
                TotalDiscount = orderTotalDiscount,
                UsedAccountBalance = usedBalance,
                FinalAmount = finalAmount,
                PromoCodeId = promoCode?.PromoCodeId,
                PromoCodeName = promoCode?.Name
            };

            await _context.Orders.AddAsync(orders);
            await _context.SaveChangesAsync();

            // Add Order Details
            orderDetail.ForEach(x => x.OrderId = orders.OrderId);
            await _context.OrderDetail.AddRangeAsync(orderDetail);
            await _context.SaveChangesAsync();

            // Update Product Inventory (reduce stock count)
            foreach (var detail in orderDetail)
            {
                if (detail.ProductsId > 0)
                {
                    var product = await _context.Products
                        .Where(p => p.ProductsId == detail.ProductsId)
                        .FirstOrDefaultAsync();

                    if (product != null)
                    {
                        // Reduce inventory by ordered quantity
                        product.Count -= detail.Count;
                        
                        // Ensure count doesn't go negative (optional safety check)
                        if (product.Count < 0)
                        {
                            product.Count = 0;
                        }

                        _context.Entry(product).State = EntityState.Modified;
                    }
                }
            }
            await _context.SaveChangesAsync();

            // Generate token
            orders.Token = JsonWebToken.GenerateToken(new UserManager() { Id = UserId, Name = firstrow.Name });

            // Return success with order info
            var resultMessage = $"تم حفظ الطلب بنجاح";
            if (promoAmount > 0)
            {
                resultMessage += $"\nتم إضافة {promoAmount} د.ع إلى رصيدك";
            }
            if (usedBalance > 0)
            {
                resultMessage += $"\nتم استخدام {usedBalance} د.ع من رصيدك";
            }
            if (finalAmount > 0)
            {
                resultMessage += $"\nالمبلغ المتبقي للدفع: {finalAmount} د.ع";
            }
            else
            {
                resultMessage += $"\nتم الدفع بالكامل من رصيدك";
            }

            return Result.Return(true, resultMessage, orders);
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
