using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Quest.Application.MediatorR.Commands
{
    public class AcceptQuestCommand : IRequest<Unit>
    {
        public Guid PlayerId { get; set; }
        public Guid QuestId { get; set; }
    }
}
