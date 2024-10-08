using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quest.Application.Abstracts.Repositories;
using Quest.Domain.Entities;
using Quest.Persistance.Context;

namespace Quest.Persistance.Concretes.Repositories;

public class QuestRepository : IQuestRepository
{
    private readonly QuestContext _context;
    private readonly ILogger<QuestRepository> _logger;
   

    public QuestRepository(QuestContext context, ILogger<QuestRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public QuestRepository(QuestContext context) => _context = context;     
   
   
    public async Task<Quests> GetQuestWithRewardsAsync(Guid questId)
    {
        try
        {
            return await _context.Quests
                   .Include(q => q.QuestRewards)
                       .ThenInclude(r => r.Items)
                   .FirstOrDefaultAsync(q => q.Id == questId);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error getting quest wiht reward {QuestId}", questId);
            throw;
        }
    }
}
