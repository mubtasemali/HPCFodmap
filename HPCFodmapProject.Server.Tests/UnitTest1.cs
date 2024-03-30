using HPCFodmapProject.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HPCFodmapProject.Shared;
using HPCFodmapProject.Server.Services;
using HPCFodmapProject.Server.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using HPCFodmapProject.Server.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace HPCFodmapProject.Server.Tests;

public class Tests
{
    private readonly Mock<FoodController> _foodMock = new Mock<FoodController>();
    private readonly Mock<ApplicationDbContext> _contextMock= new Mock<ApplicationDbContext>();
    private readonly Mock<UserManager<ApplicationUser>> _userServiceMock = new Mock<UserManager<ApplicationUser>>();
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetUserFoodIntake()

    {

        // Arrange
        List<IntakeDto> intakeList = new List<IntakeDto>
{
    new IntakeDto
    {
        Food = "hotdog",
        notes = "bad",
        date = new DateTime(2024, 03, 25, 13, 19, 05, 391, DateTimeKind.Utc),
        harmful = true
    },
    new IntakeDto
    {
        Food = "hotdog",
        notes = "was ok",
        date = new DateTime(2024, 03, 28, 17, 21, 37, 242, DateTimeKind.Utc),
        harmful = true
    },
    new IntakeDto
    {
        Food = "cookies",
        notes = "was ok",
        date = new DateTime(2024, 03, 28, 17, 21, 40, 776, DateTimeKind.Utc),
        harmful = false
    },
    new IntakeDto
    {
        Food = "squash",
        notes = "was ok",
        date = new DateTime(2024, 03, 28, 17, 21, 44, 425, DateTimeKind.Utc),
        harmful = false
    },
    new IntakeDto
    {
        Food = "curry",
        notes = "gg",
        date = new DateTime(2024, 03, 28, 18, 46, 55, 901, DateTimeKind.Utc),
        harmful = false
    },
    new IntakeDto
    {
        Food = "grape",
        notes = "gg",
        date = new DateTime(2024, 03, 28, 18, 47, 08, 171, DateTimeKind.Utc),
        harmful = false
    },
    new IntakeDto
    {
        Food = "grape",
        notes = "gfdsgfsdg",
        date = new DateTime(2024, 03, 28, 18, 49, 09, 470, DateTimeKind.Utc),
        harmful = false
    }
};

        string? username = "m2@example.com";
        _foodMock.Setup(x => x.GetFoodIntake(username)).ReturnsAsync(intakeList);

        var controller = new FoodController(_contextMock.Object, _userServiceMock.Object);

        // Act
        var result = controller.GetFoodIntake(username);

        // Assert
        Assert.That(result.Result, Is.EqualTo(intakeList));
    }

    [Test]
    public void TestGetIngredients()
    {

        // Arrange
        var testResponse = @"[{""IngredientsName"":""spark"",""harmful"":true,""inFodMap"":false},{""IngredientsName"":""flour"",""harmful"":true,""inFodMap"":false},{""IngredientsName"":""tomato"",""harmful"":false,""inFodMap"":false},{""IngredientsName"":""yeast"",""harmful"":true,""inFodMap"":false}]";

        var ingredientData = JsonConvert.DeserializeObject<List<IngredientsDto>>(testResponse);
        List<IngredientsDto> ingredientList = new List<IngredientsDto>
{
    new IngredientsDto
    {
        IngredientsName = "spark",
        harmful = true,
        inFodMap = false
    },
    new IngredientsDto
    {
        IngredientsName = "flour",
        harmful = true,
        inFodMap = false
    },
    new IngredientsDto
    {
        IngredientsName = "tomato",
        harmful = false,
        inFodMap = false
    },
    new IngredientsDto
    {
        IngredientsName = "yeast",
        harmful = true,
        inFodMap = false
    }
};


        string? username = "m2@example.com";
        string? ingredient = "hotdog";
        _foodMock.Setup(x => x.GetIngredients(username, ingredient)).ReturnsAsync(ingredientList);
        //Act
        var controller = new FoodController(_contextMock.Object, _userServiceMock.Object);
        var result = controller.GetIngredients(username, ingredient);
        //Assert
        Assert.That(result.Result, Is.EqualTo(ingredientList));

    }
    [Test]
    public void TestUpdateWhiteList()
    {
        // Arrange
        //var testResponse = { };
 }
    
}