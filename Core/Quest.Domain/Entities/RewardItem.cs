using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class RewardItem
{
    public Guid Id { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }  

    //public Guid Id { get; set; }
    //public string ItemName { get; set; }
    //public Guid PlayerId { get; set; }
    //public Player Player { get; set; }
    //public Guid ItemId { get; set; }
    //public int Quantity { get; set; } 
}
