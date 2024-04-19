using FluentAssertions;
using Moq;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;
using Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fixtures;
using Utils.Providers.Interfaces;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class ItemRepositoryTests
{
    private readonly Mock<IDateTimeOffsetProvider> _dateTimeOffsetProviderFake;
    private readonly IItemRepository _repository;

    public ItemRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.ItemRepository;
        _dateTimeOffsetProviderFake = fixture.DateTimeOffsetProviderFaker;
    }
    
    [Fact]
    public async Task AddIfNotExist_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First()
            .WithUpdatedAt(updatedTime);
        var itemId = expectedItem.ItemId;


        // Act
        await _repository.AddIfNotExist(itemId, default);


        // Asserts
        var item = await _repository.Get(itemId, default);
        item.ItemId.Should().Be(expectedItem.ItemId);
        item.Canceled.Should().Be(expectedItem.Canceled);
        item.Created.Should().Be(expectedItem.Created);
        item.Delivered.Should().Be(expectedItem.Delivered);
        Assert.True(Math.Abs(item.UpdatedAt.LocalDateTime.Ticks - expectedItem.UpdatedAt.UtcTicks) < 10);
    }

    [Fact]
    public async Task Get_AddItem_ShouldReturnThisItem()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First()
            .WithUpdatedAt(updatedTime);
        var itemId = expectedItem.ItemId;
        await _repository.AddIfNotExist(itemId, default);


        // Act
        var item = await _repository.Get(itemId, default);


        // Asserts
        item.ItemId.Should().Be(expectedItem.ItemId);
        item.Canceled.Should().Be(expectedItem.Canceled);
        item.Created.Should().Be(expectedItem.Created);
        item.Delivered.Should().Be(expectedItem.Delivered);
        Assert.True(Math.Abs(item.UpdatedAt.LocalDateTime.Ticks - expectedItem.UpdatedAt.UtcTicks) < 10);
    }


    [Fact]
    public async Task IncCreated_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First();
        var itemId = expectedItem.ItemId;
        await _repository.AddIfNotExist(itemId, default);


        // Act
        await _repository.IncCreated(itemId, default);


        // Asserts
        var item = await _repository.Get(itemId, default);
        item.Created.Should().Be(expectedItem.Created + 1);
    }

    [Fact]
    public async Task IncCanceled_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First();
        var itemId = expectedItem.ItemId;
        await _repository.AddIfNotExist(itemId, default);


        // Act
        await _repository.IncCanceled(itemId, default);


        // Asserts
        var item = await _repository.Get(itemId, default);
        item.Canceled.Should().Be(expectedItem.Canceled + 1);
        item.Created.Should().Be(expectedItem.Created - 1);
    }

    [Fact]
    public async Task IncDelivered_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First();
        var itemId = expectedItem.ItemId;
        await _repository.AddIfNotExist(itemId, default);


        // Act
        await _repository.IncDelivered(itemId, default);


        // Asserts
        var item = await _repository.Get(itemId, default);
        item.Delivered.Should().Be(expectedItem.Delivered + 1);
        item.Created.Should().Be(expectedItem.Created - 1);
    }
}