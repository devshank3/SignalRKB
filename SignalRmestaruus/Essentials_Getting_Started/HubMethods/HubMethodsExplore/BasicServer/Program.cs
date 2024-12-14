using BasicServer.Hubs;

namespace BasicServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();

            // Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHub<AccessorHub>("/accessorHub");
            app.MapHub<StringToolsHub>("/stringToolsHub");

            app.Run();
        }
    }
}
