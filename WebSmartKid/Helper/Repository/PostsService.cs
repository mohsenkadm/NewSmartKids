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
    public class PostsService : IPostsService, IRegisterScopped
    {
        // cotext only apply scopped 
        private readonly DB_Context _context;                             

        public PostsService(
            DB_Context context )
        {
            _context = context;                   
        }

        public async Task<ResObj> GetAll()
        {
            List<Posts> data = await _context.Posts.ToListAsync() ;
            return Result.Return(true, data);
        }

        public async Task<ResObj> Post(Posts Posts)
        {      
            await _context.Posts.AddAsync(Posts);
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",Posts);
        }

        public async Task<ResObj> Update(Posts Posts)
        {                                                                       
            Posts Posts1 = await GetPostsById(Posts.PostId);
            if (Posts1 is null)
                return Result.Return(false, "حدث خطا اثناء عملية جلب البيانات");
             
            Posts1.Title = Posts.Title;
            Posts1.Url = Posts.Url;
            _context.Entry(Posts1).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم الحفظ بنجاح",Posts1);
        }



        public async Task<ResObj> Delete(int Id)
        {
            Posts Posts1 = await GetPostsById(Id);
            _context.Entry(Posts1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<Posts> GetPostsById(int Id)
        {
            return await _context.Posts.Where(i => i.PostId == Id).FirstOrDefaultAsync();
        }

        public async Task<ResObj> GetById(int Id)
        {
            Posts Posts = await GetPostsById(Id);
            return Result.Return(true, Posts);
        }
    }
}
