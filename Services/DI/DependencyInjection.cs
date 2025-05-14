using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}
