using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace TulipInfo.Net.SendGrid.Tests
{
    [TestClass]
    public class SendGridTests
    {
        [TestMethod]
        public async Task SendEmail()
        {
            var loggerMock = new Mock<ILogger<SendGridEmailSender>>();
            var optMock = new Mock<IOptions<SendGridOptions>>();
            optMock.Setup(s => s.Value).Returns(new SendGridOptions()
            {
                ApiKey = "test",//replace
                MailFrom= "from@outlook.com",//replace
                MailFromDisplayName ="Sender"
            }) ;

            SendGridEmailSender sendr= new SendGridEmailSender(loggerMock.Object, optMock.Object);
            await sendr.SendAsync(new EmailMessage()
            {
                MailTo = "to@outlook.com",//replace
                Subject = "test",
                Body = "<b>Hello</b>"
            });
        }
    }
}