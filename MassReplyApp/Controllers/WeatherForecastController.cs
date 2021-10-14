using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using MassReplyApp.Interfaces;
using MassReplyApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MassReplyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IEmailSender _emailSender;
        private readonly IEmailGetter _emailGetter;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailSender emailSender, IEmailGetter emailGetter)
        {
            _logger = logger;
            _emailSender = emailSender;
            _emailGetter = emailGetter;
        }

        [HttpGet]
        public void Get()
        {
            var rng = new Random();

            GmailService service = _emailGetter.GetService();
            _emailGetter.GetAll(service);

            var message = new Message(new string[] { "devmonicasandoval@gmail.com" }, "Test emai", "This is a test email from C#");

            //_emailSender.SendEmail(message);


        }
    }
}
