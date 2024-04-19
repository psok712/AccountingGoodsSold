using AutoBogus;
using Bogus;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;

public static class SalesAddModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<SalesAddModel> Faker = new AutoFaker<SalesAddModel>()
        .RuleFor(x => x.SellerId, f => f.Random.Long(0L))
        .RuleFor(x => x.ItemId, f => f.Random.Long(0L))
        .RuleFor(x => x.Currency, f => f.Random.Word());

    public static SalesAddModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static SalesAddModel WithSellerId(
        this SalesAddModel src,
        long sellerId)
    {
        return src with { SellerId = sellerId };
    }

    public static SalesAddModel WithItemId(
        this SalesAddModel src,
        long itemId)
    {
        return src with { ItemId = itemId };
    }
}