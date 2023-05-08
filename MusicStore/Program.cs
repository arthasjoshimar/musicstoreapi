using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<MusicStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

/*
app.MapGet("/api/Genres", async (MusicStoreDbContext context) =>  await context.Set<Genre>().ToListAsync() );

app.MapGet("/api/Genres/{id:int}", async (MusicStoreDbContext context, int id) =>
{
    var genre = await context.Set<Genre>().FindAsync(id);
    if (genre is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(genre);
});
*/

/*

app.MapGet("/api/Genres/{id:int}", (int id) => list.FirstOrDefault( x => x.Id == id));

app.MapPost("/api/Genres", (Genre genre) => {
    list.Add(genre);
    return Results.Created($"/api/Genres/{genre.Id}", genre);
 });
app.MapPut("/api/Genres/{id:int}", (int id, Genre genre) => list[id] = genre);

app.MapDelete("api/Genres.{id:int}", (int id) =>
{
    var genre = list.FirstOrDefault(x => x.Id == id);
    if(genre == null)
    {
        return Results.NotFound();
    }
    else
    {
        list.Remove(genre);
        return Results.Ok();
    }
});
*/
app.Run();
