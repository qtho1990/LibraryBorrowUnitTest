using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Entity;
using Repository.Interceptor;

namespace Repository.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base()
        {

        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Book> Books{ get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
            
            optionsBuilder.AddInterceptors(new InsertInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<BookRenting>()
               .HasOne(x => x.Book)
               .WithMany(x => x.BookRentings)
               .HasForeignKey(x => x.BookId);

            modelBuilder.Entity<BookRenting>()
               .HasOne(x => x.User)
               .WithMany(x => x.BookRentings)
               .HasForeignKey(x => x.UserId);
        }
    }
}
