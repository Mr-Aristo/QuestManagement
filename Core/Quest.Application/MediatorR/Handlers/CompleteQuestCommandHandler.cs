using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Quest.Application.Abstracts.Repositories;
using Quest.Application.MediatorR.Commands;

namespace Quest.Application.MediatorR.Handlers;

public class CompleteQuestCommandHandler : IRequestHandler<CompleteQuestCommand,Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public CompleteQuestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CompleteQuestCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerWithQuestsAsync(request.PlayerId);
        var quest = await _unitOfWork.QuestRepository.GetQuestWithRewardsAsync(request.QuestId);

        if (player == null || quest == null)
        {
            throw new InvalidOperationException("Player or Quest not found.");
        }

        await _unitOfWork.PlayerRepository.CompleteQuestAsync(player, quest);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}
