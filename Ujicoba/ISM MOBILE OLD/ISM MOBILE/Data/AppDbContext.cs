using System.Data.Entity;
using ISM_MOBILE.Models;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace ISM_MOBILE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppConnectionDB"){ }

        public virtual DbSet<tr_yarn_stock> YarnStocks { get; set; }
        public virtual DbSet<tr_grey_stock> GreyStocks { get; set; }

        public virtual DbSet<tr_fg_stock> FGStocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tr_yarn_stock>().ToTable("tr_yarn_stock");
            modelBuilder.Entity<tr_grey_stock>().ToTable("tr_grey_stock");
            modelBuilder.Entity<tr_fg_stock>().ToTable("tr_fg_stock");
        }
    }
}