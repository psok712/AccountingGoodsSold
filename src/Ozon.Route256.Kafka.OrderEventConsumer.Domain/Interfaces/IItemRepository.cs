using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;

public interface IItemRepository
{
    Task Add(ItemEntityV1 item, CancellationToken token);

    Task<ItemEntityV1> Get(long itemId, CancellationToken token);

    Task Update(ItemEntityV1 updateModel, CancellationToken token);
}