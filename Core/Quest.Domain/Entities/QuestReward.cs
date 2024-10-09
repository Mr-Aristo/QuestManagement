using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class QuestReward
{
    public Guid Id { get; set; }
    public Guid QuestId { get; set; }
    public Quests Quest { get; set; }
    public int ExperiencePoints { get; set; }
    public int Currency { get; set; }
    public ICollection<RewardItem> RewardItems { get; set; } = new List<RewardItem>();
}