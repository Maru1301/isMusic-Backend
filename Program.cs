using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace api.isMusic
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

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

			//builder.Services.AddScoped<ISongRepository, SongRepository>();
			//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

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

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}