using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BookingCare.Application.Utils;
using MimeKit;
using Serilog;

namespace BookingCare.Infrastructure.Services
{
    // Cấu hình dịch vụ gửi mail, giá trị Inject từ appsettings.json
    public class MailSettings
    {
        public string? Mail { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }

    // Dịch vụ gửi mail
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public MailService
            (
                IOptions<MailSettings> mailSettings,
                ILogger<MailService> logger
            )
        {
            _mailSettings = mailSettings.Value;
            Log.Information("Create MailService");
        }

        public async Task SendAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                Log.Information("Lỗi gửi mail, lưu tại - " + emailsavefile);
                Log.Error(ex.Message);
            }

            smtp.Disconnect(true);

            Log.Information("send mail to: " + email);
        }
    }
}
