using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quest.Domain.Entities;
using Quest.Persistance.Context;

namespace QuestAppTest.Services.InMemoryService.DataSeed
{
    public class DataSeedService : IDataSeedService
    {
        private readonly QuestContext _context;

        public DataSeedService(QuestContext context)
        {
            _context = context;
        }


        public async Task SeedDataAsync()
        {
            if (!await _context.Players.AnyAsync())
            {
                _context.Players.AddRange(
                    new Player { Id = Guid.NewGuid(), Name = "Player 1", Level = 1, ExperiencePoints = 100, Currency = 1000 },
                    new Player { Id = Guid.NewGuid(), Name = "Player 2", Level = 1, ExperiencePoints = 100, Currency = 1000 },
                    new Player { Id = Guid.NewGuid(), Name = "Player 3", Level = 1, ExperiencePoints = 100, Currency = 1000 }
                );
                await _context.SaveChangesAsync();
            }

            // Other table initializations here ...

        }
    }
}
