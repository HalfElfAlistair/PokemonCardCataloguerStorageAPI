public class CardService
{

    private readonly List<User> _users;

    public CardService(List<User> users)
    {
        _users = users;
    }

    // Mapping Functions
    // Card output
    private static CardDto ToCardDto(Card card)
    {
        return new CardDto
        {
            CardId = card.CardId,
            CardName = card.CardName,
            Count = card.Count
        };
    }

    // Card Input
    private static Card FromCardCreateDto(CardCreateDto dto)
    {
        return new Card
        {
            CardId = dto.CardId,
            CardName = dto.CardName,
            Count = dto.Count,
            Illustrator = dto.Illustrator
        };
    }

    // IResults
    // Card outputs
    public IResult GetCards(Guid uid)
    {
        var user = Helpers.matchUserID(_users, uid);
        if (user is null) return Results.NotFound();

        var userCards = user.Cards.Select(ToCardDto).ToList();
        return Results.Ok(userCards);
    }

    public IResult GetCard(Guid Uid, string CardId)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var card = Helpers.matchCardID(user.Cards, CardId);
        return card is not null ? Results.Ok(ToCardDto(card)) : Results.NotFound();
    }

    // Card inputs
    // POST
    public IResult AddCard(Guid Uid, CardCreateDto dto)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var card = FromCardCreateDto(dto);
        user.Cards.Add(card);

        return Results.Created($"/users/{Uid}/cards/{card.CardId}", ToCardDto(card));
    }

    // PUT
    public IResult UpdateCard(Guid Uid, string CardId, CardUpdateDto dto)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var card = Helpers.matchCardID(user.Cards, CardId);
        if (card is null) return Results.NotFound();

        card.Count = dto.Count;

        return Results.Ok(ToCardDto(card));
    }

    // DELETE
    public IResult DeleteCard(Guid Uid, string CardId)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var card = Helpers.matchCardID(user.Cards, CardId);
        if (card is null) return Results.NotFound();

        user.Cards.Remove(card);

        return Results.NoContent();
    }
}