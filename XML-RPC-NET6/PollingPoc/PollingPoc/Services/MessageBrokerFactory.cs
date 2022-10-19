using PollingPoc.Domain.Interfaces;
using PollingPoc.Domain.Models;

namespace PollingPoc.Services
{
    /// <summary>
    /// The MessageBrokerFactory is responsible to get a communication service to WebSocket connection.
    /// </summary>
    public class MessageBrokerFactory : IMessageBrokerFactory
    {
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;
        private readonly INatsConnectionFactory _natsConnectionFactory;

        /// <summary>
        /// Constructor of <see cref="MessageBrokerFactory"/>
        /// </summary>
        /// <param name="natsProperties">The properties of Nast</param>
        /// <param name="natsConnectionFactory">ConnectionFactory of Nats.</param>
        /// <param name="loggerFactory">LoggerFactory to services</param>
        public MessageBrokerFactory( INatsConnectionFactory natsConnectionFactory, ILoggerFactory loggerFactory)
        {

            _natsConnectionFactory = natsConnectionFactory;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger(typeof(MessageBrokerFactory).ToString());
        }

        /// <inheritdoc cref="IMessageBrokerFactory.GetCommunicationServices(CommunicationService)"/>
        public IMessageBroker GetCommunicationServices(CommunicationService communicationService)
        {
            _logger.LogInformation("CommunicationService running with Nats");
            return new NatsService( _natsConnectionFactory, _loggerFactory.CreateLogger<NatsService>());
        }
    }
}
