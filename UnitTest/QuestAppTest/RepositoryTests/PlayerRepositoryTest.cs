using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Quest.Domain.Entities;
using Quest.Persistance.Concretes.Repositories;
using Quest.Persistance.Context;
using Xunit;

namespace QuestAppTest.RepositoryTests;

public class PlayerRepositoryTest
{
    private readonly Mock<ILogger<PlayerRepository>> _loggerMock;
    private readonly QuestContext _context;
    private readonly Mock<QuestContext> _mockContext;
    private readonly PlayerRepository _repository;
    private readonly Mock<IValidator<Player>> _playerValidatorMock;
    private readonly Mock<IValidator<Quests>> _questValidatorMock;
    public PlayerRepositoryTest()
    {
        _loggerMock = new Mock<ILogger<PlayerRepository>>();
        _playerValidatorMock = new Mock<IValidator<Player>>();
        _questValidatorMock = new Mock<IValidator<Quests>>();
        _mockContext = new Mock<QuestContext>();
        var options = new DbContextOptionsBuilder<QuestContext>()
            .UseInMemoryDatabase(databaseName: "QuestDb")
            .Options;

        _context = new QuestContext(options);
        _repository = new PlayerRepository(_context, _loggerMock.Object, null, null);
    }
    private void SetupMockDbSet<T>(IEnumerable<T> data, out Mock<DbSet<T>> mockSet) where T : class
    {
        var queryableData = data.AsQueryable();

        mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
    }

    [Fact]
    public async Task GetPlayerWithQuestsAsync_PlayerExists_ReturnsPlayerWithQuests()
    {
        
        var playerId = Guid.NewGuid();
        var player = new Player
        {
            Id = playerId,
            Name = "Test Player",
            PlayerQuests = new List<PlayerQuest>
            {
                new PlayerQuest { Quests = new Quests { Title = "Quest 1" } },
                new PlayerQuest { Quests = new Quests { Title = "Quest 2" } }
            }
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        
        var result = await _repository.GetPlayerWithQuestsAsync(playerId);

       
        Assert.NotNull(result);
        Assert.Equal(playerId, result.Id);
        Assert.Equal(2, result.PlayerQuests.Count);
    }

    [Fact]
    public async Task AcceptQuestAsync_ValidPlayerAndQuest_AddsPlayerQuest()
    {
        
        var player = new Player { Id = Guid.NewGuid(), Name = "Test Player" };
        var quest = new Quests { Id = Guid.NewGuid(), Title = "Test Quest" };

        _playerValidatorMock.Setup(v => v.ValidateAsync(player, default))
            .ReturnsAsync(new ValidationResult());

        _questValidatorMock.Setup(v => v.ValidateAsync(quest, default))
            .ReturnsAsync(new ValidationResult());

      
        await _repository.AcceptQuestAsync(player, quest);

     
        var playerQuest = await _context.PlayerQuests.FirstOrDefaultAsync();
        Assert.NotNull(playerQuest);
        Assert.Equal(player.Id, playerQuest.PlayerId);
        Assert.Equal(quest.Id, playerQuest.QuestId);
        Assert.Equal(QuestStatus.Accepted, playerQuest.Status);
    }

    [Fact]
    public async Task GetAvailableQuestsAsync_PlayerHasNoCompletedQuests_ReturnsAllQuests()
    {
        
        var player = new Player { Id = Guid.NewGuid(), Name = "Test Player", PlayerQuests = new List<PlayerQuest>() };
        var quest1 = new Quests { Id = Guid.NewGuid(), Title = "Quest 1" };
        var quest2 = new Quests { Id = Guid.NewGuid(), Title = "Quest 2" };

        _context.Players.Add(player);
        _context.Quests.AddRange(quest1, quest2);
        await _context.SaveChangesAsync();

        
        var availableQuests = await _repository.GetAvailableQuestsAsync(player);

      
        Assert.NotNull(availableQuests);
        Assert.Equal(2, availableQuests.Count());
        Assert.Contains(quest1, availableQuests);
        Assert.Contains(quest2, availableQuests);
    }

    [Fact]
    public async Task GetAvailableQuestsAsync_PlayerHasCompletedQuests_ReturnsOnlyAvailableQuests()
    {
      
        var questId = Guid.NewGuid(); 
        var player = new Player
        {
            Id = Guid.NewGuid(),
            Name = "Test Player",
            PlayerQuests = new List<PlayerQuest>
        {
            new PlayerQuest { QuestId = questId, Status = QuestStatus.Finished }
        }
        };

        var quest1 = new Quests { Id = Guid.NewGuid(), Title = "Quest 1" };
        var quest2 = new Quests { Id = questId, Title = "Quest 2" }; 

        _context.Players.Add(player);
        _context.Quests.AddRange(quest1, quest2);
        await _context.SaveChangesAsync();

        
        var availableQuests = await _repository.GetAvailableQuestsAsync(player);

       
        Assert.NotNull(availableQuests);
        Assert.Single(availableQuests);
        Assert.Contains(quest1, availableQuests); 
        Assert.DoesNotContain(quest2, availableQuests); 
    }

    [Fact]
    public async Task CompleteQuestAsync_PlayerQuestIsCompleted_UpdatesQuestToFinishedAndAwardsRewards()
    {
        var player = new Player
        {
            Id = Guid.NewGuid(),
            Name = "Test Player",
            PlayerQuests = new List<PlayerQuest>
        {
            new PlayerQuest
            {
                QuestId = Guid.NewGuid(),
                Status = QuestStatus.Completed
            }
        }
        };

        var quest = new Quests
        {
            Id = player.PlayerQuests.First().QuestId,
            Title = "Test Quest"
        };

        var questReward = new QuestReward
        {
            QuestId = quest.Id,
            ExperiencePoints = 100,
            Currency = 50,
            Items = new List<RewardItem> { new RewardItem { ItemName = "Reward Item" } }
        };

        _context.Players.Add(player);
        _context.QuestRewards.Add(questReward);
        await _context.SaveChangesAsync();

        await _repository.CompleteQuestAsync(player, quest);

        var updatedPlayerQuest = await _context.PlayerQuests
            .FirstOrDefaultAsync(pq => pq.PlayerId == player.Id && pq.QuestId == quest.Id);

        Assert.NotNull(updatedPlayerQuest);
        Assert.Equal(QuestStatus.Finished, updatedPlayerQuest.Status);

        var updatedPlayer = await _context.Players.FindAsync(player.Id);
        Assert.Equal(100, updatedPlayer.ExperiencePoints);
        Assert.Equal(50, updatedPlayer.Currency);
        Assert.Contains(updatedPlayer.PlayerItems, playerItem => playerItem.RewardItem.ItemName == "Reward Item"); // Adjust to match property
    }


    //[Fact]
    //public async Task UpdateQuestProgressAsync_ValidInput_UpdatesProgress()
    //{
    //    // Arrange
    //    var player = new Player { Id = Guid.NewGuid() };
    //    var quest = new Quests
    //    {
    //        Id = Guid.NewGuid(),
    //        Conditions = new List<QuestCondition>
    //        {
    //            new QuestCondition { Id = Guid.NewGuid(), RequiredValue = 5 }
    //        }
    //    };
    //    var condition = quest.Conditions.First();
    //    var playerQuest = new PlayerQuest
    //    {
    //        PlayerId = player.Id,
    //        QuestId = quest.Id,
    //        Progress = new List<QuestProgress>
    //        {
    //            new QuestProgress { ConditionId = condition.Id, CurrentValue = 0, TargetValue = 5 }
    //        }
    //    };


    //    var progressUpdates = new List<QuestProgress>
    //    {
    //       new QuestProgress { ConditionId = condition.Id, CurrentValue = 0, TargetValue = condition.RequiredValue }
    //    };

    //    // Mocking player and quest validation
    //    _playerValidatorMock.Setup(v => v.ValidateAsync(player))
    //        .ReturnsAsync(new ValidationResult());
    //    _questValidatorMock.Setup(v => v.ValidateAsync(quest))
    //        .ReturnsAsync(new ValidationResult());

    //    // Mocking DbContext behavior
    //    _mockContext.Setup(c => c.PlayerQuests)
    //        .ReturnsDbSet(new List<PlayerQuest> { playerQuest });

    //    // Act
    //    await _repository.UpdateQuestProgressAsync(player, quest, progressUpdates);

    //    // Assert
    //    Assert.Equal(3, playerQuest.Progress.First().CurrentValue);
    //    _mockContext.Verify(c => c.SaveChangesAsync(), Times.Once);
    //}
}