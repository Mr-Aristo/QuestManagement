using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class QuestRequirement
{
    public Guid Id { get; set; }
    public string RequirementType { get; set; }
    public int RequiredAmount { get; set; }

    public Guid QuestId { get; set; }
    public virtual Quests Quest { get; set; }
}
