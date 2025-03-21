
using LibraryManagement.Controllers;
using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Services from Identity Core.
            builder.Services
                .AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<LibraryDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddDbContext<LibraryDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("conn").ToString());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Config. CORS
            app.UseCors(options =>
                options.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            #endregion 

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app
                .MapGroup("/api")
                .MapIdentityApi<User>();

            app.MapGroup("/api")
                .MapIdentityUserEndpoints();
            app.Run();
        }
    }
}
