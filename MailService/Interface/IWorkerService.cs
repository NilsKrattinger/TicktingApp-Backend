namespace MailService.Interface;

public interface IWorkerService : IHostedService
{
    public void StartLoop();
}