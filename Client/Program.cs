using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using VehicleMonitoringSystem.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<VehicleMonitoringSystem.Client.ConDataService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("VehicleMonitoringSystem.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("VehicleMonitoringSystem.Server"));
builder.Services.AddScoped<VehicleMonitoringSystem.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, VehicleMonitoringSystem.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();