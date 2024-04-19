using AutoBogus;
using Bogus;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;

public static class SalesIncModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<SalesIncModel> Faker = new AutoFaker<SalesIncModel>()
        .RuleFor(x => x.SellerId, f => f.Random.Long(0L))
        .RuleFor(x => x.ItemId, f => f.Random.Long(0L))
        .RuleFor(x => x.Price, f => f.Random.Decimal());

    public static SalesIncModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static SalesIncModel WithSellerId(
        this SalesIncModel src,
        long sellerId)
    {
        return src with { SellerId = sellerId };
    }

    public static SalesIncModel WithItemId(
        this SalesIncModel src,
        long itemId)
    {
        return src with { ItemId = itemId };
    }

    public static SalesIncModel WithPrice(
        this SalesIncModel src,
        decimal price)
    {
        return src with { Price = price };
    }
}