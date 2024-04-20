using AutoBogus;
using Bogus;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;

public static class SalesEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<SalesEntityV1> Faker = new AutoFaker<SalesEntityV1>()
        .RuleFor(x => x.SellerId, f => f.Random.Long(0L))
        .RuleFor(x => x.ItemId, f => f.Random.Long(0L))
        .RuleFor(x => x.Currency, f => f.Random.Word())
        .RuleFor(x => x.Price, _ => 0L);

    public static SalesEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static SalesEntityV1 WithSellerId(
        this SalesEntityV1 src,
        long sellerId)
    {
        return src with { SellerId = sellerId };
    }

    public static SalesEntityV1 WithItemId(
        this SalesEntityV1 src,
        long itemId)
    {
        return src with { ItemId = itemId };
    }

    public static SalesEntityV1 WithCurrency(
        this SalesEntityV1 src,
        string currency)
    {
        return src with { Currency = currency };
    }

    public static SalesEntityV1 WithSales(
        this SalesEntityV1 src,
        long sales)
    {
        return src with { Price = sales };
    }
}