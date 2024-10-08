using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class Quests
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int RequiredProgress { get; set; }
    public ICollection<QuestRequirement> QuestRequirements { get; set; } = new List<QuestRequirement>();
    public ICollection<QuestReward> QuestRewards { get; set; } = new List<QuestReward>();
    public ICollection<QuestCondition> Conditions { get; set; } = new List<QuestCondition>();
    public virtual ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();
}
