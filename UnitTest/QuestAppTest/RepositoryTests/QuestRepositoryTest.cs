using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Quest.Domain.Entities;
using Quest.Persistance.Concretes.Repositories;
using Quest.Persistance.Context;

namespace QuestAppTest.QuestRepositoryTests;
public class QuestRepositoryTests
{
    private readonly Mock<ILogger<QuestRepository>> _mockLogger;
    private readonly QuestContext _context;
    private readonly QuestRepository _repository;

    public QuestRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<QuestRepository>>();

        
        var options = new DbContextOptionsBuilder<QuestContext>()
            .UseInMemoryDatabase(databaseName: "QuestDatabase")
            .Options;

        _context = new QuestContext(options);

        
        _repository = new QuestRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task GetQuestWithRewardsAsync_ValidQuestId_ReturnsQuestWithRewards()
    {
      
        var questId = Guid.NewGuid();
        var quest = new Quests
        {
            Id = questId,
            Title = "Find the Treasure",  
            Description = "A quest to find the hidden treasure.",  
            QuestRewards = new List<QuestReward>
            {
                new QuestReward
                {
                    RewardItems = new List<RewardItem>
                    {
                        new RewardItem { Id = Guid.NewGuid(), ItemName = "Gold Coin" }
                    }
                }
            }
        };

       
        _context.Quests.Add(quest);
        await _context.SaveChangesAsync();

       
        var result = await _repository.GetQuestWithRewardsAsync(questId);

        
        Assert.NotNull(result);
        Assert.Equal(questId, result.Id);
        Assert.NotEmpty(result.QuestRewards);
        Assert.Equal("Gold Coin", result.QuestRewards.First().RewardItems.First().ItemName);
    }

    [Fact]
    public async Task GetQuestWithRewardsAsync_InvalidQuestId_ReturnsNull()
    {
       
        var result = await _repository.GetQuestWithRewardsAsync(Guid.NewGuid());

        
        Assert.Null(result);
    }

    [Fact]
    public async Task GetQuestWithRewardsAsync_ExceptionThrown_LogsError()
    {
       
        _mockLogger.Setup(l => l.Log(
                LogLevel.Error,                  
                It.IsAny<EventId>(),            
                It.Is<It.IsAnyType>((v, t) => true), 
                It.IsAny<Exception>(),         
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()) 
        ).Verifiable();

       
        _context.Database.EnsureDeleted();
        var invalidContext = new QuestContext(new DbContextOptions<QuestContext>());
        var repositoryWithInvalidContext = new QuestRepository(invalidContext, _mockLogger.Object);

        
        await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryWithInvalidContext.GetQuestWithRewardsAsync(Guid.NewGuid()));

        
        _mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }
}
