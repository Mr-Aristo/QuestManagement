using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class QuestCondition
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int RequiredValue { get; set; }
    public Guid QuestId { get; set; }
    public virtual Quests Quest { get; set; }
    public ICollection<QuestProgress> QuestProgresses { get; set; } = new List<QuestProgress>();
}
