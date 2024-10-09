using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Quest.API.Controllers;
using Quest.Application.MediatorR.Commands;
using Quest.Application.MediatorR.Queries;
using Quest.Domain.Entities;
using Xunit;

namespace QuestAppTest.ControllerTest;

public class PlayerControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<PlayersController>> _loggerMock;
    private readonly PlayersController _controller;

    public PlayerControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<PlayersController>>();
        _controller = new PlayersController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AcceptQuest_ReturnsNoContent_WhenSuccessful()
    {
        
        var playerId = Guid.NewGuid().ToString();
        var questId = Guid.NewGuid().ToString();
        _mediatorMock.Setup(m => m.Send(It.IsAny<AcceptQuestCommand>(), default)).ReturnsAsync(Unit.Value);

        
        var result = await _controller.AcceptQuest(playerId, questId);

        
        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);
    }

    [Fact]
    public async Task AcceptQuest_Returns500_WhenExceptionOccurs()
    {
        
        var playerId = Guid.NewGuid().ToString();
        var questId = Guid.NewGuid().ToString();
        _mediatorMock.Setup(m => m.Send(It.IsAny<AcceptQuestCommand>(), default)).ThrowsAsync(new Exception());

        
        var result = await _controller.AcceptQuest(playerId, questId);

        
        var actionResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, actionResult.StatusCode);
        Assert.Equal("An unexpected error occurred.", actionResult.Value?.GetType().GetProperty("Message")?.GetValue(actionResult.Value));
    }

    [Fact]
    public async Task CompleteQuest_ReturnsNoContent_WhenSuccessful()
    {
        
        var playerId = Guid.NewGuid().ToString();
        var questId = Guid.NewGuid().ToString();
        _mediatorMock.Setup(m => m.Send(It.IsAny<CompleteQuestCommand>(), default)).ReturnsAsync(Unit.Value);

       
        var result = await _controller.CompleteQuest(playerId, questId);

     
        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateQuestProgress_ReturnsNoContent_WhenSuccessful()
    {
        
        var playerId = Guid.NewGuid().ToString();
        var questId = Guid.NewGuid().ToString();
        var progressUpdates = new List<QuestProgress>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuestProgressCommand>(), default)).ReturnsAsync(Unit.Value);

        
        var result = await _controller.UpdateQuestProgress(playerId, questId, progressUpdates);

        
        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);
    }
     
}
