using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;

public interface IItemRepository
{
    Task AddIfNotExist(long itemId, CancellationToken token);

    Task IncCreated(long itemId, CancellationToken token);

    Task IncCanceled(long itemId, CancellationToken token);

    Task IncDelivered(long itemId, CancellationToken token);

    Task<ItemEntityV1> Get(long itemId, CancellationToken token);
}