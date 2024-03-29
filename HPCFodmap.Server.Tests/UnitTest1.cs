//using HPCFodmap.Server;
using HPCFodmapProject.Server.Controllers;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RichardSzalay.MockHttp;




namespace HPCFodmapProject.Server.Tests;

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

        Assert.Pass();
    }
}