using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Client;
using WorkManagermentWeb.Client.Authentication;
using WorkManagermentWeb.Client.Interfaces;
using WorkManagermentWeb.Client.Models;
using WorkManagermentWeb.Client.Services;
using WorkManagermentWeb.Client.Ultilities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("./wwwroot/appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddBlazorBootstrap();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44313/") });
builder.Services.AddBlazoredLocalStorageAsSingleton();

//Add Localization
builder.Services.AddLocalization();

//Add AzureAD
builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/Calendars.ReadWrite");
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomAccountFactory>();

builder.Services
    .AddSingleton<IAuthService, AuthService>()
    .AddScoped<IBoard, BoardService>()
    .AddScoped<IUser, UserService>()
    .AddScoped<IWorkItem, WorkItemService>()
    .AddScoped<INotification, NotificationService>()
    .AddScoped<IGanttChart, GanttChartService>()
    .AddScoped<IMicrosoftCalendarService, MicrosoftCalendarService>()
    .AddScoped<ICalendar, CalendarService>();

var app = builder.Build();

//Add Localization
await app.SetDefaultCulture();
await app.RunAsync();
