﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Kafka.OrderEventConsumer.Infrastructure.Kafka;

public interface IHandler<TKey, TValue>
{
    Task Handle(IReadOnlyCollection<ConsumeResult<TKey, TValue>> messages, CancellationToken token);
}