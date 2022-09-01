using WebSmartKid.Classes;       
using System.Threading.Tasks;
using Entity.Entity;

namespace WebSmartKid.Helper.Interface
{
    public interface ICarouselService
    {
        Task<ResObj> GetAll();
        Task<ResObj> Post(Carousel Carousel); 
        Task<ResObj> Update(Carousel Carousel);
        Task<ResObj> Delete(int Id);
        Task<Carousel> GetCarouselById(int Id);
        Task<ResObj> GetById(int Id);
    }
}
