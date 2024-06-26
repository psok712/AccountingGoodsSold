using FluentAssertions;
using Kafka.OrderEventConsumer.IntegrationTests.Fakers;
using Kafka.OrderEventConsumer.IntegrationTests.Fixtures;
using Kafka.OrderEventConsumer.Domain.Interfaces;

namespace Kafka.OrderEventConsumer.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class SalesRepositoryTests(TestFixture fixture)
{
    private readonly ISalesRepository _repository = fixture.SalesRepository;

    [Fact]
    public async Task AddUpdateSale_Success()
    {
        // Arrange
        const long quantity = 5;
        var sale = SalesEntityV1Faker.Generate().First();


        // Act
        await _repository.AddUpdateSale(sale, quantity, default);


        // Asserts
        var getModel = SalesGetModelFaker.Generate().First()
            .WithItemId(sale.ItemId)
            .WithSellerId(sale.SellerId);
        var resultSale = await _repository.Get(getModel, default);
        resultSale.ItemId.Should().Be(sale.ItemId);
        resultSale.Price.Should().Be(sale.Price * quantity);
        resultSale.SellerId.Should().Be(sale.SellerId);
        resultSale.Currency.Should().Be(sale.Currency);
    }

    [Fact]
    public async Task Get_AddSale_ShouldReturnThisSale()
    {
        // Arrange
        const long quantity = 5;
        var sale = SalesEntityV1Faker.Generate().First();
        await _repository.AddUpdateSale(sale, quantity, default);


        // Act
        var getModel = SalesGetModelFaker.Generate().First()
            .WithItemId(sale.ItemId)
            .WithSellerId(sale.SellerId);
        var resultSale = await _repository.Get(getModel, default);


        // Asserts
        resultSale.Should().BeEquivalentTo(sale);
        resultSale.ItemId.Should().Be(sale.ItemId);
        resultSale.Price.Should().Be(sale.Price * quantity);
        resultSale.SellerId.Should().Be(sale.SellerId);
        resultSale.Currency.Should().Be(sale.Currency);
    }
}