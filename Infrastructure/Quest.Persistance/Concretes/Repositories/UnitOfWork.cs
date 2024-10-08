using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Quest.Application.Abstracts.Repositories;
using Quest.Domain.Entities;
using Quest.Persistance.Context;

namespace Quest.Persistance.Concretes.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly QuestContext _context;
    private PlayerRepository _playerRepository;
    private QuestRepository _questRepository;
    public UnitOfWork(QuestContext context)
    {
        _context = context;
    }

    public IPlayerRepository PlayerRepository
    {
        get => _playerRepository ??= new PlayerRepository(_context);
        set => _playerRepository = (PlayerRepository)value;
    }
    public IQuestRepository QuestRepository { 
        get => _questRepository ??= new QuestRepository(_context); 
        set => _questRepository = (QuestRepository)value; 
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
