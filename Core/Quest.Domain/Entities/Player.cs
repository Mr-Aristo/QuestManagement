using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class Player
{
    Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public virtual ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>(); 
    
}
