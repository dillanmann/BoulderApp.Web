using BoulderApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BoulderApp.Web.Types
{
    public class BoulderAppContext : DbContext
    {
        public BoulderAppContext(DbContextOptions<BoulderAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Circuit>().HasMany(c => c.Problems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Problem>().HasMany<ProblemAttempt>().WithOne(pa => pa.ProblemAttempted).OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Circuit> Circuits { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemAttempt> ProblemAttempts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Center> Centers { get; set; }
    }
}
