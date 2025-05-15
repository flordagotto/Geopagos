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
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();

            // mapper
            services.AddAutoMapper(typeof(PlayerMapper).Assembly);
            services.AddAutoMapper(typeof(MatchMapper).Assembly);

            return services;
        }
    }
}
