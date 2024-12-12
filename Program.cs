using MoviesApi.Services;

namespace MoviesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register DatabaseService as Singleton
            builder.Services.AddSingleton<DatabaseService>();

            // Register Controllers and API-related services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register HTTP Client for API calls
            builder.Services.AddHttpClient("APIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001"); // Replace with your API base address
            });

            // Build the application
            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Common middleware for production/development
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Map controllers (for your API routes)
            app.MapControllers();

            // If you want to serve Blazor from here, you can keep this part:
            // app.MapBlazorHub();
            // app.MapFallbackToPage("/_Host");

            // Run the application
            app.Run();
        }
    }
}
