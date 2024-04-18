using AutoBogus;
using Bogus;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Entities;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;

public static class ItemEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<ItemEntityV1> Faker = new AutoFaker<ItemEntityV1>()
        .RuleFor(x => x.ItemId, f => f.Random.Long(0L))
        .RuleFor(x => x.Canceled, _ => 0L)
        .RuleFor(x => x.Created, _ => 0L)
        .RuleFor(x => x.Delivered, _ => 0L)
        .RuleFor(x => x.UpdatedAt, _ => DateTimeOffset.UtcNow);

    public static ItemEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static ItemEntityV1 WithItemId(
        this ItemEntityV1 src, 
        long itemId)
        => src with { ItemId = itemId };
    
    public static ItemEntityV1 WithCanceled(
        this ItemEntityV1 src, 
        long canceled)
        => src with { Canceled = canceled };
    
    public static ItemEntityV1 WithCreated(
        this ItemEntityV1 src, 
        long created)
        => src with { Created = created };
    
    public static ItemEntityV1 WithDelivered(
        this ItemEntityV1 src, 
        long delivered)
        => src with { Delivered = delivered };
    
    public static ItemEntityV1 WithUpdatedAt(
        this ItemEntityV1 src, 
        DateTimeOffset updatedAt)
        => src with { UpdatedAt = updatedAt};
}