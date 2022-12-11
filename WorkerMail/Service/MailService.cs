using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using TicketingAppBackEnd.Protos;
using WorkerMail.Interface;

namespace WorkerMail.Service;

public class MailService : IMailService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<MailService> _logger;
    private readonly CancellationToken _cancellationToken;
    private ConcurrentQueue<mail> _mailsQueue;


    public MailService(
        IBackgroundTaskQueue taskQueue,
        ILogger<MailService> logger,
        IHostApplicationLifetime applicationLifetime)
    {
        _taskQueue = taskQueue;
        _logger = logger;
        _cancellationToken = applicationLifetime.ApplicationStopping;
        _mailsQueue = new ConcurrentQueue<mail>();
    }


    public void SendMail(mail mail)
    {
        _mailsQueue.Enqueue(mail);
    }

    public void StartMailLoop()
    {
        _logger.LogInformation($"{nameof(SendMailWorkerAsync)} loop is starting.");

        // Run a console user input loop in a background thread
        Task.Run(async () => await SendMailWorkerAsync(), _cancellationToken);
    }

    private async ValueTask SendMailWorkerAsync()
    {
        mail mailObj;
        var nbErrors = 0;
        while (!_cancellationToken.IsCancellationRequested && _mailsQueue.TryDequeue(out mailObj!))
        {
            _logger.LogInformation("Sending Mail");

            if (nbErrors > 10)
            {
                _logger.LogWarning("To Many Errors, going to sleep for {SleepingTime} Seconds ",
                    Math.Max(nbErrors, 3_000));
                await Task.Delay(Math.Max(nbErrors, 3_000) * 1_000, _cancellationToken);
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
                    await client.SendMailAsync(message, _cancellationToken);
                }
                else
                {
                    await Task.Delay(1000, _cancellationToken);
                }
                nbErrors = 0;
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if the Delay is cancelled
            }
            catch (SmtpException)
            {
                nbErrors++;
                _mailsQueue.Enqueue(mailObj);
            }

            _logger.LogInformation("Mail sent");
        }
    }
}