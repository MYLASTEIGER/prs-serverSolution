using Microsoft.EntityFrameworkCore;
using prs_server.Models;

namespace prs_server.Models {
    public class AppDbContext : DbContext {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestLine> RequestLines { get; set; }

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {

            builder.Entity<User>(e => {
                e.HasIndex(p => p.Username).IsUnique(true);
            });
            builder.Entity<Vendor>(e => {
                e.HasIndex(p => p.Code).IsUnique(true);
            });
            builder.Entity<Product>(e => {
                e.HasIndex(p => p.PartNbr).IsUnique(true);
            });

        }

        public DbSet<prs_server.Models.Request> Request { get; set; }
    }
}