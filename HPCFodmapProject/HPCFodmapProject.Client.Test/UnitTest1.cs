using HPCFodmapProject.Client.HttpRepository;
using RichardSzalay.MockHttp;
namespace HPCFodmapProject.Client.Test;
using NUnit.Framework;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Diagram;
using Syncfusion.ExcelExport;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HPCFodmapProject.Shared;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test_GetFoodintakeHomePage()
    {
        //arrange
        var mockHttp = new MockHttpMessageHandler();
        var testResponse =  "\\[{\\\"food\\\":\\\"hotdog\\\",\\\"notes\\\":\\\"bad\\\",\\\"date\\\":\\\"2024-03-25T13:15:47.8650963\\\",\\\"harmful\\\":true},{\\\"food\\\":\\\"hotdog\\\",\\\"notes\\\":\\\"bad\\\",\\\"date\\\":\\\"2024-03-25T13:19:05.3917633\\\",\\\"harmful\\\":true},{\\\"food\\\":\\\"burgers\\\",\\\"notes\\\":\\\"was ok\\\",\\\"date\\\":\\\"2024-03-28T17:21:28.4438481\\\",\\\"harmful\\\":false},{\\\"food\\\":\\\"hotdog\\\",\\\"notes\\\":\\\"was ok\\\",\\\"date\\\":\\\"2024-03-28T17:21:37.2425487\\\",\\\"harmful\\\":true},{\\\"food\\\":\\\"cookies\\\",\\\"notes\\\":\\\"was ok\\\",\\\"date\\\":\\\"2024-03-28T17:21:40.7761023\\\",\\\"harmful\\\":false},{\\\"food\\\":\\\"squash\\\",\\\"notes\\\":\\\"was ok\\\",\\\"date\\\":\\\"2024-03-28T17:21:44.425102\\\",\\\"harmful\\\":false},{\\\"food\\\":\\\"curry\\\",\\\"notes\\\":\\\"was ok\\\",\\\"date\\\":\\\"2024-03-28T17:21:48.7910556\\\",\\\"harmful\\\":false}\\]";
        //assert
        mockHttp.When("https://localhost:7192/api/getUserFoodIntake?username=m2@example.com")
        .Respond("application/json", testResponse);
        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://localhost:7176/");
        var userFoodIntakeRepo = new UserFoodDiaryHttpRepository(client);
        //Act
        var response = await userFoodIntakeRepo.GetIngredients("m2@example.com");
        var actual = JsonConvert.DeserializeObject<string>(testResponse);
        //var foodData = JsonConvert.DeserializeObject<List<JObject>>(unescapedString);
        //var actual = response[1].
        //Assert
        //Assert.That(foodData[0]["food"], Is.EqualTo(response[0].Food);

    }
}