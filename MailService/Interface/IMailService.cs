using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MailService.Protos;

namespace MailService.Interface;

public interface IMailService
{
    public Task<Empty> SendMail(mail mail,ServerCallContext context);
}