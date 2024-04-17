using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain;

public interface IItemRepository
{
    Task<long> Add(ItemEntityV1 item, CancellationToken token);
    
    Task<ItemEntityV1> Get(long itemId, CancellationToken token);

    Task Update(ItemUpdateModel updateModel, CancellationToken token);
}
