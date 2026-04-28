// Initialises a builder for web applications and services
var builder = WebApplication.CreateBuilder(args);

var users = new List<User>
{
    new User
    {
        Name = "Testing",
        Uid = Guid.Parse("0bf88970-1035-47e3-a5a7-7d8883f89d11"),
        Cards = new List<Card>
        {
            new Card
            {
                CardId = "sv03.5-092",
                CardName = "Gastly",
                Count = 1,
                Illustrator = "Tomokazu Komiya"
            },
        },
        Lists = new List<CardsList>
        {
            new CardsList
            {
                ListId = "favourites",
                ListName = "Favourites",
                CardIDs = ["sv03.5-092"]
            }
        }
    },
};

builder.Services.AddSingleton(users);
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CardService>();
builder.Services.AddSingleton<ListService>();


// Registers the required services
builder.Services.AddOpenApi();

// Produces a request function that executes added middlewares.
var app = builder.Build();

// Conditionally add services or middleware depending on the current environment
if (app.Environment.IsDevelopment())
{
    // Adds an endpoint to generate and serve an OpenAPI document in JSON format
    app.MapOpenApi();
}

// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();


app.MapGet("/", () => "API is running!");

// Users Endpoints
// Get users
app.MapGet("/users", (UserService service) =>
{
    return service.GetAllUsers();
});

// Get user
app.MapGet("/users/{uid}", (Guid uid, UserService service) =>
{
    return service.GetUser(uid);
});

// Add user
app.MapPost("/users", (UserCreateDto dto, UserService service) =>
{
    return service.CreateUser(dto);
});

// Update user
app.MapPut("/users/{uid}", (Guid uid, UserUpdateDto dto, UserService service) =>
{
    return service.UpdateUser(uid, dto);
});

// Delete user
app.MapDelete("/users/{uid}", (Guid uid, UserService service) =>
{
    return service.DeleteUser(uid);
});

// Cards Endpoints
// Get cards
app.MapGet("/users/{Uid}/cards", (Guid Uid, CardService service) =>
{
    return service.GetCards(Uid);
});

// Get card
app.MapGet("/users/{Uid}/cards/{CardId}", (Guid Uid, string CardId, CardService service) =>
{
    return service.GetCard(Uid, CardId);
});

// Add card
app.MapPost("users/{Uid}/cards", (Guid Uid, CardCreateDto dto, CardService service) =>
{
    return service.AddCard(Uid, dto);
});

// Update card
app.MapPut("/users/{Uid}/cards/{CardId}", (Guid Uid, string CardId, CardUpdateDto dto, CardService service) =>
{
    return service.UpdateCard(Uid, CardId, dto);
});

// Delete card
app.MapDelete("/users/{Uid}/cards/{CardId}", (Guid Uid, string CardId, CardService service) =>
{
    return service.DeleteCard(Uid, CardId);
});

// Lists Endpoints
// Get lists
app.MapGet("users/{Uid}/lists", (Guid Uid, ListService service) =>
{
    return service.GetLists(Uid);
});

// Get list
app.MapGet("users/{Uid}/lists/{ListId}", (Guid Uid, string ListId, ListService service) =>
{
    return service.GetList(Uid, ListId);
});

// Create list
app.MapPost("users/{Uid}/lists", (Guid Uid, CardsListCreateDto dto, ListService service) =>
{
    return service.CreateList(Uid, dto);
});

// Update list name
app.MapPut("users/{Uid}/lists/{ListId}/name", (Guid Uid, string ListId, CardsListNameUpdateDto dto, ListService service) =>
{
    return service.UpdateListName(Uid, ListId, dto);
});

// Update list cardIds
app.MapPut("users/{Uid}/lists/{ListId}", (Guid Uid, string ListId, CardsListUpdateDto dto, ListService service) =>
{
    return service.UpdateList(Uid, ListId, dto);
});

// Delete list
app.MapDelete("users/{Uid}/lists/{ListId}", (Guid Uid, string ListId, ListService service) =>
{
    return service.DeleteList(Uid, ListId);
});

app.Run();