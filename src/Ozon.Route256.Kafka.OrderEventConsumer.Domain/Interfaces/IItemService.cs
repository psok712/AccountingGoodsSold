using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.OrderProduce;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;

public interface IItemService
{
    Task OrderProcessing(OrderEventProduce orderEvent, CancellationToken token);
}