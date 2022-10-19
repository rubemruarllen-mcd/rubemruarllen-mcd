using PollingPoc.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingPoc.Domain.Interfaces
{
    /// <summary>
    /// MessageBrokerFactory pattern implementation for creation of instances of <see cref="IMessageBrokerFactory"/>.
    /// </summary>
    public interface IMessageBrokerFactory
    {
        /// <summary>
        /// This method gets a communication service according with enum CommunicationService.
        /// </summary>
        /// <param name="communicationService">Enum that represents which communication services will be sent.</param>
        /// <returns></returns>
        IMessageBroker GetCommunicationServices(CommunicationService communicationService);
    }
}
