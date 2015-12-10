using RestHomes.Domain.Entities;
using System.Data.Entity;

namespace RestHomes.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("name=RosterContext")
        {
        }

        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        //public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TimeSheet> TimeSheets { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasMany(e => e.TimeSheets)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Shifts)
                .WithMany(e => e.Positions)
                .Map(m => m.ToTable("SP").MapLeftKey("IDp").MapRightKey("IDs"));

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Workers)
                .WithMany(e => e.Positions)
                .Map(m => m.ToTable("WP").MapLeftKey("IDp").MapRightKey("IDw"));

            modelBuilder.Entity<Shift>()
                .HasMany(e => e.TimeSheets)
                .WithRequired(e => e.Shift)
                .WillCascadeOnDelete(false);

            /*modelBuilder.Entity<Worker>()
                .Property(e => e.isDeleted)
                .IsFixedLength();
            */
            modelBuilder.Entity<Worker>()
                .HasMany(e => e.TimeSheets)
                .WithRequired(e => e.Worker)
                .WillCascadeOnDelete(false);
        }
    }
}
