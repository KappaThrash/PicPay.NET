using Microsoft.EntityFrameworkCore;
using PicPay.Extensions;
using PicPay.Repository;
using PicPay.Repository.DataContext;
using PicPay.Services;
using PicPay.Services.Notification;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<INotificationSender, NotificationSender>();


builder.Services.AddDbContext<PicPayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
