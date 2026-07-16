using PicPay.Domains.Notificacoes;

namespace PicPay.Services.Notification
{
    /// <summary>
    /// Interface for sending notifications to users involved in a transaction.
    /// </summary>
    public interface INotificationSender
    {
        /// <summary>
        /// Publishes an email notification to the message broker (RabbitMQ) for processing.
        /// </summary>
        /// <param name="emailDTO">The notification payload containing transaction details, payer and payee emails.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task PublishEmail(EmailDTO emailDTO);
    }
}
