using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Quest.Domain.Entities;

namespace Quest.Application.Abstracts.Repositories;

public interface IPlayerRepository
{
    Task<Player> GetPlayerWithQuestsAsync(Guid playerId);
    Task<IEnumerable<Quests>> GetAvailableQuestsAsync(Player player);
    Task AcceptQuestAsync(Player player, Quests quest);
    Task UpdateQuestProgressAsync(Player player, Quests quest, IEnumerable<QuestProgress> progress);
    Task CompleteQuestAsync(Player player, Quests quest);
    Task AddExperienceAsync(Player player, int experiencePoints);
    Task AddCurrencyAsync(Player player, int currency);
    Task AddItemsAsync(Player player, IEnumerable<RewardItem> items);
}
