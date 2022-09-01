using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSmartKid.Helper.Interface
{
    public interface IDapperRepository<TEntity>
    {
        void RunScript(string Query);
        Task RunScriptAsync(string Query);
        void RunSp(string spName, object pars);
        Task RunSpAsync(string spName, object pars);
        TEntity GetEntity(string spName, object pars);
        Task<TEntity> GetEntityAsync(string spName, object pars);
        List<TEntity> GetEntityList(string spName, object pars);
        Task<List<TEntity>> GetEntityListAsync(string spName, object pars);
    }
}
