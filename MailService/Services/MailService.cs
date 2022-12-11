using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MailService.Interface;
using MailService.Protos;

namespace MailService.Services;

public class MailService : Protos.MailService.MailServiceBase ,IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly IShareBlockingCollection _mailsQueue;

    public MailService(
        ILogger<MailService> logger,
        IHostApplicationLifetime applicationLifetime, IShareBlockingCollection mailsQueue)
    {
        _logger = logger;
        _mailsQueue = mailsQueue;
        _cancellationToken = applicationLifetime.ApplicationStopping;
    }


    public override Task<Empty> SendMail(mail mail,ServerCallContext context)
    {
        _mailsQueue.Add(mail);
        return Task.FromResult(new Empty());
    }


}