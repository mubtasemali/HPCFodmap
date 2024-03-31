using HPCFodmapProject.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using HPCFodmapProject.Client.HttpRepository;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE3MDUzNUAzMjM1MmUzMDJlMzBZcTBpOWxkWmM3aGg5RkRiMDIySlhwY2g4cDZ4Nm9oWkNSOTlNS3R1U25rPQ==");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("HPCFodmapProject.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HPCFodmapProject.ServerAPI"));

builder.Services.AddScoped<UserFoodDiaryHttpRepository>();
builder.Services.AddScoped<UserHttpRepository>();
builder.Services.AddApiAuthorization();
builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
