using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using School_Management_System.Models.UserGroupX;
using School_Management_System.Models.UserX;
using School_Management_System.Models.ContactInfoX;
using School_Management_System.Models.ClassX;
using School_Management_System.Models.StreamX;
using School_Management_System.Models.StudentX;
using School_Management_System.Models.TermX;
using School_Management_System.Models.StudentFeesStructureX;
using School_Management_System.Models.TransactionX;


namespace School_Management_System.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactInfo> Contacts { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StreamX.Stream> Streams { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<StudentFeesStructure> StudentFeesStructures { get; set; }
        public DbSet<Transaction> Transactions { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<User>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<ContactInfo>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Class>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<StreamX.Stream>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Student>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Term>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<StudentFeesStructure>().HasQueryFilter(c => !c.DeletedAt.HasValue);
            modelBuilder.Entity<Transaction>().HasQueryFilter(c => !c.DeletedAt.HasValue);

            base.OnModelCreating(modelBuilder);


        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is UserGroup
                    || e.Entity is User
                    || e.Entity is ContactInfo
                    || e.Entity is Class
                    || e.Entity is StreamX.Stream
                    || e.Entity is Student
                    || e.Entity is Term
                    || e.Entity is StudentFeesStructure
                    || e.Entity is Transaction
                    )
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("AddedAt").CurrentValue = DateTime.UtcNow;
                }
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        public void SoftDelete<T>(T entity) where T : class
        {
            var entry = Entry(entity);
            entry.Property("DeletedAt").CurrentValue = DateTime.UtcNow;
            entry.State = EntityState.Modified;
        }
    }
}
