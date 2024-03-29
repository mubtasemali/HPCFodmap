using HPCFodmapProject.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Services;
using HPCFodmapProject.Server.Models;
using Moq;

namespace HPCFodmapProject.Server.Tests;

public class Tests
{
    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetUserFoodIntake()
    {
        // Arrange
        
        var controller = new FoodIntakeController(logger.Object, _userServiceMock.Object);

        // Act
        var result = controller.GetUserFoodIntake("test");

        // Assert
        Assert.IsNotNull(result);
    }
}