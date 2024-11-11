using BookRentalService.Interface;
using BookRentalService.Middleware;
using BookRentalService.Repository;
using BookRentalService.Service;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .CreateLogger();

// Add Serilog to the logging pipeline
builder.Host.UseSerilog((ctx, logconfig) => logconfig
    .WriteTo.Console()
    .WriteTo.File("logs\\myapp.log", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<BookRentalDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register repositories and services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IEmailService, EmailService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHangfire();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AppDBInitialiser();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard();
app.ConfigureRecurringJobs();

app.Run();
