using Kooliprojekt.BlazorApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KooliProjekt.PublicApi;
using KooliProjekt.PublicApi.Api;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient nüüd suunatud sinu API-le
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://localhost:7282/api/") }
);

builder.Services.AddScoped<IApiClient, ApiClient>();

await builder.Build().RunAsync();
