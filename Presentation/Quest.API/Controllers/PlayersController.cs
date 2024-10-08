using MediatR;
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
    public async Task<IActionResult> AcceptQuest(Guid playerId, Guid questId)
    {
        try
        {
            await _mediator.Send(new AcceptQuestCommand { PlayerId = playerId, QuestId = questId });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while accepting quest for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while accepting quest for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }

    [HttpPost("{playerId}/complete-quest/{questId}")]
    public async Task<IActionResult> CompleteQuest(Guid playerId, Guid questId)
    {
        try
        {
            await _mediator.Send(new CompleteQuestCommand { PlayerId = playerId, QuestId = questId });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while completing quest for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while completing quest for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }

    [HttpPut("{playerId}/update-quest-progress/{questId}")]
    public async Task<IActionResult> UpdateQuestProgress(Guid playerId, Guid questId, [FromBody] IEnumerable<QuestProgress> progressUpdates)
    {
        try
        {
            await _mediator.Send(new UpdateQuestProgressCommand { PlayerId = playerId, QuestId = questId, ProgressUpdates = progressUpdates });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while updating quest progress for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating quest progress for player {PlayerId} and quest {QuestId}.", playerId, questId);
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }

    [HttpGet("{playerId}/quests")]
    public async Task<ActionResult<IEnumerable<Quests>>> GetAvailableQuests(Guid playerId)
    {
        try
        {
            var quests = await _mediator.Send(new GetAvailableQuestsQuery { PlayerId = playerId });
            return Ok(quests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving available quests for player {PlayerId}.", playerId);
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }
}
