using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MassReplyApp.Interfaces;
using MassReplyApp.Models;
using Message = Google.Apis.Gmail.v1.Data.Message;

namespace MassReplyApp
{
    public class EmailGetter : IEmailGetter
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailGetter(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

       public GmailService GetService()
            {
            UserCredential credential;
                string[] Scopes = { GmailService.Scope.GmailReadonly };
                string ApplicationName = "MassReplyApp";
                using (var stream =
              new FileStream("appsettings.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Gmail API service.
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            //TODO:somehow use same method as EmailSender service to connec to gmail
                return service;

            }
           

       
           public void GetAll(GmailService service)
            {
            string host = _emailConfig.From;

            List<GmailEmail> emails = new List<GmailEmail>();
            GmailEmail email = new GmailEmail();

                UsersResource.MessagesResource.ListRequest listRequest = service.Users.Messages.List(host);
                listRequest.IncludeSpamTrash = false;
                listRequest.Q = "is:unread";
            //TODO:Filter messages, job related messages only

                ListMessagesResponse listMessagesResponse = listRequest.Execute();

                if (listMessagesResponse != null)
                {
                    foreach (Message message in listMessagesResponse.Messages)
                    {
                        UsersResource.MessagesResource.GetRequest singleMessage = service.Users.Messages.Get(host, message.Id);
                        Console.WriteLine("MESSAGE RECIEVED --> {0}", message.Id);

                        Message msg = singleMessage.Execute();
                        if (msg != null)
                        {
                            email.MessageId = message.Id;

                            foreach (var messageParts in msg.Payload.Headers)
                            {
                                if (messageParts.Name == "From")
                                {
                                    email.From = messageParts.Value;
                                }
                                if (messageParts.Name == "To")
                                {
                                    email.To = messageParts.Value;
                                }
                                if (messageParts.Name == "Date")
                                {
                                    email.MailDateTime = messageParts.Value;
                                }
                                else if (messageParts.Name == "Subject")
                                {
                                    email.Subject = messageParts.Value;
                                }
                                Console.WriteLine(email);
                            }

                         
                            //List<string> files = new List<string>();

                            //IList<MessagePart> parts = msg.Payload.Parts;
                            //if (parts != null)
                            //{
                            //    foreach(MessagePart part in parts)
                            //    {
                            //        if (!String.IsNullOrEmpty(part.Filename)
                            //        {
                            //            string attId = part.Body.AttachmentId;
                            //            MessagePartBody attachment = service.Users.Messages.Attachments.Get(host, msg.Id, attId).Execute();

                            //        }

                            //   }
                            // }

                        }
                    }
                }

                Console.WriteLine(listMessagesResponse);
         
                return;
            }


      

        
    }
}

