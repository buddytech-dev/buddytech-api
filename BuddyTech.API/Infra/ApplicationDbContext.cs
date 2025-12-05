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

            modelBuilder.Entity<LeadScore>()
                .HasOne(s => s.Lead)
                .WithMany(l => l.ScoreHistory)
                .HasForeignKey(s => s.LeadId)
                .IsRequired(); 

            modelBuilder.Entity<Lead>()
                .HasOne(l => l.CurrentScore)
                .WithOne() 
                .HasForeignKey<Lead>(l => l.CurrentScoreId) 
                .IsRequired(false);

            modelBuilder.Entity<Lead>()
                .HasOne(l => l.Suggestion)
                .WithOne(s => s.Lead) 
                .HasForeignKey<Suggestion>(s => s.LeadId) 
                .IsRequired(false);

            modelBuilder.Entity<LeadInteraction>()
                .HasOne(i => i.Lead)
                .WithMany(l => l.Interactions)
                .HasForeignKey(i => i.LeadId)
                .IsRequired();

            modelBuilder.Entity<SellerMission>()
                .HasKey(sm => new { sm.SellerId, sm.MissionId });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}