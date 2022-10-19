using Microsoft.AspNetCore.Connections;
using NATS.Client;
using PollingPoc.Domain.Interfaces;
using System.Text;

namespace PollingPoc.Services
{
    /// <summary>
    /// This service is responsible for sending messages through NATS.
    /// </summary>
    public class NatsService : IMessageBroker
    {

        private readonly INatsConnectionFactory _natsConnectionFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor of <see cref="NatsService"/>
        /// </summary>
        /// <param name="natsConnectionFactory">ConnectionFactory of Nats.</param>
        /// <param name="logger">NatsService logger.</param>
        public NatsService( INatsConnectionFactory natsConnectionFactory, ILogger<NatsService> logger)
        {

            _natsConnectionFactory = natsConnectionFactory;
            _logger = logger;
        }

        /// <inheritdoc cref="IMessageBroker.Publish(string, object)"/>
        public void Publish(string subject = null, object message = null)
        {
            try
            {
                _logger.LogInformation("Connecting to Nats");
                IConnection connection = ConnectToNats();

                if (string.IsNullOrEmpty(subject))
                    subject = "PocReport";

                if (string.IsNullOrEmpty(message.ToString()))
                    message = "Backend State Changed";

                _logger.LogInformation("Publishing Message {subject}", subject);
                connection.Publish(subject, Encoding.UTF8.GetBytes(message.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending NATS message");
            }
        }

        /// <inheritdoc cref="ConnectToNats" />
        private IConnection ConnectToNats()
        {
            var options = ConnectionFactory.GetDefaultOptions();
            string[] serve = { "nats://localhost:4222" };
            options.Servers = serve;
            options.Timeout = 3000;
            var connection = _natsConnectionFactory.CreateConnection(options);
            _logger.LogInformation("Connection with nats completed");
            return connection;
        }
    }
}
