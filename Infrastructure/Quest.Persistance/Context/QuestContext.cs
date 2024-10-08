using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quest.Domain.Entities;

namespace Quest.Persistance.Context;
public class QuestContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Quests> Quests { get; set; }
    public DbSet<PlayerQuest> PlayerQuests { get; set; }
    public DbSet<QuestReward> QuestRewards { get; set; }
    public DbSet<QuestRequirement> QuestRequirements { get; set; }
    public DbSet<PlayerItem> PlayerItems { get; set; }
    public DbSet<RewardItem> RewardItems { get; set; }
    public DbSet<QuestProgress> QuestProgresses { get; set; } // Ensure this DbSet is included

    public QuestContext(DbContextOptions<QuestContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerQuest>()
            .HasKey(pq => new { pq.PlayerId, pq.QuestId });

        modelBuilder.Entity<QuestReward>()
            .HasKey(qr => qr.Id);

        modelBuilder.Entity<QuestRequirement>()
            .HasKey(qr => qr.Id);

        modelBuilder.Entity<QuestProgress>()
            .HasKey(qp => qp.Id); 

        modelBuilder.Entity<QuestProgress>()
            .HasOne(qp => qp.PlayerQuest)
            .WithMany(pq => pq.Progress)
            .HasForeignKey(qp => qp.PlayerQuestId);

        modelBuilder.Entity<QuestProgress>()
            .HasOne(qp => qp.Condition)
            .WithMany(c => c.QuestProgresses)
            .HasForeignKey(qp => qp.ConditionId);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
