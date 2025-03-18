using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingtelAssessment.Application.Interfaces
{
    public interface IMessageQueueService
    {
        Task PublishAsync<T>(string queueName, T message);
    }
}
