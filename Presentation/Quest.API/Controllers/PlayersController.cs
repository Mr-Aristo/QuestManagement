﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quest.Application.MediatorR.Commands;
using Quest.Application.MediatorR.Queries;
using Quest.Domain.Entities;

namespace Quest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(IMediator mediator, ILogger<PlayersController> logger)
    {
        _mediator = mediator;
        _logger = logger; 
    }

    [HttpPost("{playerId}/accept-quest/{questId}")]
    public async Task<IActionResult> AcceptQuest(string playerId, string questId)
    {
        try
        {
            await _mediator.Send(new AcceptQuestCommand { PlayerId = Guid.Parse(playerId), QuestId = Guid.Parse(questId) });
            return NoContent();
        }      
        catch (Exception ex)
        {
            _logger.LogError(ex, "При приеме произошла непредвиденная ошибка: quest for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return StatusCode(500, new { Message = "Произошла непредвиденная ошибка." });
        }
    }

    [HttpPost("{playerId}/complete-quest/{questId}")]
    public async Task<IActionResult> CompleteQuest(string playerId, string questId)
    {
        try
        {
            await _mediator.Send(new CompleteQuestCommand { PlayerId = Guid.Parse(playerId), QuestId = Guid.Parse(questId) });
            return NoContent();
        }     
        catch (Exception ex)
        {
            _logger.LogError(ex, "При завершении работы произошла непредвиденная ошибка: quest for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return StatusCode(500, new { Message = "Произошла непредвиденная ошибка." });
        }
    }

    [HttpPut("{playerId}/update-quest-progress/{questId}")]
    public async Task<IActionResult> UpdateQuestProgress(string playerId, string questId, [FromBody] IEnumerable<QuestProgress> progressUpdates)
    {
        try
        {
            await _mediator.Send(new UpdateQuestProgressCommand { PlayerId = Guid.Parse(playerId), QuestId = Guid.Parse(questId), ProgressUpdates = progressUpdates });
            return NoContent();
        }      
        catch (Exception ex)
        {
            _logger.LogError(ex, "При обновлении произошла непредвиденная ошибка: quest progress for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return StatusCode(500, new { Message = "Произошла непредвиденная ошибка." });
        }
    }

    [HttpGet("{playerId}/quests")]
    public async Task<ActionResult<IEnumerable<Quests>>> GetAvailableQuests(string playerId)
    {
        try
        {
            var quests = await _mediator.Send(new GetAvailableQuestsQuery { PlayerId = Guid.Parse(playerId)});
            return Ok(quests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "При извлечении доступных данных произошла непредвиденная ошибка: quests for player {PlayerId}.", playerId);
            return StatusCode(500, new { Message = "Произошла непредвиденная ошибка." });
        }
    }
}
