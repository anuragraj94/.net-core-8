using Query_Management_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Query_Management_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure in-memory database
            builder.Services.AddDbContext<QueryDbContext>(options =>
                options.UseInMemoryDatabase("QueryDatabase"));

            var app = builder.Build();

            // Seed the in-memory database with initial data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<QueryDbContext>();

                // Seed data if the database is empty
                if (!context.Queries.Any())
                {
                    context.Queries.AddRange(
                        new Query { Requestor = "John", Status = "Raised", CreatedAt = DateTime.Now },
                        new Query { Requestor = "Jane", Status = "Resolved", CreatedAt = DateTime.Now.AddDays(-1) },
                        new Query { Requestor = "Alice", Status = "On Hold", CreatedAt = DateTime.Now.AddDays(-2) }
                    );
                    context.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                //pattern: "{controller=Query}/{action=Index}/{id?}");
                pattern: "{controller=Query}/{action=Raise}/{id?}");

            app.Run();
        }
    }
}
