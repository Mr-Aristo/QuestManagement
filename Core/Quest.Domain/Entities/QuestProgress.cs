using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class QuestProgress
{
    public Guid Id { get; set; }
    public Guid PlayerQuestId { get; set; }
    public virtual PlayerQuest PlayerQuest { get; set; }
    public Guid ConditionId { get; set; }
    public virtual QuestCondition Condition { get; set; }
    public int CurrentValue { get; set; }
    public int TargetValue { get; set; }
}
