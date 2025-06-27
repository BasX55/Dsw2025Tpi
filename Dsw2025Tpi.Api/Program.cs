using Dsw2025Ej15.Application.Dtos;
using Dsw2025Ej15.Application.Services;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain;
using Microsoft.EntityFrameworkCore;
namespace Dsw2025Tpi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
        {
            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Dsw2025Tpi;Integrated Security=True;MultipleActiveResultSets=true");
        }
        );
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();

        builder.Services.AddScoped<Dsw2025Tpi.Domain.Interfaces.IRepository, Dsw2025Tpi.Data.Repositories.InMemory>(); // PARA JSON

        //builder.Services.AddScoped<Dsw2025Tpi.Domain.Interfaces.IRepository, Dsw2025Tpi.Data.Repositories.EfRepository>(); //PARA SQL SERVER
        
        builder.Services.AddScoped<ProductsManagementService>();
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
        
        app.MapHealthChecks("/healthcheck");

        app.Run();
    }
}
