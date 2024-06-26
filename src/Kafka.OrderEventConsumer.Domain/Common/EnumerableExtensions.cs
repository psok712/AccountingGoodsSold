﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Kafka.OrderEventConsumer.Domain.Common;

public static class EnumerableExtensions
{
    public static TResult[] ToArrayBy<TSource, TResult>(this IEnumerable<TSource> collection,
        Func<TSource, TResult> selector)
    {
        return collection.Select(selector).ToArray();
    }
}