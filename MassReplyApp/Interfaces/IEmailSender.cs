using System;
using MassReplyApp.Models;

namespace MassReplyApp.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
