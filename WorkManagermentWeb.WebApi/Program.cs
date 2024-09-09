using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.Net.Http.Headers;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Background.DependencyInjection;
using WorkManagermentWeb.Background.Options;
using WorkManagermentWeb.Background.Sevices;
using WorkManagermentWeb.Infrastructure.DependencyInjection;
using WorkManagermentWeb.Infrastructure.Jobs;
using WorkManagermentWeb.Infrastructure.Options;
using WorkManagermentWeb.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwagger();

builder.Services.InfrastructureServices(builder.Configuration);
//Add Background Job Service
BackgroundJobOptions bgJOptions =
    builder.Configuration.GetSection(BackgroundConstants.BackgroundConfigName).Get<BackgroundJobOptions>()!;
builder.Services.BackgroundJobServices(bgJOptions.ConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YourAPI v1"));
    app.UseCors(policy =>
    {
        policy.WithOrigins("http://localhost:7254", "https://localhost:7254")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders(HeaderNames.ContentType);
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

//Add Hangfire Dashboard
app.UseHangfireDashboard(bgJOptions.HangfireRoute, new DashboardOptions
{
    DashboardTitle = bgJOptions.HangfireTitle,
    DisplayStorageConnectionString = false,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = bgJOptions.Account.UserName,
            Pass = bgJOptions.Account.Password
        }
    }
});
RemindNearingDuesOptions options = builder.Configuration.GetSection(nameof(RemindNearingDuesOptions))
    .Get<RemindNearingDuesOptions>()!;

using var scope = app.Services.CreateScope();
var backgroundService = scope.ServiceProvider.GetRequiredService<IBackground>();

backgroundService!.CreateOrUpdateRecurring<RemindNearingDuesJob>(options.JobId, _ => _.RunAsync(),
        options.CronExpression, TimeZoneInfo.Utc);

app.Run();
