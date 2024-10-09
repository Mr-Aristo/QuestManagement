using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Quest.Application.Validations;
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
                new PlayerQuest { Quest = new Quests { Title = "Quest 1" , Description = "Description 1 "} },
                new PlayerQuest { Quest = new Quests { Title = "Quest 2" , Description = "Description 1 "} }
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
        var playerId = Guid.NewGuid();

        var rewardItem = new RewardItem
        {
            Id = Guid.NewGuid(),
            ItemName = "item",
            Quantity = 2
        };

        var player = new Player
        {
            Id = playerId,
            Name = "Test Player",
            ExperiencePoints = 100,
            Currency = 50
        };

        var quest = new Quests
        {
            Id = Guid.NewGuid(),
            Title = "Test Quest",
            Description = "This is a test quest.",
            RequiredProgress = 1,
            QuestRequirements = new List<QuestRequirement>
        {
            new QuestRequirement
            {
                Id = Guid.NewGuid(),
                RequirementType = "Collect",
                RequiredAmount = 5
            }
        },
            QuestRewards = new List<QuestReward>
        {
            new QuestReward
            {
                Id = Guid.NewGuid(),
                ExperiencePoints = 500,
                RewardItems = new List<RewardItem> { rewardItem }
            }
        },
            Conditions = new List<QuestCondition>
        {
            new QuestCondition
            {
                Id = Guid.NewGuid(),
                RequiredValue = 1,
                Description = "test"
            }
        },
            PlayerQuests = new List<PlayerQuest>
        {
            new PlayerQuest
            {
                PlayerId = Guid.NewGuid(),
                QuestId = Guid.NewGuid(),
                Status = QuestStatus.Accepted
            }
        }
        };

        _playerValidatorMock.Setup(v => v.ValidateAsync(player, default))
                            .ReturnsAsync(new ValidationResult());
        _questValidatorMock.Setup(v => v.ValidateAsync(quest, default))
                           .ReturnsAsync(new ValidationResult());

        await _repository.AcceptQuestAsync(player, quest);

        var playerQuest = await _context.PlayerQuests.FirstOrDefaultAsync();

        Assert.NotNull(playerQuest);
        Assert.Equal(playerId, playerQuest.PlayerId);
        Assert.Equal(quest.Id, playerQuest.QuestId);
        Assert.Equal(QuestStatus.Accepted, playerQuest.Status);
    }


    [Fact]
    public async Task AcceptQuestAsync_NullPlayer_ThrowsArgumentNullException()
    {

        Player player = null;
        var quest = new Quests { Id = Guid.NewGuid(), Title = "Test Quest" };


        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AcceptQuestAsync(player, quest));
        Assert.Equal("player", exception.ParamName);
    }


    [Fact]
    public async Task GetAvailableQuestsAsync_PlayerHasNoCompletedQuests_ReturnsAllQuests()
    {

        var player = new Player { Id = Guid.NewGuid(), Name = "Test Player", PlayerQuests = new List<PlayerQuest>() };
        var quest1 = new Quests { Id = Guid.NewGuid(), Title = "Quest 1", Description = "Test Description 1 " };
        var quest2 = new Quests { Id = Guid.NewGuid(), Title = "Quest 2", Description = "Test Description 2 " };

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

        var quest1 = new Quests { Id = Guid.NewGuid(), Title = "Quest 1", Description = "Test Description 1 " };
        var quest2 = new Quests { Id = questId, Title = "Quest 2", Description = "Test Description 1 " };

        _context.Players.Add(player);
        _context.Quests.AddRange(quest1, quest2);
        await _context.SaveChangesAsync();


        var availableQuests = await _repository.GetAvailableQuestsAsync(player);


        Assert.NotNull(availableQuests);
        Assert.Single(availableQuests);
        Assert.Contains(quest1, availableQuests);
        Assert.DoesNotContain(quest2, availableQuests);
    }

 }