using System.Collections.Concurrent;
using MailService.Protos;

namespace MailService.Interface;

public interface IShareBlockingCollection
{
    public BlockingCollection<mail> GetCollection();
    public void Add(mail mail);
    public mail Take();

}