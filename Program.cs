using Microsoft.EntityFrameworkCore;
using PicPay.Extesions;
using PicPay.Repository;
using PicPay.Repository.DataContext;
using PicPay.Services;
using PicPay.Services.Notification;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<NotificationSender>();


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
