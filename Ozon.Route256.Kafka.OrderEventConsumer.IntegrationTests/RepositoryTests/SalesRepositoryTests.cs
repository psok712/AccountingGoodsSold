using FluentAssertions;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Interfaces;
using Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fakers;
using Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.Fixtures;

namespace Ozon.Route256.Kafka.OrderEventConsumer.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class SalesRepositoryTests
{
    private readonly ISalesRepository _repository;

    public SalesRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.SalesRepository;
    }

    [Fact]
    public async Task AddIfNotExist_Success()
    {
        // Arrange
        var sale = SalesAddModelFaker.Generate().First();
        var expectedSale = SalesEntityV1Faker.Generate().First()
            .WithItemId(sale.ItemId)
            .WithSellerId(sale.SellerId)
            .WithCurrency(sale.Currency);


        // Act
        await _repository.AddIfNotExist(sale, default);


        // Asserts
        var getModel = SalesGetModelFaker.Generate().First()
            .WithItemId(sale.ItemId)
            .WithSellerId(sale.SellerId);
        var resultSale = await _repository.Get(getModel, default);
        resultSale.Should().BeEquivalentTo(expectedSale);
    }

    [Fact]
    public async Task Get_AddSale_ShouldReturnThisSale()
    {
        // Arrange
        var sale = SalesAddModelFaker.Generate().First();
        await _repository.AddIfNotExist(sale, default);
        var expectedSale = SalesEntityV1Faker.Generate().First()
            .WithItemId(sale.ItemId)
            .WithSellerId(sale.SellerId)
            .WithCurrency(sale.Currency);


        // Act
        var getModel = SalesGetModelFaker.Generate().First()
            .WithItemId(sale.ItemId)
            .WithSellerId(sale.SellerId);
        var resultSale = await _repository.Get(getModel, default);


        // Asserts
        resultSale.Should().BeEquivalentTo(expectedSale);
    }

    [Fact]
    public async Task IncSale_Success()
    {
        // Arrange
        var saleAdd = SalesAddModelFaker.Generate().First();
        await _repository.AddIfNotExist(saleAdd, default);


        // Act
        var salesEntity = SalesEntityV1Faker.Generate().First()
            .WithItemId(saleAdd.ItemId)
            .WithSellerId(saleAdd.SellerId);
        var expectedSales = salesEntity.Price;
        await _repository.IncSale(salesEntity, default);


        // Asserts
        var getModel = SalesGetModelFaker.Generate().First()
            .WithItemId(saleAdd.ItemId)
            .WithSellerId(saleAdd.SellerId);
        var sale = await _repository.Get(getModel, default);
        sale.Price.Should().Be(expectedSales);
    }
}