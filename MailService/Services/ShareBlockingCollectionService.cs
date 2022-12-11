using System.Collections.Concurrent;
using MailService.Interface;
using MailService.Protos;

namespace MailService.Services;

public class ShareBlockingCollectionService : IShareBlockingCollection
{
    private BlockingCollection<mail> _mails;

    public ShareBlockingCollectionService()
    {
        _mails = new BlockingCollection<mail>();
    }

    public BlockingCollection<mail> GetCollection()
    {
        throw new NotImplementedException();
    }

    public void Add(mail addMail)
    {
        _mails.Add(addMail);
    }

    public mail Take()
    {
        return _mails.Take();
    }
}