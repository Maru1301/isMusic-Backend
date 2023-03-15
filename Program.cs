using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace api.isMusic
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			string MyAllowOrigins = "AllowAny";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(
						name: MyAllowOrigins,
						policy => policy.WithOrigins("http://localhost:8080")
						   .AllowCredentials()
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						);
			});

			// Add Authentication services to the container.
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
				option.Cookie.SameSite = SameSiteMode.None;
                option.LoginPath = null;
            });

			builder.Services.AddMvc(options =>
			{
				options.Filters.Add(new AuthorizeFilter());
			});

			builder.Services.AddControllers();

			// Get all repository types
			var assembly = Assembly.GetExecutingAssembly();
			var repositoryTypes = assembly.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.Name == "IRepository"));

			// Register each type as a scoped service
			foreach (var repositoryType in repositoryTypes)
			{
				var interfaceType = repositoryType.GetInterfaces().First(i => i.Name != "IRepository");
				builder.Services.AddScoped(interfaceType, repositoryType);
			}

			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

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

            app.UseCors(MyAllowOrigins);

            app.UseHttpsRedirection();
            
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();            

			app.MapControllers();

			app.Run();
		}
	}
}