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
    private readonly Mock<IFoodService> _foodMock = new Mock<IFoodService>();
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

        var controller = new FoodController(_foodMock.Object);

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
        var controller = new FoodController(_foodMock.Object);
        var result = controller.GetIngredients(username, ingredient);
        //Assert
        Assert.That(result.Result, Is.EqualTo(ingredientList));

    }
    [Test]
    public void TestUpdateWhiteList()
    {
        // Arrange
        string? username = "m2@example.com";
        string? IngName = "spark";
        _foodMock.Setup(x => x.updateWhiteList(username, IngName)).ReturnsAsync(true);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.updateWhiteList(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

        // Arrange
        username = "M15@EXAMPLE.COM";
        IngName = " spark";
        _foodMock.Setup(x => x.updateWhiteList(username, IngName)).ReturnsAsync(false);
        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.updateWhiteList(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));

        // Arrange
        username = "m2@example.com";
        IngName = "sparks";
        _foodMock.Setup(x => x.updateWhiteList(username, IngName)).ReturnsAsync(false);
        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.updateWhiteList(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));
        // Arrange
        username = "m2@example.com";
        IngName = "spark";
        _foodMock.Setup(x => x.updateWhiteList(username, IngName)).ReturnsAsync(true);
        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.updateWhiteList(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));


    }
    [Test]
    public void TestAddFoodIntake()
    {
        // Arrange
        string? username = "m1@example.com";
        string? foodName = "hotdog";
        string? notes = "really bad";
        _foodMock.Setup(x => x.AddFoodIntake(username, foodName, notes)).ReturnsAsync(true);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.AddFoodIntake(username, foodName, notes);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));


        // Arrange
        username = "M15@example.com";
        foodName = "hotdog";
        notes = "really bad";
        _foodMock.Setup(x => x.AddFoodIntake(username, foodName, notes)).ReturnsAsync(false);
        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.AddFoodIntake(username, foodName, notes);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));

        // Arrange
        username = "m1@example.com";
        foodName = "hhhhhhhhhhhhhhhhhhhhh";
        notes = "really bad";
        _foodMock.Setup(x => x.AddFoodIntake(username, foodName, notes)).ReturnsAsync(false);

        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.AddFoodIntake(username, foodName, notes);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));


    }
    [Test]
    public void TestDeleteFoodIntake()
    {
        // Arrange
        string? username = "m@example.com";
        DeleteIntakeDto intake = new DeleteIntakeDto
        {
            Food = "squash",
            notes = "was ok",
            date = new DateTime(2024, 03, 28, 17, 21, 44, 425, DateTimeKind.Utc)

        };

        _foodMock.Setup(x => x.DeleteFoodIntake(username, intake)).ReturnsAsync(true);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.DeleteFoodIntake(username, intake);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

        // Arrange
        username = "m15@example.com";
        intake = new DeleteIntakeDto
        {
            Food = "squash",
            notes = "was ok",
            date = new DateTime(2024, 03, 28, 17, 21, 44, 425, DateTimeKind.Utc)

        };
        //Act
        _foodMock.Setup(x => x.DeleteFoodIntake(username, intake)).ReturnsAsync(false);
        controller = new FoodController(_foodMock.Object);
        result = controller.DeleteFoodIntake(username, intake);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));


    }
    [Test]
    public void TestGetWhiteList()
    {
        // Arrange
        string? username = "m2@example.com";
        List<WTDto> whiteList = new List<WTDto>
        { new WTDto
        {
            ingredient = "eggs"
        },
        new WTDto
        {
            ingredient = "spark"
        } };

        _foodMock.Setup(x => x.GetWhiteList(username)).ReturnsAsync(whiteList);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.GetWhiteList(username);
        //Assert
        Assert.That(result.Result, Is.EqualTo(whiteList));
        Assert.That(result.Result.Count, Is.EqualTo(2));
        Assert.That(result.Result[0].ingredient, Is.EqualTo("eggs"));
        Assert.That(result.Result[1].ingredient, Is.EqualTo("spark"));
    }
    [Test]
    public void TestUpdateIntake()
    {
        // Arrange
        string? username = "m2@example.com";
        IntakeDto intake = new IntakeDto
        {
            Food = "hotdog",
            notes = "garbage",
            date = new DateTime(2024, 03, 25, 13, 19, 05, 391, DateTimeKind.Utc),
            harmful = true
        };
        _foodMock.Setup(x => x.UpdateIntake(username, intake)).ReturnsAsync(true);

        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.UpdateIntake(username, intake);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

        // Arrange
        username = "m2@example.com";
        intake = new IntakeDto
        {
            Food = "hotdog",
            notes = "bad",
            date = new DateTime(2024, 03, 25, 13, 19, 05, 391, DateTimeKind.Utc),
            harmful = true
        };
        _foodMock.Setup(x => x.UpdateIntake(username, intake)).ReturnsAsync(true);

    //Act   
    controller = new FoodController(_foodMock.Object);
        result = controller.UpdateIntake(username, intake);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

        // Arrange
        username = "m15@username.com";
        intake = new IntakeDto
        {
            Food = "hotdog",
            notes = "bad",
            date = new DateTime(2024, 03, 25, 13, 19, 05, 391, DateTimeKind.Utc),
            harmful = true
        };
        _foodMock.Setup(x => x.UpdateIntake(username, intake)).ReturnsAsync(false);
        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.UpdateIntake(username, intake);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));

    }
    [Test]
    public void TestGetUserFlagged()
    {
        // Arrange
        string? username = "m2@example.com";
        List<FlaggedDto> flaggedList = new List<FlaggedDto>
{
    new FlaggedDto
    {
        ingredient = "cheese",
        issues = "bad",
        lastEaten = DateTime.Parse("2024-03-25T13:15:47.8650963")
    },
    new FlaggedDto
    {
        ingredient = "chilli",
        issues = "bad",
        lastEaten = DateTime.Parse("2024-03-25T13:15:47.8650963")
    }
};
        _foodMock.Setup(x => x.GetUserFlagged(username)).ReturnsAsync(flaggedList);
            //Act
            var controller = new FoodController(_foodMock.Object);
            var result = controller.GetUserFlagged(username);
            //Assert
            Assert.That(result.Result, Is.EqualTo(flaggedList));
            Assert.That(result.Result.Count, Is.EqualTo(2));
        Assert.That(result.Result[0].ingredient, Is.EqualTo("cheese"));
        Assert.That(result.Result[1].ingredient, Is.EqualTo("chilli"));
    }
    [Test]
    public void TestUpdateFlagged()
    {
        // Arrange
        string? username = "m1@example.com";
        string? IngName = "cheese";
        _foodMock.Setup(x => x.UpdateFlagged(username, IngName)).ReturnsAsync(true);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.UpdateFlagged(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

        // Arrange
        username = "m1@example.com";
        IngName = "cheese";
        _foodMock.Setup(x => x.UpdateFlagged(username, IngName)).ReturnsAsync(true);
        //Act
        controller = new FoodController(_foodMock.Object);
        result = controller.UpdateFlagged(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

        // Arrange
        username = "m15@example.com";
        IngName = "cheese";
        _foodMock.Setup(x => x.UpdateFlagged(username, IngName)).ReturnsAsync(false);
        //Act
         controller = new FoodController(_foodMock.Object);
        result = controller.UpdateFlagged(username, IngName);
        //Assert
        Assert.That(result.Result, Is.EqualTo(false));
    }
    [Test]
    public void TestGetFodmap()
    {
        // Arrange
        List<WTDto> fodmapList = new List<WTDto>
{
    new WTDto { ingredient = "spark" },
    new WTDto { ingredient = "cheese" },
    new WTDto { ingredient = "flour" },
    new WTDto { ingredient = "yeast" },
    new WTDto { ingredient = "chilli" },
    new WTDto { ingredient = "milk" }
};
        _foodMock.Setup(x => x.GetFodmap()).ReturnsAsync(fodmapList);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.GetFodmap();
        //Assert
        Assert.That(result.Result.Count, Is.EqualTo(6));
        Assert.That(result.Result[0].ingredient, Is.EqualTo("spark"));
        Assert.That(result.Result[1].ingredient, Is.EqualTo("cheese"));
        Assert.That(result.Result[2].ingredient, Is.EqualTo("flour"));
        Assert.That(result.Result[3].ingredient, Is.EqualTo("yeast"));
        Assert.That(result.Result[4].ingredient, Is.EqualTo("chilli"));
        Assert.That(result.Result[5].ingredient, Is.EqualTo("milk"));

        

    }
    [Test]
    public void TestAddFodmap()
    {
        // Arrange
        string? ingredient = "agave";
        _foodMock.Setup(x => x.AddFodmap(ingredient)).ReturnsAsync(true);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.AddFodmap(ingredient);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));

    }
    [Test]
    public void TestDeleteFodmap()
    {
        // Arrange
        string? ingredient = "agave";
        _foodMock.Setup(x => x.DeleteFodmap(ingredient)).ReturnsAsync(true);
        //Act
        var controller = new FoodController(_foodMock.Object);
        var result = controller.DeleteFodmap(ingredient);
        //Assert
        Assert.That(result.Result, Is.EqualTo(true));
    }
   


    
}