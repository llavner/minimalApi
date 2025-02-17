using minimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Minimal API";
    config.Title = "Minimal API v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
        {
        config.DocumentTitle = "Minimal API";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
        });
}

//Users Endpoints

var users = new List<User>() {
   new User {Id = 1, Name = "Anna"},
   new User {Id = 2, Name = "Bob"},
   new User {Id = 3, Name = "Charlie"}
};
app.MapGet("/users", () => users);
app.MapGet("/users/{id}", (int id) => {
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user.Name) : Results.NotFound($"Unable to find user with ID: {id}.");
});
app.MapPost("/users", (User user) => {
    if (users.Any(u => u.Id == user.Id))
        return Results.BadRequest($"UserId: {user.Id} is occupied, use another Id.");
    
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});
app.MapPut("/users", (int id, User userToUpdate) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null)
    {
        return Results.NotFound($"Unable to find user with ID: {id}.");
    }

    user.Name = userToUpdate.Name;
    return Results.Ok(user);

});
app.MapDelete("/users", (int id) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user is null)
    {
        return Results.NotFound($"Unable to find user with ID: {id}.");
    }

    users.Remove(user);
    return Results.Ok("User deleted.");
});


// app.MapGet("/product", () => product);
// app.MapGet("/order", () => order);


// app.MapGet("/category", () => category);
var categories = new List<Category>() 
{
    new () { Id = 1, Name = "Electronics"},
    new () { Id = 2, Name = "Clothes"},
    new () { Id = 3, Name =  "Books"},
};

app.MapGet("/categories", () => categories);
app.MapGet("/categories/{id}", (int id) => {
    var cat = categories.FirstOrDefault(c => c.Id == id);
    return cat is not null ? Results.Ok(cat.Name) : Results.NotFound($"Unable to find category with ID: {id}.");
});
app.MapPost("/categories", (Category cat) => {
    if (categories.Any(c => c.Id == cat.Id))
        return Results.BadRequest($"CatId: {cat.Id} is occupied, use another Id.");
    
    categories.Add(cat);
    return Results.Created($"/categories/{cat.Id}", cat);
});
app.MapPut("/categories", (int id, Category catToUpdate) => 
{
    var cat = categories.FirstOrDefault(c => c.Id == id);
    if (cat is null)
    {
        return Results.NotFound($"Unable to find category with ID: {id}.");
    }

    cat.Name = catToUpdate.Name;
    return Results.Ok(cat);

});
app.MapDelete("/categories", (int id) => 
{
    var cat = categories.FirstOrDefault(c => c.Id == id);

    if (cat is null)
    {
        return Results.NotFound($"Unable to find category with ID: {id}.");
    }

    categories.Remove(cat);
    return Results.Ok("Category deleted.");
});

app.Run();
