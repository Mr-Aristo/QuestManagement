using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestAppTest.Services.InMemoryService.DataSeed;

public interface IDataSeedService
{
    Task SeedDataAsync();
}
