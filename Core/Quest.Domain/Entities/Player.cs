using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class Player
{   
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int ExperiencePoints { get; set; }
    public int Currency { get; set; }

    public ICollection<PlayerQuest> PlayerQuests { get; set; } = new List<PlayerQuest>();
    public ICollection<PlayerItem> PlayerItems { get; set; } = new List<PlayerItem>();

}
