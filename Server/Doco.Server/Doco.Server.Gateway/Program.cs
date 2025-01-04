using Doco.Server.Gateway.Extensions;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureMaxRequestSize()
    .AddSwagger()
    .AddServices()
    .AddGrpcServices()
    .AddDaemons();

builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();