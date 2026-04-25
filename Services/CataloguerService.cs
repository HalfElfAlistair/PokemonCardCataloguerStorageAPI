using System.Linq;

public class CataloguerService
{
    // checks through users and returns valid user if matching provided Uid
    static User matchUserID(List<User> users, string Uid)
    {
        return users.FirstOrDefault(u => u.Uid == Uid);
    }
    private readonly List<User> _users;

    // User functions
    public CataloguerService(List<User> users)
    {
        _users = users;
    }

    public IResult GetUsers()
    {
        return Results.Ok(_users);
    }

    public IResult getUser(string Uid)
    {
        var user = matchUserID(_users, Uid);

        if (user is null)
            return Results.NotFound();

        // separates child data from user, which is collected from a different endpoint
        var userData = new
        {
            user.Name,
            user.Uid
        };

        return Results.Ok(userData);
    }

    public IResult AddUser(User user)
    {
        _users.Add(user);
        return Results.Created($"/users/{user.Uid}", user);
    }

    public IResult UpdateUser(string Uid, User userData)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        user.Name = userData.Name;

        return Results.Ok(user);
    }

    public IResult DeleteUser(string Uid)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        _users.Remove(user);
        return Results.NoContent();
    }

    // Cards functions
    static Card matchCardID(List<Card> cards, string CardId)
    {
        return cards.FirstOrDefault(c => c.CardId == CardId);
    }

    public IResult GetCards(string Uid)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        return Results.Ok(user.Cards);
    }

    public IResult GetCard(string Uid, string CardId)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        var card = matchCardID(user.Cards, CardId);

        return card is not null ? Results.Ok(card) : Results.NotFound();
    }

    public IResult AddCard(string Uid, Card cardData)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        user.Cards.Add(cardData);

        return Results.Created($"/users/{Uid}/cards/{cardData.CardId}", cardData);
    }

    public IResult UpdateCard(string Uid, string CardId, Card cardData)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        var card = matchCardID(user.Cards, CardId);

        if (card is null)
            return Results.NotFound();

        card.Count = cardData.Count;

        return Results.Ok(card);
    }

    public IResult DeleteCard(string Uid, string CardId)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        var card = matchCardID(user.Cards, CardId);

        if (card is null)
            return Results.NotFound();

        user.Cards.Remove(card);
        return Results.NoContent();
    }

    // Lists functions
    static CardsList matchListID(List<CardsList> lists, string ListId)
    {
        return lists.FirstOrDefault(l => l.ListId == ListId);
    }
    public IResult GetLists(string Uid)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        return Results.Ok(user.Lists);
    }

    public IResult GetList(string Uid, string ListId)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        var cardsList = matchListID(user.Lists, ListId);

        return cardsList is not null ? Results.Ok(cardsList) : Results.NotFound();
    }

    public IResult AddList(string Uid, CardsList list)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        Guid listGUID = Guid.NewGuid();
        list.ListId = listGUID.ToString();

        user.Lists.Add(list);

        return Results.Created($"/users/{Uid}/lists/{list.ListId}", list);
    }

    public IResult UpdateList(string Uid, string ListId, CardsList list)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        var cardsList = matchListID(user.Lists, ListId);

        if (cardsList is null)
            return Results.NotFound();

        cardsList.CardIDs = list.CardIDs;

        return Results.Ok(cardsList);
    }

    public IResult DeleteList(string Uid, string ListId)
    {
        var user = matchUserID(_users, Uid);
        if (user is null)
            return Results.NotFound();

        var cardsList = matchListID(user.Lists, ListId);

        if (cardsList is null)
            return Results.NotFound();

        // favourites is a default list that cannot be removed, returns custom 405 error
        if (cardsList.ListId == "favourites")
            return Results.StatusCode(405);

        user.Lists.Remove(cardsList);
        return Results.NoContent();
    }
}