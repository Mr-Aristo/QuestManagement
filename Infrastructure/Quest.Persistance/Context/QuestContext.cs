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
    public QuestContext(DbContextOptions<QuestContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerQuest>()
            .HasKey(pq => pq.Id);

        modelBuilder.Entity<QuestReward>()
            .HasKey(qr => qr.Id);

        modelBuilder.Entity<QuestRequirement>()
            .HasKey(qr => qr.Id);
    }
}
