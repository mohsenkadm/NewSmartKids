using WebSmartKid.Classes;       
using System.Threading.Tasks;
using Entity.Entity;
using Entity.Entity;

namespace WebSmartKid.Helper.Interface
{
    public interface IProductsService
    {
        Task<ResObj> GetAll(string? Name, int? CategoriesId,int index,bool? ConatinImage, string? SortOrder);
        Task<ResObj> Post(Products Products); 
        Task<ResObj> Update(Products Products);
        Task<ResObj> Delete(int Id);
        Task<Products> GetProductsById(int Id);
        Task<ResObj> GetById(int Id);
        Task<ResObj> DeleteImage(int id);
        Task<ResObj> DeleteImageForPost(int id);
        Task<ResObj> PostImages(Images images);  
        Task<ResObj> GetImagesByProductsId(int Id);
        Task<ResObj> GetProdForApp(Products productFilter);

        Task<ResObj> RemoveLike(int ProductsId, int userId);
        Task<ResObj> PostLike(Like like);
        Task<ResObj> ReportQtyGetAll(string? name, int? categoriesId, int index);
    }
}
