﻿using System;
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
    public int ExperiencePoints { get; set; } // Reward: Experience points
    public int Currency { get; set; } // Reward: Currency
    public ICollection<RewardItem> Items { get; set; } // Reward: Items given to the player
}
