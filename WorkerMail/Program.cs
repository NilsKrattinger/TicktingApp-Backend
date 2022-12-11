using Microsoft.AspNetCore.Builder;
using WorkerMail;
using WorkerMail.Interface;
using WorkerMail.Model;
using MailService = WorkerMail.Service.MailService;

AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MailService>();
builder.Services.AddHostedService<MailHostedService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(_ =>
{
    const int queueCapacity = 100;
    return new DefaultBackgroundTaskQueue(queueCapacity);
});
builder.Services.AddGrpc();


var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapGrpcService<MailService>(); });
var monitorLoop = app.Services.GetRequiredService<MailService>();
monitorLoop.StartMailLoop();


await app.RunAsync();