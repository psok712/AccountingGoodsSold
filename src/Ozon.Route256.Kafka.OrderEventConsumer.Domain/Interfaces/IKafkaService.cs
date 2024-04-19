using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Order;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;

public interface IKafkaService
{
    Task OrderProcessing(OrderEvent orderEvent, CancellationToken token);
}