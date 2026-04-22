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
        }
    },
};


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
// Get users
app.MapGet("/users", () => users);

// Get user
app.MapGet("/users/{Uid}", (string Uid) =>
{
    var user = matchUserID(users, Uid);

    if (user is null)
        return Results.NotFound();

    // separates child data from user, which is collected from a different endpoint
    var userData = new
    {
        user.Name,
        user.Uid
    };

    return Results.Ok(userData);
});

// Add user
app.MapPost("/users", (User item) =>
{
    users.Add(item);
    return Results.Created($"/users/{item.Uid}", item);
});

// Update user
app.MapPut("/users/{Uid}", (string Uid, User item) =>
{
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    user.Name = item.Name;

    return Results.Ok(user);
});

// Delete user
app.MapDelete("/users/{Uid}", (string Uid) =>
{
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    users.Remove(user);
    return Results.NoContent();
});

// Cards Endpoints
// Get cards
app.MapGet("/users/{Uid}/cards", (string Uid) =>
{
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    return Results.Ok(user.Cards);
});

// Get card
app.MapGet("/users/{Uid}/cards/{CardId}", (string Uid, string CardId) =>
{
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    var card = matchCardID(user.Cards, CardId);

    return card is not null ? Results.Ok(card) : Results.NotFound();
});

// Add card 
app.MapPost("users/{Uid}/cards", (string Uid, Card card) =>
{
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    user.Cards.Add(card);

    return Results.Created($"/tasks/{Uid}/children/{card.CardId}", card);
});

// Update card
app.MapPut("/users/{Uid}/cards/{CardId}", (string Uid, string CardId, Card item) =>
{
    // Returns the first element of a sequence, or a default value if no element is found.
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    var card = matchCardID(user.Cards, CardId);

    if (card is null)
        return Results.NotFound();

    card.Count = item.Count;

    return Results.Ok(card);
});

// Delete card
app.MapDelete("/users/{Uid}/cards/{CardId}", (string Uid, string CardId) =>
{
    // Returns the first element of a sequence, or a default value if no element is found.
    var user = matchUserID(users, Uid);
    if (user is null)
        return Results.NotFound();

    var card = matchCardID(user.Cards, CardId);

    if (card is null)
        return Results.NotFound();

    user.Cards.Remove(card);
    return Results.NoContent();
});








app.Run();

public class User
{
    // User data
    public string Uid { get; set; } = "";
    public string Name { get; set; } = "";

    public List<Card> Cards { get; set; } = new();
}

public class Card
{
    public string CardId { get; set; } = "";
    public string CardName { get; set; } = "";
    public int Count { get; set; } = 0;
}