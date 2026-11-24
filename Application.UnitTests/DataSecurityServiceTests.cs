using Domain;
using Infrastructure.Repositories;
using Moq;

namespace Application.UnitTests;

public class DataSecurityServiceTests
{
    [Fact]
    public async Task GetByRoleIdsAsync_WhenRepositoryReturnsData_ReturnsActiveRecordsOnly()
    {
        // Arrange
        var roleIds = new List<int> { 1, 2 };

        var repositoryData = new List<DataSecurity>
        {
            new()
            {
                Id = 1,
                RoleId = 1,
                PermissionName = "ViewUsers",
                IsActive = true
            },
            new()
            {
                Id = 2,
                RoleId = 2,
                PermissionName = "EditUsers",
                IsActive = false
            },
            new()
            {
                Id = 3,
                RoleId = 1,
                PermissionName = "DeleteUsers",
                IsActive = true
            }
        };

        var repositoryMock = new Mock<IDataSecurityRepository>();

        repositoryMock
            .Setup(x => x.GetByRoleIdsAsync(
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(repositoryData);

        var service = new DataSecurityService(repositoryMock.Object);

        // Act
        var result = await service.GetByRoleIdsAsync(roleIds);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, record => Assert.True(record.IsActive));
        Assert.Contains(result, record => record.PermissionName == "ViewUsers");
        Assert.Contains(result, record => record.PermissionName == "DeleteUsers");
        Assert.DoesNotContain(result, record => record.PermissionName == "EditUsers");
    }

    [Fact]
    public async Task GetByRoleIdsAsync_WhenRoleIdsContainInvalidAndDuplicateIds_PassesOnlyValidDistinctIds()
    {
        // Arrange
        var roleIds = new List<int> { 1, 0, -2, 2, 1, 2 };

        var repositoryMock = new Mock<IDataSecurityRepository>();

        repositoryMock
            .Setup(x => x.GetByRoleIdsAsync(
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DataSecurity>());

        var service = new DataSecurityService(repositoryMock.Object);

        // Act
        await service.GetByRoleIdsAsync(roleIds);

        // Assert
        repositoryMock.Verify(
            x => x.GetByRoleIdsAsync(
                It.Is<IReadOnlyCollection<int>>(
                    ids => ids.SequenceEqual(new[] { 1, 2 })),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByRoleIdsAsync_WhenNoValidRoleIds_ReturnsEmptyWithoutCallingRepository()
    {
        // Arrange
        var roleIds = new List<int> { 0, -1, -5 };

        var repositoryMock = new Mock<IDataSecurityRepository>();
        var service = new DataSecurityService(repositoryMock.Object);

        // Act
        var result = await service.GetByRoleIdsAsync(roleIds);

        // Assert
        Assert.Empty(result);

        repositoryMock.Verify(
            x => x.GetByRoleIdsAsync(
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetByRoleIdsAsync_WhenRoleIdsAreEmpty_ReturnsEmptyWithoutCallingRepository()
    {
        // Arrange
        var repositoryMock = new Mock<IDataSecurityRepository>();
        var service = new DataSecurityService(repositoryMock.Object);

        // Act
        var result = await service.GetByRoleIdsAsync(
            Array.Empty<int>());

        // Assert
        Assert.Empty(result);

        repositoryMock.Verify(
            x => x.GetByRoleIdsAsync(
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetByRoleIdsAsync_WhenRoleIdsAreNull_ThrowsArgumentNullException()
    {
        // Arrange
        var repositoryMock = new Mock<IDataSecurityRepository>();
        var service = new DataSecurityService(repositoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => service.GetByRoleIdsAsync(null!));

        // Assert
        Assert.Equal("roleIds", exception.ParamName);

        repositoryMock.Verify(
            x => x.GetByRoleIdsAsync(
                It.IsAny<IReadOnlyCollection<int>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}