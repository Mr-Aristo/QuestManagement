using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quest.Persistance.Context;
using QuestAppTest.Services.InMemoryService.DataSeed;

namespace QuestAppTest.Services.TextFixtureService
{
    public class TestFixture : IDisposable
    {
        private readonly QuestContext _context;
        private readonly IDataSeedService _dataSeedService;
        private readonly IServiceProvider _serviceProvider;

        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddDbContext<QuestContext>(options => options.UseInMemoryDatabase(databaseName:"TestQuestDB"));

            services.AddTransient<IDataSeedService,DataSeedService>();

            _serviceProvider = services.BuildServiceProvider();

            _context = _serviceProvider.GetRequiredService<QuestContext>();
            _dataSeedService = _serviceProvider.GetRequiredService<IDataSeedService>();

            _dataSeedService.SeedDataAsync().Wait();
        }
        public QuestContext GetContext() => _context;

        public T GetService<T>() where T : class => _serviceProvider.GetRequiredService<T>();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
