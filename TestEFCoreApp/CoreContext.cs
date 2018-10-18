using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TestEFCoreApp
{
   public class CoreContext : DbContext
    {
        public DbSet<Object> Objects { get; set; }
        public DbSet<ObjectItem> ObjectItems { get; set; }
        public DbSet<Container> Container { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=testApp;Trusted_Connection=True;");
            optionsBuilder.ConfigureWarnings(b =>
                                             {
                                                 b.Default(WarningBehavior.Throw);
                                                 b.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.SensitiveDataLoggingEnabledWarning);
                                             });
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(new Microsoft.Extensions.Logging.LoggerFactory(new[] {new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()}));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Container>(entity =>
                {
                    entity.HasKey(e => e.ContainerId);
                    entity.Property(e => e.ContainerId).ValueGeneratedOnAdd();
                });

            modelBuilder.Entity<Object>(entity =>
                {
                    entity.HasKey(e => new { e.ContainerId, e.ObjectId});
                    entity.Property(e => e.ObjectId).ValueGeneratedOnAdd();

                    entity.HasOne(e => e.Container)
                          .WithMany(e => e.Objects)
                          .HasForeignKey(e => e.ContainerId);
                    entity.HasMany(e => e.ChildrenObjectItems)
                          .WithOne(e => e.ParentObject)
                          .HasForeignKey(e => new { e.ContainerId, e.ParentObjectId})
                          .OnDelete(DeleteBehavior.Restrict);
                    entity.HasMany(e => e.ParentObjectItems)
                          .WithOne(e => e.ChildObject)
                          .HasForeignKey(e => new { e.ContainerId, e.ChildObjectId})
                          .OnDelete(DeleteBehavior.Restrict);
                });
            modelBuilder.Entity<ObjectItem>(entity =>
                {
                    entity.HasKey(e => new { e.ContainerId, e.ObjectItemId});
                    entity.Property(e => e.ObjectItemId).ValueGeneratedOnAdd();

                    entity.HasOne(e => e.Container)
                          .WithMany(e => e.ObjectItems)
                          .HasForeignKey(e => e.ContainerId);
                });
        }
    }
}
