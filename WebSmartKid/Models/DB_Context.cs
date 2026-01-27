using WebSmartKid.Model.EntityMap;  
using Microsoft.EntityFrameworkCore;  
using Entity.Entity;
using WebSmartKid.Models.EntityMap;

namespace WebSmartKid.Model
{
    public class DB_Context : DbContext
    {
        public DB_Context(DbContextOptions<DB_Context> options) : base(options)
        {

        }

        protected DB_Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                                                      
            modelBuilder.ApplyConfiguration(new UsersMap());       
            modelBuilder.ApplyConfiguration(new NotificationMap());     
            modelBuilder.ApplyConfiguration(new CarouselMap());   
            modelBuilder.ApplyConfiguration(new CategoriesMap());   
            modelBuilder.ApplyConfiguration(new CountriesMap());   
            modelBuilder.ApplyConfiguration(new ImagesMap());   
            modelBuilder.ApplyConfiguration(new MessagesMap());   
            modelBuilder.ApplyConfiguration(new OrderDetailMap());   
            modelBuilder.ApplyConfiguration(new OrdersMap());   
            modelBuilder.ApplyConfiguration(new ProductsMap());   
            modelBuilder.ApplyConfiguration(new TblAgesMap());   
            modelBuilder.ApplyConfiguration(new AdminMap());   
            modelBuilder.ApplyConfiguration(new PostsMap());   
            modelBuilder.ApplyConfiguration(new PermissionMap());   
            modelBuilder.ApplyConfiguration(new TypeDiscountMap());   
            modelBuilder.ApplyConfiguration(new ProductAndAgeMap());   
            modelBuilder.ApplyConfiguration(new PromoCodeMap());
            modelBuilder.ApplyConfiguration(new UserPromoCodeMap());
        }
                                      
        public DbSet<Users> Users { get; set; }         
        public DbSet<Notification> Notification { get; set; }   
        public DbSet<Carousel> Carousel { get; set; }        
        public DbSet<Categories> Categories { get; set; }        
        public DbSet<Countries> Countries { get; set; }        
        public DbSet<Images> Images { get; set; }        
        public DbSet<Messages> Messages { get; set; }        
        public DbSet<OrderDetail> OrderDetail { get; set; }        
        public DbSet<Orders> Orders { get; set; }        
        public DbSet<Products> Products { get; set; }        
        public DbSet<TblAges> TblAges { get; set; }        
        public DbSet<Admin> Admin { get; set; }        
        public DbSet<Posts> Posts { get; set; }        
        public DbSet<Permission> Permission { get; set; }        
        public DbSet<TypeDiscount> TypeDiscount { get; set; }        
        public DbSet<ProductAndAge> ProductAndAge { get; set; }
        public DbSet<PromoCode> PromoCode { get; set; }
        public DbSet<UserPromoCode> UserPromoCode { get; set; }        
    }
}
