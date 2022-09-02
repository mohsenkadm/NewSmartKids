using WebSmartKid.Classes;       
using System.Threading.Tasks;
using Entity.Entity;

namespace WebSmartKid.Helper.Interface
{
    public interface IPostsService
    {
        Task<ResObj> GetAll();
        Task<ResObj> Post(Posts Posts); 
        Task<ResObj> Update(Posts Posts);
        Task<ResObj> Delete(int Id);
        Task<Posts> GetPostsById(int Id);
        Task<ResObj> GetById(int Id);
    }
}
