using MailService.Interface;
using MailService.Services;

namespace MailService;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        AppContext.SetSwitch(
            "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        services.AddSingleton<IShareBlockingCollection, ShareBlockingCollectionService>();
        services.AddSingleton<IWorkerService, WorkerService>();
        services.AddHostedService<WorkerService>();
        services.AddScoped<IMailService, Services.MailService>();
        services.AddGrpc(options => { });
        services.AddGrpc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapGrpcService<Services.MailService>(); });
    }
}