using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class PlayerQuest
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public virtual Player Player { get; set; }

    public Guid QuestId { get; set; }
    public virtual Quests Quest { get; set; }

    public QuestStatus Status { get; set; }
    public int Progress { get; set; }
}
