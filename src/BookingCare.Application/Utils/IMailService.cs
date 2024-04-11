namespace BookingCare.Application.Utils
{
    public interface IMailService
    {
        public Task SendAsync(string email, string subject, string htmlMessage);
    }
}
