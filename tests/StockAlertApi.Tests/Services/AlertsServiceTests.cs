using Xunit;
using Moq;
using FluentAssertions;
using StockAlertApi.Application.Services;
using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Enums;
using StockAlertApi.Core.Interfaces.Repositories;

namespace StockAlertApi.Tests.Services;

public class AlertsServiceTests
{
    [Fact]
    public async Task CreateAlertAsync_WhenDuplicateExists_ReturnsNull()
    {
        var mockRepo = new Mock<IAlertRepository>();
        var userId = Guid.NewGuid();
        var stockId = Guid.NewGuid();

        mockRepo.Setup(r => r.GetUserAlertsAsync(userId))
            .ReturnsAsync(new List<Alert>
            {
                new Alert
                {
                    StockId = stockId,
                    TargetPrice = 100m,
                    Direction = AlertDirection.Above,
                    Status = AlertStatus.Active
                }
            });

        var service = new AlertsService(mockRepo.Object);

        var result = await service.CreateAlertAsync(
            userId, stockId, 100m, AlertDirection.Above);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAlertAsync_WhenNoDuplicate_CreatesAlert()
    {
        var mockRepo = new Mock<IAlertRepository>();
        var userId = Guid.NewGuid();
        var stockId = Guid.NewGuid();

        mockRepo.Setup(r => r.GetUserAlertsAsync(userId))
            .ReturnsAsync(new List<Alert>());

        var service = new AlertsService(mockRepo.Object);

        var result = await service.CreateAlertAsync(
            userId, stockId, 100m, AlertDirection.Above);

        result.Should().NotBeNull();
        result!.TargetPrice.Should().Be(100m);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public async Task CreateAlertAsync_WhenSameStockButDifferentDirection_CreatesAlert()
    {
        var mockRepo = new Mock<IAlertRepository>();
        var userId = Guid.NewGuid();
        var stockId = Guid.NewGuid();

        mockRepo.Setup(r => r.GetUserAlertsAsync(userId))
            .ReturnsAsync(new List<Alert>
            {
                new Alert
                {
                    StockId = stockId,
                    TargetPrice = 100m,
                    Direction = AlertDirection.Above,
                    Status = AlertStatus.Active
                }
            });

        var service = new AlertsService(mockRepo.Object);

        var result = await service.CreateAlertAsync(
            userId, stockId, 100m, AlertDirection.Below);

        result.Should().NotBeNull();
        result!.Direction.Should().Be(AlertDirection.Below);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public async Task CreateAlertAsync_WhenSameUserButDifferentStock_CreatesAlert()
    {
        var mockRepo = new Mock<IAlertRepository>();
        var userId = Guid.NewGuid();
        var existingStockId = Guid.NewGuid();
        var newStockId = Guid.NewGuid();

        mockRepo.Setup(r => r.GetUserAlertsAsync(userId))
            .ReturnsAsync(new List<Alert>
            {
                new Alert
                {
                    StockId = existingStockId,
                    TargetPrice = 100m,
                    Direction = AlertDirection.Above,
                    Status = AlertStatus.Active
                }
            });

        var service = new AlertsService(mockRepo.Object);

        var result = await service.CreateAlertAsync(
            userId, newStockId, 100m, AlertDirection.Above);

        result.Should().NotBeNull();
        result!.StockId.Should().Be(newStockId);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public async Task CreateAlertAsync_WhenExistingAlertIsInactive_CreatesAlert()
    {
        var mockRepo = new Mock<IAlertRepository>();
        var userId = Guid.NewGuid();
        var stockId = Guid.NewGuid();

        mockRepo.Setup(r => r.GetUserAlertsAsync(userId))
            .ReturnsAsync(new List<Alert>
            {
                new Alert
                {
                    StockId = stockId,
                    TargetPrice = 100m,
                    Direction = AlertDirection.Above,
                    Status = AlertStatus.Triggered
                }
            });

        var service = new AlertsService(mockRepo.Object);

        var result = await service.CreateAlertAsync(
            userId, stockId, 100m, AlertDirection.Above);

        result.Should().NotBeNull();
        result!.Status.Should().Be(AlertStatus.Active);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Alert>()), Times.Once);
    }
}