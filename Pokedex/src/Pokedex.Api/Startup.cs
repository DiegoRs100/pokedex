using Pokedex.Api.Configurations;
using Pokedex.Api.Filters;
using Pokedex.Api.Meddlewares;
using Pokedex.Business.Core.Notifications;

namespace Pokedex.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;

        public Startup(WebApplicationBuilder builder)
        {
            _configuration = builder.Configuration;
            _services = builder.Services;
        }

        public void ConfigureServices()
        {
            _services.AddControllers(options => 
                options.Filters.Add<ViewModelFilter>());

            _services.AddEndpointsApiExplorer();
            _services.AddSwaggerGen();
            _services.AddSmartNotification();
            _services.AddServices();
            _services.AddRepositories();
            _services.AddDbContexts(_configuration);
            _services.AddAutoMapper(GetType().Assembly);
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionMeddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();            
            app.UseEndpoints(options => options.MapControllers());
        }
    }
}