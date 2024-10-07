using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Domain.Entities;

namespace Quest.Application.Abstracts.Repositories;

public interface IQuestRepository
{
    Task<IEnumerable<Quests>> GetAvailableQuestsAsync(Player player);
    Task<Quests> GetQuestByIdAsync(Guid questId);
}
