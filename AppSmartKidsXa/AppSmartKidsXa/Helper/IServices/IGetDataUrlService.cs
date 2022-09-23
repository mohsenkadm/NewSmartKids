using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppSmartKidsXa.Helper.IServices
{
    public interface IGetDataUrlService<TEntity>
    {
        Task<ResponseCollection<TEntity>> PostToGetCollectionAsync(string url, TEntity entity);
        Task<Response<TEntity>> PostAsync(string url, TEntity entity);
        Task<ResponseList<TEntity>> GetListAllAsync(string url);
        Task<ResponseCollection<TEntity>> GetCollectionAllAsync(string url);
        Task<Response<TEntity>> GetAsync(string url);
        Task<Response<TEntity>> DeleteAsync(string url, string para);
    }
}
