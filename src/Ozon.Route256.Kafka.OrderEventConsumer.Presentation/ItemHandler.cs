using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.OrderProduce;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public class ItemHandler : IHandler<long, OrderEventProduce>
{
    private readonly ILogger<ItemHandler> _logger;
    private readonly IItemService _service;

    public ItemHandler(ILogger<ItemHandler> logger, IItemService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<long, OrderEventProduce>> messages,
        CancellationToken token)
    {
        var tasks = messages
            .Select(async m => await _service.OrderProcessing(m.Message.Value, token))
            .ToList();
        await Task.WhenAll(tasks);

        _logger.LogInformation("Handled {Count} messages", messages.Count);
    }
}