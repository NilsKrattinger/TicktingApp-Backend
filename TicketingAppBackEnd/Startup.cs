using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketingAppBackEnd.Models;
using TicketingAppBackEnd.Sql;
using TicketingAppBackEnd.Sql.Interfaces;
using TicketingAppBackEnd.Services;

namespace TicketingAppBackEnd
{
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
            services.AddControllers();

            services.AddDbContext<DevContext>(x => x.UseSqlite("Data Source=database.db"));
             services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IConcertRepository, ConcertRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TicektingAppBackEnd", Version = "v1" });
            });
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketing Backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BookingService>();
                endpoints.MapGrpcService<ConcertService>();

                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
