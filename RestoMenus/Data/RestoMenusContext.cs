using Microsoft.EntityFrameworkCore;
using RestoMenus.Entities;

namespace RestoMenus.Data
{
    public class RestoMenusContext : DbContext
    {
        public RestoMenusContext(DbContextOptions<RestoMenusContext> options) : base(options) 
        { 
        
        }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuImage> MenuImages { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BannerImage> BannerImages { get; set; }
        public DbSet<Banner> Banner { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
