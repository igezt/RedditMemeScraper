using dotenv.net;
using RedditScraper.Services.Reddit;
using RedditScraper.Startup;
using TelegramBot.Services.TelegramBot;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<ITelegramBotService, TelegramBotService>();
builder.Services.AddRedditScraperDependencies();

var app = builder.Build();

var botService = app.Services.GetRequiredService<ITelegramBotService>();
await botService.Run();

app.Run();
