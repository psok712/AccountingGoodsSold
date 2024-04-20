using FluentAssertions;
using Moq;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;
using Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fixtures;
using Utils.Providers.Interfaces;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class ItemRepositoryTests(TestFixture fixture)
{
    private readonly Mock<IDateTimeOffsetProvider> _dateTimeOffsetProviderFake = fixture.DateTimeOffsetProviderFaker;
    private readonly IItemRepository _repository = fixture.ItemRepository;

    [Fact]
    public async Task AddUpdateCreated_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First()
            .WithUpdatedAt(updatedTime);

        var updateModel = ItemUpdateModelFaker.Generate().First()
            .WithItemId(expectedItem.ItemId);


        // Act
        await _repository.AddUpdateCreated(updateModel, token: default);


        // Asserts
        var item = await _repository.Get(updateModel.ItemId, token: default);
        item.ItemId.Should().Be(expectedItem.ItemId);
        item.Canceled.Should().Be(expectedItem.Canceled);
        item.Created.Should().Be(expectedItem.Created + updateModel.Quantity);
        item.Delivered.Should().Be(expectedItem.Delivered);
        Assert.True(Math.Abs(item.UpdatedAt.LocalDateTime.Ticks - expectedItem.UpdatedAt.UtcTicks) < 10);
    }
    
    [Fact]
    public async Task AddUpdateCanceled_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First()
            .WithUpdatedAt(updatedTime);

        var updateModel = ItemUpdateModelFaker.Generate().First()
            .WithItemId(expectedItem.ItemId);


        // Act
        await _repository.AddUpdateCanceled(updateModel, token: default);


        // Asserts
        var item = await _repository.Get(updateModel.ItemId, token: default);
        item.ItemId.Should().Be(expectedItem.ItemId);
        item.Canceled.Should().Be(expectedItem.Canceled + updateModel.Quantity);
        item.Created.Should().Be(expectedItem.Created - updateModel.Quantity);
        item.Delivered.Should().Be(expectedItem.Delivered);
        Assert.True(Math.Abs(item.UpdatedAt.LocalDateTime.Ticks - expectedItem.UpdatedAt.UtcTicks) < 10);
    }

    [Fact]
    public async Task AddUpdateDelivered_Success()
    {
        // Arrange
        var updatedTime = DateTimeOffset.UtcNow;
        _dateTimeOffsetProviderFake
            .Setup(x => x.UtcNow)
            .Returns(updatedTime);

        var expectedItem = ItemEntityV1Faker.Generate().First()
            .WithUpdatedAt(updatedTime);

        var updateModel = ItemUpdateModelFaker.Generate().First()
            .WithItemId(expectedItem.ItemId);


        // Act
        await _repository.AddUpdateDelivered(updateModel, token: default);


        // Asserts
        var item = await _repository.Get(updateModel.ItemId, token: default);
        item.ItemId.Should().Be(expectedItem.ItemId);
        item.Canceled.Should().Be(expectedItem.Canceled);
        item.Created.Should().Be(expectedItem.Created - updateModel.Quantity);
        item.Delivered.Should().Be(expectedItem.Delivered + updateModel.Quantity);
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
        var updateModel = ItemUpdateModelFaker.Generate().First()
            .WithItemId(expectedItem.ItemId);
        await _repository.AddUpdateCreated(updateModel, token: default);


        // Act
        var item = await _repository.Get(updateModel.ItemId, token: default);


        // Asserts
        item.ItemId.Should().Be(expectedItem.ItemId);
        item.Canceled.Should().Be(expectedItem.Canceled);
        item.Created.Should().Be(expectedItem.Created + updateModel.Quantity);
        item.Delivered.Should().Be(expectedItem.Delivered);
        Assert.True(Math.Abs(item.UpdatedAt.LocalDateTime.Ticks - expectedItem.UpdatedAt.UtcTicks) < 10);
    }
}