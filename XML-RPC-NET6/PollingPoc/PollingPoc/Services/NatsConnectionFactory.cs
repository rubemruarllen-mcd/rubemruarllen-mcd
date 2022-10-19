using NATS.Client;
using PollingPoc.Domain.Interfaces;

namespace PollingPoc.Services
{
    /// <summary>
    /// Default implementation of <see cref="INatsConnectionFactory"/>
    /// </summary>
    public class NatsConnectionFactory : INatsConnectionFactory
    {
        private ConnectionFactory _connectionFactory;

        /// <summary>
        /// Constructor of NatsConnectionFactory.
        /// </summary>
        /// <param name="connectionFactory">Provides factory methods to create connections to Nats.</param>
        public NatsConnectionFactory(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <inheritdoc cref="INatsConnectionFactory.CreateConnection"/>
        public IConnection CreateConnection(Options options)
        {
            return _connectionFactory.CreateConnection(options);
        }

    }
}
