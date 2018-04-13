using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KEACanteenREST.Models
{
    public partial class db_sysint_prodContext : DbContext
    {
        public db_sysint_prodContext(DbContextOptions<db_sysint_prodContext> options) : base(options) { }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<SensorDatas> SensorDatas { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=tcp:sysint.database.windows.net,1433;Initial Catalog=db-sysint-prod;Persist Security Info=False;User ID=PbKxhmUQS;Password=g]iY47upJ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey });

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<SensorDatas>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Timestamp).IsRequired();
            });
        }
    }
}
