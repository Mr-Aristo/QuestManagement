using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quest.Application.Abstracts.Repositories;
using Quest.Persistance.Concretes.Repositories;
using Quest.Persistance.Context;

namespace Quest.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceService(this IServiceCollection services)
        {
            services.AddDbContext<QuestContext>(opttions => opttions.UseNpgsql(ConfigurationContext.ConnectionString));

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }

    }
}
