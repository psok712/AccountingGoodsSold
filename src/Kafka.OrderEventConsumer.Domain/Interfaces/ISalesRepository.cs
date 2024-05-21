using System.Threading;
using System.Threading.Tasks;
using Kafka.OrderEventConsumer.Domain.Entities;
using Kafka.OrderEventConsumer.Domain.Models;

namespace Kafka.OrderEventConsumer.Domain.Interfaces;

public interface ISalesRepository
{
    Task AddUpdateSale(SalesEntityV1 sale, long quantity, CancellationToken token);

    Task<SalesEntityV1> Get(SalesGetModel sale, CancellationToken token);
}