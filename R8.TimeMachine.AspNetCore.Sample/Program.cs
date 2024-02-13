using R8.TimeMachine.AspNetCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTimeMachine();

        var app = builder.Build();

        var sampleTodos = new Todo[]
        {
            new(1, "Walk the dog"),
            new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
            new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
            new(4, "Clean the bathroom"),
            new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
        };

        app.UseTimeMachine(o => o.Provider = _ => Task.FromResult("Asia/Tehran"));
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context => { await context.Response.WriteAsJsonAsync(sampleTodos); });

            endpoints.MapGet("/todos", async context => { await context.Response.WriteAsJsonAsync(sampleTodos); });

            endpoints.MapGet("/todos/{id}", async context =>
            {
                if (int.TryParse(context.Request.RouteValues["id"]?.ToString(), out var id))
                {
                    var todo = sampleTodos.FirstOrDefault(a => a.Id == id);
                    if (todo is not null)
                    {
                        await context.Response.WriteAsJsonAsync(todo);
                        return;
                    }
                }

                context.Response.StatusCode = 404;
            });
        });

        app.Run();
    }
}

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);