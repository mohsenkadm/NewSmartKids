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
    public class CountriesService : ICountriesService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;                             

        public CountriesService(
            DB_Context context )
        {
            _context = context;                   
        }

        public async Task<ResObj> GetAll()
        {
            List<Countries> data = await _context.Countries.ToListAsync() ;
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(Countries Countries)
        {      
            await _context.Countries.AddAsync(Countries);
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",Countries);
        }

        public async Task<ResObj> Update(Countries Countries)
        {                                                                       
            Countries Countries1 = await GetCountriesById(Countries.CountryId);
            if (Countries1 is null)
                return Result.Return(false, "حدث خطا اثناء عملية جلب البيانات");
             
            Countries1.CountryName = Countries.CountryName;      
            _context.Entry(Countries1).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",Countries1);
        }



        public async Task<ResObj> Delete(int Id)
        {
            Countries Countries1 = await GetCountriesById(Id);
            _context.Entry(Countries1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<Countries> GetCountriesById(int Id)
        {
            return await _context.Countries.Where(i => i.CountryId == Id).FirstOrDefaultAsync();
        }

        public async Task<ResObj> GetById(int Id)
        {
            Countries Countries = await GetCountriesById(Id);
            return Result.Return(true, Countries);
        }
    }
}
