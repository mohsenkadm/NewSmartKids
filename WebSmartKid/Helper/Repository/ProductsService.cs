using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;          
using WebSmartKid.Model.General;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Entity;
using System.Xml.Linq;               

namespace WebSmartKid.Helper.Repository
{
    public class ProductsService : IProductsService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;
        private readonly IDapperRepository<Products> _prodService;

        public ProductsService(
            DB_Context context, IDapperRepository<Products> prodService)
        {
            _context = context;
            _prodService = prodService;
        }

        public async Task<ResObj> GetAll(string? Name,int? CategoriesId,int index)
        {
            List<Products> data = await _prodService.GetEntityListAsync("dbo.GetProdAll", new { Name, CategoriesId , index });
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(Products Products)
        {                        
            await _context.Products.AddAsync(Products);
            await _context.SaveChangesAsync();
            var ages = await _context.TblAges.ToListAsync();
            if (ages != null)
            {
                List<ProductAndAge> productAndAges = new();
                foreach(var item in ages)
                {    
                    if(item.AgeId==1) 
                    productAndAges.Add(new() { ProductsId = Products.ProductsId, AgeId=item.AgeId , State=true }); 
                    else
                    productAndAges.Add(new() { ProductsId = Products.ProductsId, AgeId=item.AgeId , State=false });  
                }
                await _context.ProductAndAge.AddRangeAsync(productAndAges);
                await _context.SaveChangesAsync();
            }
            return Result.Return(true, "تم الحفظ بنجاح",Products);
        }

        public async Task<ResObj> Update(Products Products)
        {                                                                       
            Products Products1 = await GetProductsById(Products.ProductsId);
            if (Products1 is null)
                return Result.Return(false, "حدث خطا اثناء عملية جلب البيانات");
             
            Products1.Name = Products.Name;
            Products1.Detail = Products.Detail;
            Products1.Price = Products.Price;
            Products1.CategoriesId = Products.CategoriesId;  
            Products1.DiscountPercentage = Products.DiscountPercentage;
            Products1.IsDiscount = Products.IsDiscount;   

            _context.Entry(Products1).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // set prod and age
            await _prodService.RunScriptAsync("delete from ProductAndAge where ProductsId="+Products.ProductsId);
            var ages = await _context.TblAges.ToListAsync();
            if (ages != null)
            {
                List<ProductAndAge> productAndAges = new();
                foreach (var item in ages)
                {
                    if (item.AgeId == 1)
                        productAndAges.Add(new() { ProductsId = Products.ProductsId, AgeId = item.AgeId, State = true });
                    else
                        productAndAges.Add(new() { ProductsId = Products.ProductsId, AgeId = item.AgeId, State = false });
                }
                await _context.ProductAndAge.AddRangeAsync(productAndAges);
                await _context.SaveChangesAsync();
            }
            return Result.Return(true, "تم الحفظ بنجاح",Products1);
        }



        public async Task<ResObj> Delete(int Id)
        {
            Products Products1 = await GetProductsById(Id);
            _context.Entry(Products1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<Products> GetProductsById(int Id)
        {
            return await _context.Products.Where(i => i.ProductsId == Id).FirstOrDefaultAsync();
        }

        public async Task<ResObj> GetById(int Id)
        {
            Products Products = await GetProductsById(Id);
            return Result.Return(true, Products);
        }

        public async Task<ResObj> DeleteImage(int id)
        {
            await _prodService.RunScriptAsync("delete from Images where ImageId=" + id);
            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<ResObj> DeleteImageForPost(int id)
        {
            await _prodService.RunScriptAsync("delete from Images where ProductsId=" + id);
            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<ResObj> PostImages(Images images)
        {
            await _prodService.RunScriptAsync(" insert into Images (ImagePath,ProductsId)Values('" + images.ImagePath + "'," + images.ProductsId + ")");

            return Result.Return(true, "تم الحفظ بنجاح");
        }

        public async Task<ResObj> GetImagesByProductsId(int Id)
        {
            List<Images> img=await _context.Images.Where(i=>i.ProductsId==Id).ToListAsync();
            return Result.Return(true, img);
        }

        public async Task<ResObj> GetProdForApp(Products productFilter)
        {
            string? Name = productFilter.Name;
            int? CategoriesId = productFilter.CategoriesId;
            string wherecode = " and 1=1 ";
            if(productFilter.AgeFilter!=null)
            {
                wherecode += " and ( ";
                foreach(var item in productFilter.AgeFilter)
                {
                    wherecode += $" AgeId={item.Id} or";
                }
                wherecode += " 1=1)";
            }
            List<Products> data = await _prodService.GetEntityListAsync("dbo.GetProdForApp", new { Name, CategoriesId , wherecode });
            return Result.Return(true, data);
        }
        public async Task<ResObj> RemoveLike(int ProductsId, int userId)
        {
            await _prodService.RunScriptAsync("delete from [Like] where ProductsId=" + ProductsId + " and UserId=" + userId +
                " update Products set LikeCount=isnull(LikeCount,0)-1 where ProductsId=" + ProductsId);
            return Result.Return(true);
        }

        public async Task<ResObj> PostLike(Like like)
        {
            await _prodService.RunScriptAsync("INSERT INTO [Like] ([ProductsId],[UserId]) VALUES ("
                + like.ProductsId + "," + like.UserId+")" +
                " update Products set LikeCount=isnull(LikeCount,0)+1 where ProductsId=" + like.ProductsId);
            return Result.Return(true);
        }
    }
}
