
using System.Reflection;
using webapi.Models;

namespace webapi
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
            builder.Services.AddSwaggerGen(options => options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")));
            builder.Services.AddSqlServer<DemoContext>
            (builder.Configuration.GetConnectionString("MyConn"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Test", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.WithMethods("GET", "POST");
                });
                options.AddPolicy("Secure", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.WithMethods("GET", "POST");
                });
            });

            builder.Services.AddMemoryCache(); //enable in memory catching

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("Test");
            }
            else
            {
                app.UseCors("Secure");
            }
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}