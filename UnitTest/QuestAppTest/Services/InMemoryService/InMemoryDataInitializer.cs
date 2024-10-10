using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quest.Persistance.Context;
using QuestAppTest.Services.InMemoryService.DataSeed;

namespace QuestAppTest.Services.InMemoryService
{
    public static class InMemoryDataInitializer
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddDbContext<QuestContext>(options => options.UseInMemoryDatabase(databaseName: "TestQuestDB"));

            services.AddTransient<IDataSeedService, DataSeedService>();
        }
    }
}
