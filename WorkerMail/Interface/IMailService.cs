using TicketingAppBackEnd.Protos;

namespace WorkerMail.Interface;

public interface IMailService
{
    public void SendMail(mail mail);
}