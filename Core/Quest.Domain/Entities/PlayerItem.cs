﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Domain.Entities;

public class PlayerItem
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public Player Player { get; set; }

    public Guid ItemId { get; set; }
    public RewardItem Item { get; set; } 

    public int Quantity { get; set; } 
}
