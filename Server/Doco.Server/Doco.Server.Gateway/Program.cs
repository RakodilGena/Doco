using Doco.Server.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddSwagger();

builder.Services.AddEndpointsApiExplorer();

builder.AddServices();
builder.AddGrpcServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();