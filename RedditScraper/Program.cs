using dotenv.net;
using RedditScraper.Services.Adapters.Models.Enums;
using RedditScraper.Services.Converter;
using RedditScraper.Services.Environment;
using RedditScraper.Services.Reddit;
using RedditScraper.Services.RedditAuth;
using RedditScraper.Services.RedditClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DotEnv.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<IEnvService, EnvService>();
builder.Services.AddSingleton<IRedditService, RedditService>();
builder.Services.AddSingleton<IRedditAuthService, RedditAuthService>();
builder.Services.AddSingleton<IRedditClient, RedditClient>();
builder.Services.AddSingleton<IConverterService, ConverterService>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var posts = await app.Services.GetService<IRedditService>().GetTopPostsInPastDay("memes", 20);
app.Services.GetService<IConverterService>().Convert(FileType.HTML, posts);

app.Run();
