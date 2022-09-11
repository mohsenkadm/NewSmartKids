using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;          
using WebSmartKid.Model.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Entity;

namespace WebSmartKid.Helper.Repository
{
    public class TypeDiscountService : ITypeDiscountService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;                             

        public TypeDiscountService(
            DB_Context context )
        {
            _context = context;                   
        }

        public async Task<ResObj> GetAll(string? NameDis)
        {
            List<TypeDiscount> data;
            if(NameDis.IsEmpty())
            data= await _context.TypeDiscount.ToListAsync() ;
            else
            data= await _context.TypeDiscount.Where(i=>i.NameDis== NameDis).ToListAsync() ;
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(TypeDiscount TypeDiscount)
        {      
            await _context.TypeDiscount.AddAsync(TypeDiscount);
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",TypeDiscount);
        }

        public async Task<ResObj> Update(TypeDiscount TypeDiscount)
        {                                                                       
            TypeDiscount TypeDiscount1 = await GetTypeDiscountById(TypeDiscount.Id);
            if (TypeDiscount1 is null)
                return Result.Return(false, "حدث خطا اثناء عملية جلب البيانات");
                                                               
            TypeDiscount1.Price = TypeDiscount.Price;      
            _context.Entry(TypeDiscount1).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",TypeDiscount1);
        }



        public async Task<ResObj> Delete(int Id)
        {
            TypeDiscount TypeDiscount1 = await GetTypeDiscountById(Id);
            _context.Entry(TypeDiscount1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<TypeDiscount> GetTypeDiscountById(int Id)
        {
            return await _context.TypeDiscount.Where(i => i.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<ResObj> GetById(int Id)
        {
            TypeDiscount TypeDiscount = await GetTypeDiscountById(Id);
            return Result.Return(true, TypeDiscount);
        }
    }
}
