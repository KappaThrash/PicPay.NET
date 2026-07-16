using PicPay.Domains.Notificacoes;
using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace PicPay.Services.Notification
{
    /// <summary>
    /// Implements <see cref="INotificationSender"/> using RabbitMQ to publish transaction notifications asynchronously.
    /// </summary>
    public class NotificationSender(RabbitMqConnection _rabbitMqConnection) : INotificationSender
    {
        /// <summary>
        /// Validates the email details and publishes them to the RabbitMQ queue 'default.email'.
        /// </summary>
        /// <param name="emailDTO">The validation-annotated DTO detailing the email properties.</param>
        /// <returns>A task representing the asynchronous publication operation.</returns>
        public async Task PublishEmail(EmailDTO emailDTO)
        {
            var context = new ValidationContext(emailDTO);
            Validator.ValidateObject(emailDTO, context, validateAllProperties: true);

            using var channel = await _rabbitMqConnection.GetChannelAsync();

            string emailjson = JsonSerializer.Serialize(emailDTO);

            var body = Encoding.UTF8.GetBytes(emailjson);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "default.email",
                body: body
            );
        }
    }
}
