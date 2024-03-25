using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;
namespace ToDo.Controllers;

public static class ApiController
{
    public static void MapToDosEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/ToDos", async (ApplicationDbContext db) =>
        {
            return await db.ToDos.ToListAsync();
        })
        .WithName("GetAllToDoss")
        .Produces<List<ToDos>>(StatusCodes.Status200OK);

        routes.MapGet("/api/ToDos/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.ToDos.FindAsync(Id)
                is ToDos model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetToDosById")
        .Produces<ToDos>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/ToDos/{id}", async (int Id, ToDos toDos, ApplicationDbContext db) =>
        {
            var foundModel = await db.ToDos.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(toDos);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateToDos")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/ToDos/", async (ToDos toDos, ApplicationDbContext db) =>
        {
            db.ToDos.Add(toDos);
            await db.SaveChangesAsync();
            return Results.Created($"/ToDoss/{toDos.Id}", toDos);
        })
        .WithName("CreateToDos")
        .Produces<ToDos>(StatusCodes.Status201Created);

        routes.MapDelete("/api/ToDos/{id}", async (int Id, ApplicationDbContext db) =>
                {
                    if (await db.ToDos.FindAsync(Id) is ToDos toDos)
                    {
                        db.ToDos.Remove(toDos);
                        await db.SaveChangesAsync();
                        return Results.Ok(toDos);
                    }

                    return Results.NotFound();
                })
                .WithName("DeleteToDos")
                .Produces<ToDos>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
    }
}
