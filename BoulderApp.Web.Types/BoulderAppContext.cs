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

        public DbSet<Circuit> Circuits { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemAttempt> ProblemAttempts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Center> Centers { get; set; }
    }
}
