using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;          
using WebSmartKid.Model.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;

namespace WebSmartKid.Helper.Repository
{
    public class TblAgesService : ITblAgesService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;
        private readonly IDapperRepository<ProductAndAge> _prodService;

        public TblAgesService(
            DB_Context context,IDapperRepository<ProductAndAge> prodService)
        {
            _context = context;
            _prodService = prodService;
        }

        public async Task<ResObj> GetAll()
        {
            List<TblAges> data = await _context.TblAges.ToListAsync() ;
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(TblAges TblAges)
        {      
            await _context.TblAges.AddAsync(TblAges);
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",TblAges);
        }

        public async Task<ResObj> Update(TblAges TblAges)
        {                                                                       
            TblAges TblAges1 = await GetTblAgesById(TblAges.AgeId);
            if (TblAges1 is null)
                return Result.Return(false, "حدث خطا اثناء عملية جلب البيانات");
             
            TblAges1.AgeName = TblAges.AgeName;      
            _context.Entry(TblAges1).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",TblAges1);
        }



        public async Task<ResObj> Delete(int Id)
        {
            TblAges TblAges1 = await GetTblAgesById(Id);
            _context.Entry(TblAges1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<TblAges> GetTblAgesById(int Id)
        {
            return await _context.TblAges.Where(i => i.AgeId == Id).FirstOrDefaultAsync();
        }

        public async Task<ResObj> GetById(int Id)
        {
            TblAges TblAges = await GetTblAgesById(Id);
            return Result.Return(true, TblAges);
        }
        public async Task<ResObj> SetProductAndAge(ProductAndAge productAndAge)
        {
            var re = await _context.ProductAndAge.FirstOrDefaultAsync(i => i.Id == productAndAge.Id);
            
            re.State=productAndAge.State;
            _context.Entry(re).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Return(true, "تم الحفظ");
        }
                  
        public async Task<ResObj> SetSalactAllProductAndAge(ProductAndAge productAndAge)
        {
            List<ProductAndAge> re = await _context.ProductAndAge.Where(i => i.ProductsId == productAndAge.ProductsId).ToListAsync();
            foreach (var item in re)
            {
                if (item.AgeId != 1)
                {
                    item.State = productAndAge.State;
                    _context.Entry(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Result.Return(true, "تم الحفظ");
        }

        public async Task<ResObj> GetProductAndAge(int Id)
        {
            List<ProductAndAge> data = await _prodService.GetEntityListAsync("dbo.GetProductAndAge", new { Id});
            return Result.Return(true, data);
        }
    }
}
