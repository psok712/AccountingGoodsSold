using AutoBogus;
using Bogus;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Models;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;

public static class ItemUpdateModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<ItemUpdateModel> Faker = new AutoFaker<ItemUpdateModel>()
        .RuleFor(x => x.ItemId, f => f.Random.Long(0L))
        .RuleFor(x => x.Quantity, f => f.Random.Long(0L));

    public static ItemUpdateModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static ItemUpdateModel WithItemId(
        this ItemUpdateModel src,
        long itemId)
    {
        return src with { ItemId = itemId };
    }

    public static ItemUpdateModel WithQuantity(
        this ItemUpdateModel src,
        long quantity)
    {
        return src with { Quantity = quantity };
    }
}