using WebSmartKid.Model.EntityMap;  
using Microsoft.EntityFrameworkCore;  
using Entity.Entity;

namespace WebSmartKid.Model
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected DBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                                                      
            modelBuilder.ApplyConfiguration(new UsersMap());       
            modelBuilder.ApplyConfiguration(new NotificationMap());     
            modelBuilder.ApplyConfiguration(new CarouselMap());   
        }
                                      
        public DbSet<Users> Users { get; set; }         
        public DbSet<Notification> Notification { get; set; }   
        public DbSet<Carousel> Carousel { get; set; }        
    }
}
