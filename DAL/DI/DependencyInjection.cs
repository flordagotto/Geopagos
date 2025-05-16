using DAL.Context;
using DAL.Mapping;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<GameDbContext>(options =>
                options.UseSqlServer(connectionString));

            // repositories
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<GameDbContext>());
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();

            // mapper
            services.AddAutoMapper(typeof(PlayerMapper).Assembly);
            services.AddAutoMapper(typeof(MatchMapper).Assembly);
            services.AddAutoMapper(typeof(TournamentMapper).Assembly);

            return services;
        }
    }
}
