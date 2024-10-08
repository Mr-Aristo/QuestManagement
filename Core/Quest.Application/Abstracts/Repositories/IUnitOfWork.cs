using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Application.Abstracts.Repositories;

public interface IUnitOfWork : IDisposable
{
    IPlayerRepository PlayerRepository { get; set; }
    IQuestRepository QuestRepository { get; set; }

    Task<int> SaveAsync();
}
