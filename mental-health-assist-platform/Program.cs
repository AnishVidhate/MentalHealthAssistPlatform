using Microsoft.EntityFrameworkCore;
using mental_health_assist_platform.Models;
using System;
using mental_health_assist_platform.Services;
using mental_health_assist_platform.Configuration;

namespace mental_health_assist_platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure database connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MentalHealthDbContext>(options =>
                options.UseSqlServer(connectionString));

            // configure email service from appsetting
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddScoped<IEmailService, EmailService>();
            // Add services to the container.
            builder.Services.AddControllers();

            //  Enable CORS correctly
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("*",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            builder.Services.AddScoped<IEmailService, EmailService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection(); // ❌ Disable HTTPS redirection for local testing

            //  Use CORS middleware with the correct policy
            app.UseCors("*");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
