using PicPay.Domains;
using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace PicPay.Services.Notification
{
    public class NotificationSender(RabbitMqConnection _rabbitMqConnection) : INotificationSender
    {
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
