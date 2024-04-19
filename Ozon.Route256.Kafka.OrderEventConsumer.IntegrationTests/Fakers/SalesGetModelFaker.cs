using AutoBogus;
using Bogus;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;

public static class SalesGetModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<SalesGetModel> Faker = new AutoFaker<SalesGetModel>()
        .RuleFor(x => x.SellerId, f => f.Random.Long(0L))
        .RuleFor(x => x.ItemId, f => f.Random.Long(0L));

    public static SalesGetModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static SalesGetModel WithSellerId(
        this SalesGetModel src,
        long sellerId)
    {
        return src with { SellerId = sellerId };
    }

    public static SalesGetModel WithItemId(
        this SalesGetModel src,
        long itemId)
    {
        return src with { ItemId = itemId };
    }
}