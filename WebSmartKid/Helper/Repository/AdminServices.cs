using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using WebSmartKid.Classes;
using WebSmartKid.Helper.Interface;
using WebSmartKid.Model;
using WebSmartKid.Model.General;

namespace WebSmartKid.Helper.Repository
{
    public class AdminServices : IAdminServices, IRegisterScopped
    {
        private readonly DB_Context _context;
        private readonly IDapperRepository<Permission> _permissionService;
        private readonly IDapperRepository<Admin> _adminService;

        public AdminServices(
            DB_Context context, IDapperRepository<Permission>  dapperRepository, IDapperRepository<Admin> adminService)
        {
            _context = context;
            _permissionService = dapperRepository;
            _adminService = adminService;
        }
        public async Task<ResObj> Login(string AdminNo, string password)
        {
            password = Encyptmethod.EncryptStringToBytes_Aes(password);

            Admin login = await _context.Admin.Where(i => i.Password == password
            && i.AdminNo == Convert.ToInt32(AdminNo)).FirstOrDefaultAsync();

            if (login is null)
                return Result.Return(false, "اسم المستخدم او كلمة المرور غير صحيحة");
                

            UserManager userManager = new UserManager() { Id = login.AdminId, Name = login.AdminName };
            login.Token = JsonWebToken.GenerateToken(userManager);

            return Result.Return(true, login);
        }
        public async Task<ResObj> GetCountForMain()
        {
            Admin admin = await _adminService.GetEntityAsync("dbo.GetCountForMain", new {  });

            return Result.Return(true, admin);
        }

        public async Task<ResObj> GetAll(string? name)
        {
            List<Admin> admin;
            if (name.IsEmpty())
                admin = await _context.Admin.ToListAsync();
            else
                admin =await _context.Admin.Where(i => i.AdminName.Contains(name)).ToListAsync();
            foreach (Admin Admin in admin)
            {
                if (Admin.Password != null)
                    if (Admin.Password.Length > 0)
                        Admin.Password = Encyptmethod.DecryptStringFromBytes_Aes(Admin.Password);
            }
            return Result.Return(true, admin);
        }

        public async Task<ResObj> Post(Admin Admin)
        {
            Admin.Password = Encyptmethod.EncryptStringToBytes_Aes(Admin.Password);
            await _context.Admin.AddAsync(Admin);
            await _context.SaveChangesAsync();
            return Result.Return(true, "تم الحفظ بنجاح");
        }

        public async Task<ResObj> Update(Admin Admin)
        {
            Admin Admin1 = await GetAdminById(Admin.AdminId);
            if (Admin1 is null)
                return Result.Return(false, "حدث خطأ اثناء عملية جلب البيانات");          
            Admin1.AdminName = Admin.AdminName;
            Admin1.AdminNo = Admin.AdminNo; 
            Admin1.Password = Encyptmethod.EncryptStringToBytes_Aes(Admin.Password);  

            _context.Entry(Admin1).State = EntityState.Modified;
            await _context.SaveChangesAsync();
                                                               
            return Result.Return(true, "تم الحفظ بنجاح");

        }

        public async Task<ResObj> Delete(int Id)
        {
            Admin Admin1 = await GetAdminById(Id); 
            _context.Entry(Admin1).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Result.Return(true, "تم حذف بنجاح");
        }

        public async Task<ResObj> GetById(int Id)
        {
            Admin Admin = await GetAdminById(Id);
            if (Admin.Password.Length > 0)
            {
                Admin.Password = Encyptmethod.DecryptStringFromBytes_Aes(Admin.Password);
                Admin.Password = Admin.Password.Replace("\0", "");
            }
            return Result.Return(true, Admin);
        }

        private async Task<Admin> GetAdminById(int Id)
        {
            return await _context.Admin.Where(i=>i.AdminId==Id).FirstOrDefaultAsync();
        }

        public async Task<ResObj> GetPermissionForLayout(int id)
        {       
            List<Permission> Permissions =  await _permissionService.GetEntityListAsync("dbo.GetPermissionForLayout", new { id }); ;
            return Result.Return(true, Permissions);
        }
        public async Task<ResObj> changestate(Permission permission)
        {
            await _permissionService.RunScriptAsync("UPDATE [dbo].[Permission]  SET  [State] ='" + permission.State + "' WHERE PermissionId=" + permission.PermissionId);
            return Result.Return(true, "تم الحفظ");
        }
        public async Task<ResObj> SavePermission(int UserId)
        {
            await _permissionService.RunSpAsync("dbo.SavePermission", new { UserId });
            return Result.Return(true);
        }
        public async Task<ResObj> GetPermissionByUserId(int UserId)
        {
            List<Permission> permissions= await _permissionService.GetEntityListAsync("dbo.GetPermissionByUserId", new { UserId });
            return Result.Return(true, permissions);
        }

        public async Task<ResObj> GetDashboardReports()
        {
            try
            {
                var dashboardData = new
                {
                    // Recent Orders
                    recentOrders = await _context.Orders
                        .OrderByDescending(o => o.OrderDate)
                        .Take(10)
                        .Select(o => new
                        {
                            o.OrderId,
                            o.OrderNo,
                            o.OrderDate,
                            o.Name,
                            o.Phone,
                            o.FinalAmount,
                            o.IsApporve,
                            o.IsDone,
                            o.IsCancel
                        })
                        .ToListAsync(),

                    // Order Status Counts
                    orderStatus = new
                    {
                        pending = await _context.Orders.CountAsync(o => !o.IsApporve && !o.IsCancel),
                        approved = await _context.Orders.CountAsync(o => o.IsApporve && !o.IsDone),
                        done = await _context.Orders.CountAsync(o => o.IsDone),
                        cancelled = await _context.Orders.CountAsync(o => o.IsCancel)
                    },

                    // Top Products
                    topProducts = await _context.Products
                        .OrderByDescending(p => p.NoOfBuyers)
                        .Take(5)
                        .Select(p => new
                        {
                            p.ProductsId,
                            p.Name,
                            p.Price,
                            p.NoOfBuyers,
                            p.CategoriesName,
                            p.Image
                        })
                        .ToListAsync(),

                    // Sales by Category
                    categorySales = await _context.Categories
                        .Select(c => new
                        {
                            c.CategoriesId,
                            c.CategoriesName,
                            productCount = _context.Products.Count(p => p.CategoriesId == c.CategoriesId),
                            totalSales = _context.Products.Where(p => p.CategoriesId == c.CategoriesId).Sum(p => (decimal?)p.NoOfBuyers * p.Price) ?? 0
                        })
                        .OrderByDescending(c => c.totalSales)
                        .ToListAsync(),

                    // Recent Users
                    recentUsers = await _context.Users
                        .OrderByDescending(u => u.UserId)
                        .Take(10)
                        .Select(u => new
                        {
                            u.UserId,
                            u.Name,
                            u.Phone,
                            u.AccountBalance
                        })
                        .ToListAsync(),

                    // Sales Trends (Last 7 days)
                    salesTrends = await _context.Orders
                        .Where(o => o.OrderDate >= DateTime.Now.AddDays(-7))
                        .GroupBy(o => o.OrderDate.Date)
                        .Select(g => new
                        {
                            date = g.Key,
                            totalSales = g.Sum(o => o.FinalAmount),
                            orderCount = g.Count()
                        })
                        .OrderBy(g => g.date)
                        .ToListAsync(),

                    // Low Stock Products
                    lowStockProducts = await _context.Products
                        .Where(p => p.Count < 10)
                        .OrderBy(p => p.Count)
                        .Take(10)
                        .Select(p => new
                        {
                            p.ProductsId,
                            p.Name,
                            p.Count,
                            p.Price
                        })
                        .ToListAsync()
                };

                return Result.Return(true, dashboardData);
            }
            catch (Exception ex)
            {
                return Result.Return(false, "حدث خطأ في جلب البيانات");
            }
        }
    }
}
