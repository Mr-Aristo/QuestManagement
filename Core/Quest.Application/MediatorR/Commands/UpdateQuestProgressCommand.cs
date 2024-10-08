using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Quest.Domain.Entities;

namespace Quest.Application.MediatorR.Commands
{
    public class UpdateQuestProgressCommand : IRequest <Unit>
    {
        public Guid PlayerId { get; set; }
        public Guid QuestId { get; set; }
        public IEnumerable<QuestProgress> ProgressUpdates { get; set; }
    }
}
