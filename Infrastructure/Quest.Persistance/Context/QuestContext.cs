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

    public QuestContext(DbContextOptions<QuestContext> options) : base(options) { }


    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerItem> PlayerItems { get; set; }
    public DbSet<PlayerQuest> PlayerQuests { get; set; }
    public DbSet<QuestCondition> QuestConditions { get; set; }
    public DbSet<QuestProgress> QuestProgresses { get; set; }
    public DbSet<QuestRequirement> QuestRequirements { get; set; }
    public DbSet<QuestReward> QuestRewards { get; set; }
    public DbSet<Quests> Quests { get; set; }
    public DbSet<RewardItem> RewardItems { get; set; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
