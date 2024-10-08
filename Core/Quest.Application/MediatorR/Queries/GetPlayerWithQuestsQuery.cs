using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Quest.Domain.Entities;

namespace Quest.Application.MediatorR.Queries
{
    public class GetPlayerWithQuestsQuery : IRequest<Player>
    {
        public Guid PlayerId { get; set; }
    }
}
