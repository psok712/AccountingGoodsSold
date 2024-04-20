using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;

public interface IItemRepository
{
    Task AddUpdateCreated(ItemUpdateModel updateModel, CancellationToken token);

    Task AddUpdateCanceled(ItemUpdateModel updateModel, CancellationToken token);

    Task AddUpdateDelivered(ItemUpdateModel updateModel, CancellationToken token);

    Task<ItemEntityV1> Get(long itemId, CancellationToken token);
}