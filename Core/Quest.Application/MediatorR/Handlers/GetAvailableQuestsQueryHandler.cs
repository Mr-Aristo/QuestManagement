using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Quest.Application.Abstracts.Repositories;
using Quest.Application.MediatorR.Queries;
using Quest.Domain.Entities;

namespace Quest.Application.MediatorR.Handlers;

public class GetAvailableQuestsQueryHandler : IRequestHandler<GetPlayerWithQuestsQuery, Player>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAvailableQuestsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Player> Handle(GetPlayerWithQuestsQuery request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerWithQuestsAsync(request.PlayerId);
        return player ?? throw new InvalidOperationException("Player not found.");
    }
}

