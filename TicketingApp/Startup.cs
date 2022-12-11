using Grpc.Net.Client;

namespace TicketingApp;

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


// The port number(5000) must match the port of the gRPC server.
        services.AddControllersWithViews();
        services.AddGrpcClient<Protos.ConcertService.ConcertServiceClient>(x =>
        {
            x.Address = new Uri("https://localhost:5006");
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            return httpHandler;
        });
        //   services.AddGrpcClient<Protos.BookingService.BookingServiceClient>(x =>
        //      x.Address = new Uri("https://localhost:5006"));
        services.AddGrpcClient<Protos.BookingService.BookingServiceClient>(x =>
        {
            x.Address = new Uri("https://localhost:5006");
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            return httpHandler;
        });
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        AppContext.SetSwitch(
            "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Concerts}/{action=Index}/{id?}");
        });
    }
}