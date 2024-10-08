using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Quest.Domain.Entities;
using Quest.Persistance.Concretes.Repositories;
using Quest.Persistance.Context;


public class QuestRepositoryTests
{
    private readonly Mock<ILogger<QuestRepository>> _mockLogger;
    private readonly QuestContext _context;
    private readonly QuestRepository _repository;

    public QuestRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<QuestRepository>>();

        // Set up an in-memory database for testing
        var options = new DbContextOptionsBuilder<QuestContext>()
            .UseInMemoryDatabase(databaseName: "QuestDatabase")
            .Options;

        _context = new QuestContext(options);

        // Initialize the repository
        _repository = new QuestRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task GetQuestWithRewardsAsync_ValidQuestId_ReturnsQuestWithRewards()
    {
        // Arrange
        var questId = Guid.NewGuid();
        var quest = new Quests
        {
            Id = questId,
            QuestRewards = new List<QuestReward>
            {
                new QuestReward
                {
                    Items = new List<RewardItem>
                    {
                        new RewardItem { Id = Guid.NewGuid(), ItemName = "Gold Coin" }
                    }
                }
            }
        };

        // Add the quest to the in-memory database
        _context.Quests.Add(quest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetQuestWithRewardsAsync(questId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(questId, result.Id);
        Assert.NotEmpty(result.QuestRewards);
        Assert.Equal("Gold Coin", result.QuestRewards.First().Items.First().ItemName);
    }

    [Fact]
    public async Task GetQuestWithRewardsAsync_InvalidQuestId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetQuestWithRewardsAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetQuestWithRewardsAsync_ExceptionThrown_LogsError()
    {
        // Arrange
        _mockLogger.Setup(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()))
                    .Verifiable();

        // Intentionally setting up the context to throw an exception
        _context.Database.EnsureDeleted(); // Clean the database
        var invalidContext = new QuestContext(new DbContextOptions<QuestContext>()); // Create a new invalid context
        var repositoryWithInvalidContext = new QuestRepository(invalidContext, _mockLogger.Object);

        // Act
        await Assert.ThrowsAsync<Exception>(() => repositoryWithInvalidContext.GetQuestWithRewardsAsync(Guid.NewGuid()));

        // Assert
        _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
    }
}
