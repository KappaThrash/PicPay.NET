using PicPay.Domains;

namespace PicPay.Services.Notification
{
    public interface INotificationSender
    {
        public Task PublishEmail(EmailDTO emailDTO);
    }
}
