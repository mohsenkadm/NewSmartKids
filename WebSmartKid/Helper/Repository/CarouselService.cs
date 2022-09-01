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
    public class CarouselService : ICarouselService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DBContext _context;
        private readonly IDapperRepository<Carousel> _CarouselRepository;

        public CarouselService(
            DBContext context,
            IDapperRepository<Carousel> userRepository)
        {
            _context = context;
            _CarouselRepository = userRepository;
        }

        public async Task<ResObj> GetAll()
        {
            List<Carousel> data = await _CarouselRepository.GetEntityListAsync("dbo.GetCarouselAll", new { });
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(Carousel Carousel)
        {
            if (Carousel.Image.IsEmpty())
                return Result.Return(false, "رجاءا اكتب التفاصيل بالغة العربية");
             
            await _context.Carousel.AddAsync(Carousel);
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",Carousel);
        }

        public async Task<ResObj> Update(Carousel Carousel)
        {
            if (Carousel.Image.IsEmpty())
                return Result.Return(false, "رجاءا اكتب التفاصيل بالغة العربية");
            Carousel Carousel1 = await GetCarouselById(Carousel.CarouseId);
            if (Carousel1 is null)
                return Result.Return(false, "حدث خطا اثناء عملية جلب البيانات");
             
            Carousel1.Image = Carousel.Image;
            Carousel1.IsShow = Carousel.IsShow;
            _context.Entry(Carousel1).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",Carousel1);
        }



        public async Task<ResObj> Delete(int Id)
        {
            Carousel Carousel1 = await GetCarouselById(Id);
            _context.Entry(Carousel1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<Carousel> GetCarouselById(int Id)
        {
            return await _CarouselRepository.GetEntityAsync("dbo.GetCarouselById",
                new { Id });
        }

        public async Task<ResObj> GetById(int Id)
        {
            Carousel Carousel = await GetCarouselById(Id);
            return Result.Return(true, Carousel);
        }
    }
}
