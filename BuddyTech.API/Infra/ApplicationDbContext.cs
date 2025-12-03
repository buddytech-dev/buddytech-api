using Microsoft.EntityFrameworkCore;
using BuddyTech.API.Models;

namespace BuddyTech.API.Infra
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<LeadInteraction> LeadInteractions { get; set; }
        public DbSet<LeadScore> LeadScores { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SellerMission> SellerMissions { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
