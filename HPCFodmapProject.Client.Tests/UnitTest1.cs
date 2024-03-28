using HPCFodmapProject.Shared;
using HPCFodmapProject.Client.HttpRepository;
using System.Runtime.CompilerServices;
using Moq;
using RichardSzalay.MockHttp;
namespace HPCFodmapProject.Client.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Test1()
    {

        //arrage
        var mockHttp = new MockHttpMessageHandler();
        string testUserResonse = """
            fdgshfsdhsd
            """;
        Assert.Pass();
    }
}