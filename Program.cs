// Initialises a builder for web applications and services
var builder = WebApplication.CreateBuilder(args);

var users = new List<User>
{
    new User
    {
        Name = "Testing",
        Uid = "mFaFtWyHGzVqtuM4kQ9LVzbSpzk1",
        Cards = new List<Card>
        {
            new Card
            {
                CardId = "sv03.5-092",
                CardName = "Gastly",
                Count = 1
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
builder.Services.AddSingleton<CataloguerService>();


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

// checks through users and returns valid user if matching provided Uid
static User matchUserID(List<User> users, string Uid)
{
    return users.FirstOrDefault(u => u.Uid == Uid);
}

// checks through cards and returns valid card if matching provided CardId
static Card matchCardID(List<Card> cards, string CardId)
{
    return cards.FirstOrDefault(c => c.CardId == CardId);
}

app.MapGet("/", () => "API is running!");

// Users Endpoints
// Get user
app.MapGet("/users/{Uid}", (string Uid, CataloguerService service) =>
{
    return service.getUser(Uid);
});

// Add user
app.MapPost("/users", (User user, CataloguerService service) =>
{
    return service.AddUser(user);
});

// Update user
app.MapPut("/users/{Uid}", (string Uid, User userData, CataloguerService service) =>
{
    return service.UpdateUser(Uid, userData);
});

// Delete user
app.MapDelete("/users/{Uid}", (string Uid, CataloguerService service) =>
{
    return service.DeleteUser(Uid);
});

// Cards Endpoints
// Get cards
app.MapGet("/users/{Uid}/cards", (string Uid, CataloguerService service) =>
{
    return service.GetCards(Uid);
});

// Get card
app.MapGet("/users/{Uid}/cards/{CardId}", (string Uid, string CardId, CataloguerService service) =>
{
    return service.GetCard(Uid, CardId);
});

// Add card 
app.MapPost("users/{Uid}/cards", (string Uid, Card cardData, CataloguerService service) =>
{
    return service.AddCard(Uid, cardData);
});

// Update card
app.MapPut("/users/{Uid}/cards/{CardId}", (string Uid, string CardId, Card cardData, CataloguerService service) =>
{
    return service.UpdateCard(Uid, CardId, cardData);
});

// Delete card
app.MapDelete("/users/{Uid}/cards/{CardId}", (string Uid, string CardId, CataloguerService service) =>
{
    return service.DeleteCard(Uid, CardId);
});

// Lists Endpoints
// Get lists
app.MapGet("users/{Uid}/lists", (string Uid, CataloguerService service) =>
{
    return service.GetLists(Uid);
});

// Get list
app.MapGet("users/{Uid}/lists/{ListId}", (string Uid, string ListId, CataloguerService service) =>
{
    return service.GetList(Uid, ListId);
});

// Add list
app.MapPost("users/{Uid}/lists", (string Uid, CardsList list, CataloguerService service) =>
{
    return service.AddList(Uid, list);
});

// Update list
app.MapPut("users/{Uid}/lists/{ListId}", (string Uid, string ListId, CardsList list, CataloguerService service) =>
{
    return service.UpdateList(Uid, ListId, list);
});

// Delete list
app.MapDelete("users/{Uid}/lists/{ListId}", (string Uid, string ListId, CataloguerService service) =>
{
    return service.DeleteList(Uid, ListId);
});





app.Run();

public class User
{
    // User data
    public string Uid { get; set; } = "";
    public string Name { get; set; } = "";

    public List<Card> Cards { get; set; } = new();
    public List<CardsList> Lists { get; set; } = new();
}

public class Card
{
    public string CardId { get; set; } = "";
    public string CardName { get; set; } = "";
    public int Count { get; set; } = 0;
}

public class CardsList
{
    public string ListId { get; set; } = "";
    public string ListName { get; set; } = "";
    public List<string> CardIDs { get; set; } = [];
}