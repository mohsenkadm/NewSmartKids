
using WebSmartKid.Classes;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace WebSmartKid.Helper.Interface
{
    public interface IOrdersService
    { 
        Task<ResObj> Delete(int Id);
        Task<ResObj> GetById(int Id);
        Task<ResObj> SetIsApporve(int id);
        Task<ResObj> SetIsCancel(int id);
        Task<ResObj> SetIsDone(int id);
        Task<ResObj> GetAll(string? orderNo, string? UserName, DateTime datefrom, DateTime dateto);
        Task<ResObj> GetOrdersWithDetailAll(int Id);
        Task<ResObj> Post(List<OrderDetail> orderDetail);
        Task<ResObj> DeleteDetails(int id);
        Task<ResObj> GetOrdersByOrderNo(string orderNo, int? id);  
    }
}
