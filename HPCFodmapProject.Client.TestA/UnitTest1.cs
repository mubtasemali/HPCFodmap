using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HPCFodmapProject.Client.HttpRepository;
using RichardSzalay.MockHttp;
using NUnit.Framework;
using System.Collections.Generic;
using HPCFodmapProject.Shared; // Add this line

namespace HPCFodmapProject.Client.TestA
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task TestGetFoodintakeHomePage()
        {
            //arrange
            var mockHttp = new MockHttpMessageHandler();
            var testResponse = @"[{""food"":""hotdog"",""notes"":""bad"",""date"":""2024-03-25T13:15:47.8650963"",""harmful"":true},{""food"":""hotdog"",""notes"":""bad"",""date"":""2024-03-25T13:19:05.3917633"",""harmful"":true},{""food"":""hotdog"",""notes"":""was ok"",""date"":""2024-03-28T17:21:37.2425487"",""harmful"":true},{""food"":""cookies"",""notes"":""was ok"",""date"":""2024-03-28T17:21:40.7761023"",""harmful"":false},{""food"":""squash"",""notes"":""was ok"",""date"":""2024-03-28T17:21:44.425102"",""harmful"":false},{""food"":""curry"",""notes"":""was ok"",""date"":""2024-03-28T17:21:48.7910556"",""harmful"":false}]";

            mockHttp.When("https://localhost:7192/api/getUserFoodIntake?username=m2@example.com")
                .Respond("application/json", testResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://localhost:7192");

            var userFoodIntakeRepo = new UserFoodDiaryHttpRepository(client);

            //Act
            var response = await userFoodIntakeRepo.GetFoodIntake("m2@example.com");

            //Assert
            var foodData = JsonConvert.DeserializeObject<List<IntakeDto>>(testResponse);
            Assert.That(response[0].Food, Is.EqualTo(foodData[0].Food));
            Assert.That(response[1].Food, Is.EqualTo(foodData[1].Food));
            Assert.That(response[2].Food, Is.EqualTo(foodData[2].Food));
            Assert.That(response[3].Food, Is.EqualTo(foodData[3].Food));
            Assert.That(response[4].Food, Is.EqualTo(foodData[4].Food));
            Assert.That(response[0].harmful, Is.EqualTo(foodData[0].harmful));
            Assert.That(response[1].harmful, Is.EqualTo(foodData[1].harmful));
            Assert.That(response[2].harmful, Is.EqualTo(foodData[2].harmful));
            Assert.That(response[3].harmful, Is.EqualTo(foodData[3].harmful));
            Assert.That(response[4].harmful, Is.EqualTo(foodData[4].harmful));
            Assert.That(response[0].date, Is.EqualTo(foodData[0].date));
            Assert.That(response[1].date, Is.EqualTo(foodData[1].date));
            Assert.That(response[2].date, Is.EqualTo(foodData[2].date));
            Assert.That(response[3].date, Is.EqualTo(foodData[3].date));
            Assert.That(response[4].date, Is.EqualTo(foodData[4].date));
            Assert.That(response[0].notes, Is.EqualTo(foodData[0].notes));
            Assert.That(response[1].notes, Is.EqualTo(foodData[1].notes));
            Assert.That(response[2].notes, Is.EqualTo(foodData[2].notes));
            Assert.That(response[3].notes, Is.EqualTo(foodData[3].notes));
            Assert.That(response[4].notes, Is.EqualTo(foodData[4].notes));
            Assert.That(response.Count, Is.EqualTo(6));

        }

        [Test]
        public async Task TestGetIngredients()
        {
            //arrange
            var mockHttp = new MockHttpMessageHandler();
            var testResponse = @"[{""IngredientsName"":""flour"",""harmful"":true,""inFodMap"":false},{""IngredientsName"":""tomato"",""harmful"":false,""inFodMap"":false},{""IngredientsName"":""yeast"",""harmful"":true,""inFodMap"":false}]"; ;
            mockHttp.When("https://localhost:7192/api/getIngredients?foodName=burgers&username=m2%40example.com").Respond("application/json", testResponse);
            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://localhost:7192");
            var userFoodIntakeRepo = new UserFoodDiaryHttpRepository(client);
            //act
            var response = await userFoodIntakeRepo.GetIngredients("burgers", "m2@example.com");
            //assert
            var ingredientData = JsonConvert.DeserializeObject<List<IngredientsDto>>(testResponse);
            Assert.That(response[0].IngredientsName, Is.EqualTo(ingredientData[0].IngredientsName));
            Assert.That(response[1].IngredientsName, Is.EqualTo(ingredientData[1].IngredientsName));
            Assert.That(response[2].IngredientsName, Is.EqualTo(ingredientData[2].IngredientsName));
            Assert.That(response[0].harmful, Is.EqualTo(ingredientData[0].harmful));
            Assert.That(response[1].harmful, Is.EqualTo(ingredientData[1].harmful));
            Assert.That(response[2].harmful, Is.EqualTo(ingredientData[2].harmful));
            Assert.That(response[0].inFodMap, Is.EqualTo(ingredientData[0].inFodMap));
            Assert.That(response[1].inFodMap, Is.EqualTo(ingredientData[1].inFodMap));
            Assert.That(response[2].inFodMap, Is.EqualTo(ingredientData[2].inFodMap));
            Assert.That(response.Count, Is.EqualTo(3));


        }

        public async Task TestGetFoodIntake()
        {
            //arrange
            var mockHttp = new MockHttpMessageHandler();
            var testResponse = @"[{""food"":""hotdog"",""notes"":""bad"",""date"":""2024-03-25T13:15:47.8650963"",""harmful"":true},{""food"":""hotdog"",""notes"":""bad"",""date"":""2024-03-25T13:19:05.3917633"",""harmful"":true},{""food"":""burgers"",""notes"":""was ok"",""date"":""2024-03-28T17:21:28.4438481"",""harmful"":false},{""food"":""hotdog"",""notes"":""was ok"",""date"":""2024-03-28T17:21:37.2425487"",""harmful"":true},{""food"":""cookies"",""notes"":""was ok"",""date"":""2024-03-28T17:21:40.7761023"",""harmful"":false},{""food"":""squash"",""notes"":""was ok"",""date"":""2024-03-28T17:21:44.425102"",""harmful"":false},{""food"":""curry"",""notes"":""was ok"",""date"":""2024-03-28T17:21:48.7910556"",""harmful"":false}]";

            mockHttp.When("https://localhost:")
                .Respond("application/json", testResponse);


        }

    }
}