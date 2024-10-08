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

internal class GetPlayerWithQuestsQueryHandler : IRequestHandler<GetAvailableQuestsQuery, IEnumerable<Quests>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPlayerWithQuestsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Quests>> Handle(GetAvailableQuestsQuery request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.PlayerRepository.GetPlayerWithQuestsAsync(request.PlayerId);
        return await _unitOfWork.PlayerRepository.GetAvailableQuestsAsync(player);
    }
}
