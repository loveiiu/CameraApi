using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Core;
using WebApi.Entities;

namespace WebApi.DbContexts
{
    public class CommonDbContext : DbContext
    {
        private static ILoggerFactory loggerFactory;
        static CommonDbContext()
        {
            loggerFactory = new EntityFrameworkSqlLoggerFactory();
            loggerFactory.AddProvider(new EntityFrameworkSqlLoggerProvider());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = Ioc.GetConfig().ConnectionString;
            optionsBuilder.UseSqlServer(connStr);

            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SectionSpace>().HasKey(x => x.SectionSpaceId);
        }

        public DbSet<SectionSpace> SectionSpaceSet { get; set; }
        public DbSet<CameraRaw> CameraRawSet { get; set; }
        public DbSet<CameraDailyReport> CameraDailyReportSet { get; set; }
        public DbSet<CameraName> CameraNameSet { get; set; }
        public DbSet<ApiLog> ApiLogSet { get; set; }
    }
}
