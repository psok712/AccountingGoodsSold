﻿namespace Kafka.OrderEventConsumer.Domain.ValueObjects;

public readonly record struct Money(decimal Value, string Currency);