using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quest.Application.Abstracts.Repositories;
using Quest.Domain.Entities;
using Quest.Persistance.Context;

namespace Quest.Persistance.Concretes.Repositories;
public class PlayerRepository : IPlayerRepository
{
    private readonly QuestContext _context;
    private readonly ILogger<PlayerRepository> _logger;
    private readonly IValidator<Player> _playerValidator;
    private readonly IValidator<Quests> _questValidator;
    public PlayerRepository(QuestContext context, ILogger<PlayerRepository> logger, IValidator<Player> playerValidator, IValidator<Quests> questValidator)
    {
        _context = context;
        _logger = logger;
        _playerValidator = playerValidator;
        _questValidator = questValidator;
    }
    public PlayerRepository(QuestContext context) => _context = context;


    public async Task<Player?> GetPlayerWithQuestsAsync(Guid playerId)
    {
        try
        {
            var player = await _context.Players
         .Include(p => p.PlayerQuests)
             .ThenInclude(pq => pq.Quest)
         .Include(p => p.PlayerItems)
         .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player == null)
                throw new InvalidOperationException($"Player с ID {playerId} не найдено.");

            return player;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при извлечении  player с ID {PlayerId}.", playerId);
            throw new InvalidOperationException("При извлечении плеера произошла ошибка.", ex);
        }
    }

    public async Task<IEnumerable<Quests>> GetAvailableQuestsAsync(Player player)
    {
        try
        {
            var completedQuestIds = player.PlayerQuests
                .Where(pq => pq.Status == QuestStatus.Finished)
                .Select(pq => pq.QuestId);

            return await _context.Quests
                .Where(q => !completedQuestIds.Contains(q.Id))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выборе доступных quests для player {PlayerId}", player.Id);
            throw;
        }
    }

    public async Task AcceptQuestAsync(Player player, Quests quest)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));
        if (quest == null) throw new ArgumentNullException(nameof(quest));

        // Validate player and quest
        //var playerValidationResult = await _playerValidator.ValidateAsync(player);
        //if (!playerValidationResult.IsValid)
        //{
        //    throw new ValidationException(playerValidationResult.Errors);
        //}

        //var questValidationResult = await _questValidator.ValidateAsync(quest);
        //if (!questValidationResult.IsValid)
        //{
        //    throw new ValidationException(questValidationResult.Errors);
        //}

        try
        {
            var playerQuest = new PlayerQuest
            {
                PlayerId = player.Id,
                QuestId = quest.Id,
                Status = QuestStatus.Accepted,
                Progress = new List<QuestProgress>()
            };

            await _context.PlayerQuests.AddAsync(playerQuest);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при принятии quest {QuestId} для player {PlayerId}", quest.Id, player.Id);
            throw;
        }
    } 
    public async Task CompleteQuestAsync(Player player, Quests quest)
    {
        // Validate player and quest
        var playerValidationResult = await _playerValidator.ValidateAsync(player);
        if (!playerValidationResult.IsValid)
        {
            throw new ValidationException(playerValidationResult.Errors);
        }

        var questValidationResult = await _questValidator.ValidateAsync(quest);
        if (!questValidationResult.IsValid)
        {
            throw new ValidationException(questValidationResult.Errors);
        }

        try
        {
            var playerQuest = await _context.PlayerQuests
            .FirstOrDefaultAsync(pq => pq.PlayerId == player.Id && pq.QuestId == quest.Id && pq.Status == QuestStatus.Completed);

            if (playerQuest == null)
                throw new InvalidOperationException("Quest не найдено или не завершено.");


            playerQuest.Status = QuestStatus.Finished;

            
            var rewards = await _context.QuestRewards
                .Include(r => r.RewardItems)
                .FirstOrDefaultAsync(r => r.QuestId == quest.Id);

            if (rewards != null)
            {
                await AddExperienceAsync(player, rewards.ExperiencePoints);
                await AddCurrencyAsync(player, rewards.Currency);
                await AddItemsAsync(player, rewards.RewardItems);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при завершении quest {QuestId} для player {PlayerId}", quest.Id, player.Id);
            throw;
        }
    }

    public async Task AddExperienceAsync(Player player, int experiencePoints)
    {
        try
        {
            player.ExperiencePoints += experiencePoints;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении experience points to player {PlayerId}", player.Id);
            throw new InvalidOperationException("Не удалось добавить experience points.", ex);
        }
    }

    public async Task AddCurrencyAsync(Player player, int currency)
    {
        try
        {
            player.Currency += currency;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Ошибка при добавлении currency to player {PlayerId}", player.Id);
            throw new InvalidOperationException("Не удалоси добавить currency.", ex);
        }
    }

    public async Task AddItemsAsync(Player player, IEnumerable<RewardItem> items)
    {
        try
        {
            foreach (var item in items)
            {
                var playerItem = player.PlayerItems.FirstOrDefault(pi => pi.Id == item.Id);
                if (playerItem != null)
                {
                    
                    playerItem.Quantity += item.Quantity;
                }
                else
                {
               
                    player.PlayerItems.Add(new PlayerItem
                    {
                        PlayerId = player.Id,
                        Id = item.Id,
                        Quantity = item.Quantity
                    });
                }
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Ошибка при добавлении items to player {PlayerId}", player.Id);
            throw new InvalidOperationException("Не удалось добавить items to player's inventory.", ex);
        }
    }

    public async Task UpdateQuestProgressAsync(Player player, Quests quest, IEnumerable<QuestProgress> progress)
    {
        // Validate player and quest
        var playerValidationResult = await _playerValidator.ValidateAsync(player);
        if (!playerValidationResult.IsValid)
        {
            throw new ValidationException(playerValidationResult.Errors);
        }

        var questValidationResult = await _questValidator.ValidateAsync(quest);
        if (!questValidationResult.IsValid)
        {
            throw new ValidationException(questValidationResult.Errors);
        }

        try
        {
            var playerQuest = await _context.PlayerQuests
                .Include(pq => pq.Progress)
                .FirstOrDefaultAsync(pq => pq.PlayerId == player.Id && pq.QuestId == quest.Id);

            if (playerQuest == null)
                throw new InvalidOperationException("Player quest не найдено.");

            foreach (var progressUpdate in progress)
            {
                var playerProgress = playerQuest.Progress
                    .FirstOrDefault(p => p.ConditionId == progressUpdate.ConditionId);

                if (playerProgress == null)
                {
                    var condition = quest.Conditions.FirstOrDefault(c => c.Id == progressUpdate.ConditionId);
                    if (condition == null)
                        throw new InvalidOperationException("Недопустимое условие.");

                    playerProgress = new QuestProgress
                    {
                        PlayerQuestId = playerQuest.PlayerId,
                        ConditionId = condition.Id,
                        TargetValue = condition.RequiredValue,
                        CurrentValue = 0
                    };

                    playerQuest.Progress.Add(playerProgress);
                }

                if (progressUpdate.CurrentValue < playerProgress.CurrentValue)
                    throw new InvalidOperationException("Progress значение не может уменьшиться");

                if (progressUpdate.CurrentValue > playerProgress.TargetValue)
                    throw new InvalidOperationException("Progress значение превышает требуемое значение.");

                playerProgress.CurrentValue = progressUpdate.CurrentValue;
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Обновление ошибок quest progress.");
            throw;
        }
    }   

}
