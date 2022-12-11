using System.Net;
using System.Net.Mail;
using MailService.Interface;

namespace MailService.Services;

public class WorkerService : IWorkerService
{
    private readonly CancellationToken _cancellationToken;
    private readonly IShareBlockingCollection _mailsQueue;
    private readonly ILogger<WorkerService> _logger;


    public WorkerService(  ILogger<WorkerService> logger,
        IHostApplicationLifetime applicationLifetime, IShareBlockingCollection mailsQueue)
    {
        _logger = logger;
        _mailsQueue = mailsQueue;
        _cancellationToken = applicationLifetime.ApplicationStopping;
    }


    public void StartLoop()
    {
        _logger.LogInformation($"{nameof(SendMailWorkerAsync)} loop is starting.");
        Task.Run(SendMailWorkerAsync, _cancellationToken);
    }

    private async ValueTask SendMailWorkerAsync()
    {
        var nbErrors = 0;
        while (true)
        {
            _logger.LogTrace("Awaiting mail");
            var mailObj = _mailsQueue.Take();

            if (nbErrors > 10)
            {
                _logger.LogInformation("To Many Errors, going to sleep for {SleepingTime} Seconds ",
                    Math.Min(nbErrors, 3_000));
                await Task.Delay(Math.Min(nbErrors, 3_000), _cancellationToken);
            }

            try
            {
                var to = new MailAddress(mailObj.Target);
                var from = new MailAddress(mailObj.Sender);
                var message = new MailMessage(from, to);
                message.Subject = mailObj.Subject;
                message.Body = mailObj.Body;
                var client = new SmtpClient("smtp.server.address", 2525)
                {
                    Credentials = new NetworkCredential("smtp_username", "smtp_password"),
                    EnableSsl = true
                };

                if (Environment.GetEnvironmentVariable("ENVIRONEMENT") == "Production")
                {
                    _logger.LogInformation("Sending Mail");
                    client.Send(message);
                }
                else
                {
                    _logger.LogInformation("Dev Mail not really send");

                    //Simulate some errors
                    var rand = new Random();
                    var number = rand.Next(0, 100);
                    if (number < 60)
                    {
                        throw new SmtpException(SmtpStatusCode.GeneralFailure,"Simulating some troubles");
                    }
                    await Task.Delay(1000, _cancellationToken);
                }
                _logger.LogInformation("Mail sent to : {dest}",mailObj.Target);
                nbErrors = 0;
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (SmtpException ex)
            {
                nbErrors++;
                _logger.LogWarning("Mail Not sent, cause : " + ex?.Message + " -> Retry");
                _mailsQueue.Add(mailObj);
            }
            catch (Exception ex)
            {
                _logger.LogError("Mail Not sent, cause : " + ex?.Message + " -> Aborting");
            }

        }
        // ReSharper disable once FunctionNeverReturns
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        StartLoop();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}