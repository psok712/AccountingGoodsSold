using System.Threading;
using System.Threading.Tasks;
using Kafka.OrderEventConsumer.Domain.Order;

namespace Kafka.OrderEventConsumer.Domain.Interfaces;

public interface IKafkaService
{
    Task OrderProcessing(OrderEvent orderEvent, CancellationToken token);
}