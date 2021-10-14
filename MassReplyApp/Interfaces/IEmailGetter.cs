using System;
using Google.Apis.Gmail.v1;

namespace MassReplyApp.Interfaces
{
    public interface IEmailGetter
    {
        GmailService GetService();
        void GetAll(GmailService service);
    }
}
