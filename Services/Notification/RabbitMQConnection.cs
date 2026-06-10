using RabbitMQ.Client;

namespace PicPay.Services.Notification
{
    public class RabbitMqConnection : IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqConnection()
        {
            string cloudAmqpUrl = "amqps://kllfmgma:SVKhpZM31WD61v4zqmb0mlupwg8WQoWn@://cloudamqp.com";

            _factory = new ConnectionFactory
            {
                Uri = new Uri(cloudAmqpUrl),
                AutomaticRecoveryEnabled = true
            };
        }
        public async Task<IChannel> GetChannelAsync()
        {
            if (_connection == null) _connection = await _factory.CreateConnectionAsync();
            if (_channel == null) _channel = await _connection.CreateChannelAsync();
            return _channel;
        }
        public async ValueTask DisposeAsync()
        {
            if (_channel != null) await _channel.DisposeAsync();
            if (_connection != null) await _connection.CloseAsync();
        }
    }
}
