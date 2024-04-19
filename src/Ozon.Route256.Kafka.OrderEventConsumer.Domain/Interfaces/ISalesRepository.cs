using System.Threading;
using System.Threading.Tasks;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;

public interface ISalesRepository
{
    Task AddIfNotExist(SalesAddModel sale, CancellationToken token);

    Task IncSale(SalesIncModel sale, CancellationToken token);

    Task<SalesEntityV1> Get(SalesGetModel sale, CancellationToken token);
}