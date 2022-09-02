//using Com.OneSignal;
using AppSmartKid.Helper.IServices;   
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;   
using Entity.Entity;

namespace AppSmartKid.Helper
{
    public class GetDataUrlService<TEntity>: IGetDataUrlService<TEntity> 
    { 
        public readonly string BaseUrl = "http://smartserveriq-001-site4.htempurl.com/";

        /// <summary>
        /// GetListAllAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ResponseList<TEntity>> GetListAllAsync(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                if (InfoAccess.Token != "" || InfoAccess.Token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", InfoAccess.Token);
                var response = await client.GetAsync(url);
                if (response.ReasonPhrase == "Unauthorized")
                {
                    await LoginAfetrAuthrize();
                }
                var json = await response.Content.ReadAsStringAsync();
                ResponseList<TEntity> res = JsonConvert.DeserializeObject<ResponseList<TEntity>>(json);
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// GetAllAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Response<TEntity>> GetAsync(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                if (InfoAccess.Token != "" && InfoAccess.Token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", InfoAccess.Token);
                var response = await client.GetAsync(url);
                if (response.ReasonPhrase == "Unauthorized")
                {
                    await LoginAfetrAuthrize();
                }
                var json = await response.Content.ReadAsStringAsync();
                Response<TEntity> res = JsonConvert.DeserializeObject<Response<TEntity>>(json);
                return res;
            }

            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// GetCollectionAllAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ResponseCollection<TEntity>> GetCollectionAllAsync(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                if (InfoAccess.Token != "" && InfoAccess.Token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", InfoAccess.Token);
                var response = await client.GetAsync(url);
                if (response.ReasonPhrase == "Unauthorized")
                {
                    await LoginAfetrAuthrize();
                }
                var json = await response.Content.ReadAsStringAsync();

                ResponseCollection<TEntity> res = JsonConvert.DeserializeObject<ResponseCollection<TEntity>>(json);
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// PostAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>

        public async Task<Response<TEntity>> PostAsync(string url, TEntity entity)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                if (InfoAccess.Token != "" && InfoAccess.Token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", InfoAccess.Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(entity);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                if (response.ReasonPhrase == "Unauthorized")
                {
                    await LoginAfetrAuthrize();
                }
                var json1 = await response.Content.ReadAsStringAsync();
                Response<TEntity> res = JsonConvert.DeserializeObject<Response<TEntity>>(json1);
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<Response<TEntity>> DeleteAsync(string url, string para)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                if (InfoAccess.Token != "" && InfoAccess.Token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", InfoAccess.Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.DeleteAsync(url + para);
                if (response.ReasonPhrase == "Unauthorized")
                {     
                        await LoginAfetrAuthrize();
                }
                var json1 = await response.Content.ReadAsStringAsync();
                Response<TEntity> res = JsonConvert.DeserializeObject<Response<TEntity>>(json1);
                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task LoginAfetrAuthrize()
        {
            try
            {
                string Phone = await  SecureStorage.GetAsync("Phone");
                string Name = await SecureStorage.GetAsync("Name");
                Users users = new Users { Name = Name, Phone = Phone };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(users);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Account/Login", data);
                var json1 = await response.Content.ReadAsStringAsync();
                Response<Users> res = JsonConvert.DeserializeObject<Response<Users>>(json1);
                Users user = res.data;
              //  try { OneSignal.Current.SetExternalUserId(user.UserId.ToString()); } catch (Exception ex) { }
                await saveprop(user);
            }
            catch (Exception)
            {

            }
        }
        private static async Task saveprop(Users users)
        {
            await SecureStorage.SetAsync("Id", users.UserId.ToString());
            await SecureStorage.SetAsync("Token", users.Token.ToString());              
            InfoAccess.Id = users.UserId;
            InfoAccess.Token = users.Token;              
        }
    }
}
