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
        // Arrange
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

        // Act
        var result = await service.CreateAlertAsync(
            userId, stockId, 100m, AlertDirection.Above);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAlertAsync_WhenNoDuplicate_CreatesAlert()
    {
        // Arrange
        var mockRepo = new Mock<IAlertRepository>();
        var userId = Guid.NewGuid();
        var stockId = Guid.NewGuid();

        mockRepo.Setup(r => r.GetUserAlertsAsync(userId))
            .ReturnsAsync(new List<Alert>());

        var service = new AlertsService(mockRepo.Object);

        // Act
        var result = await service.CreateAlertAsync(
            userId, stockId, 100m, AlertDirection.Above);

        // Assert
        result.Should().NotBeNull();
        result!.TargetPrice.Should().Be(100m);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Alert>()), Times.Once);
    }
}