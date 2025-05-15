using Microsoft.Extensions.DependencyInjection;
using Services.Mapping;
using Services.Services;

namespace Services.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // services
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<ITournamentService, TournamentService>();

            // mappers
            services.AddAutoMapper(typeof(PlayerMapper).Assembly);
            services.AddAutoMapper(typeof(MatchMapper).Assembly);

            return services;
        }
    }
}
