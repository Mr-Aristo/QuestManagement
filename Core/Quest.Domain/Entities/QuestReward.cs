using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class QuestReward
{
    public Guid Id { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
}
