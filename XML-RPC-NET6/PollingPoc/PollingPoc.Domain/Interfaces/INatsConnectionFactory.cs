using NATS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingPoc.Domain.Interfaces
{
    /// <summary>
    /// NatsConnectionFactory pattern implementation for creation of instances of <see cref="INatsConnectionFactory"/>.
    /// </summary>
    public interface INatsConnectionFactory
    {
        /// <summary>
        /// Create a connection to a NATS Server.
        /// </summary>
        /// <param name="options">Class used to Setup All NATs client options</param>
        /// <returns></returns>
        IConnection CreateConnection(Options options);
    }
}
