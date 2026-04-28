public class ListService
{
    private readonly List<User> _users;

    public ListService(List<User> users)
    {
        _users = users;
    }

    // Mapping Functions
    // List output
    private static CardsListDto ToCardsListDto(CardsList list)
    {
        return new CardsListDto
        {
            ListId = list.ListId,
            ListName = list.ListName,
            CardIDs = list.CardIDs
        };
    }

    // List Input
    private static CardsList FromCardsListCreateDto(CardsListCreateDto dto)
    {
        return new CardsList
        {
            // ListId = dto.ListId,
            ListName = dto.ListName,
            CardIDs = dto.CardIDs,

        };
    }

    // IResults
    // List outputs
    public IResult GetLists(Guid uid)
    {
        var user = Helpers.matchUserID(_users, uid);
        if (user is null) return Results.NotFound();

        var userLists = user.Lists.Select(ToCardsListDto).ToList();
        return Results.Ok(userLists);
    }

    public IResult GetList(Guid Uid, string ListId)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var list = Helpers.matchListID(user.Lists, ListId);
        return list is not null ? Results.Ok(ToCardsListDto(list)) : Results.NotFound();
    }

    // List inputs
    // POST
    public IResult CreateList(Guid Uid, CardsListCreateDto dto)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var list = FromCardsListCreateDto(dto);
        Guid listGUID = Guid.NewGuid();
        list.ListId = listGUID.ToString();
        user.Lists.Add(list);

        return Results.Created($"/users/{Uid}/cards/{list.ListId}", ToCardsListDto(list));
    }

    // PUT
    public IResult UpdateListName(Guid Uid, string ListId, CardsListNameUpdateDto dto)
    {
        // favourites list is a default, only the cardIds can be updated
        if (ListId == "favourites") return Results.StatusCode(405);

        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var list = Helpers.matchListID(user.Lists, ListId);
        if (list is null) return Results.NotFound();

        list.ListName = dto.ListName;

        return Results.Ok(ToCardsListDto(list));
    }

    public IResult UpdateList(Guid Uid, string ListId, CardsListUpdateDto dto)
    {
        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var list = Helpers.matchListID(user.Lists, ListId);
        if (list is null) return Results.NotFound();

        list.CardIDs = dto.CardIDs;

        return Results.Ok(ToCardsListDto(list));
    }

    // DELETE
    public IResult DeleteList(Guid Uid, string ListId)
    {
        // favourites list is a default that should never be removed
        if (ListId == "favourites") return Results.StatusCode(405);

        var user = Helpers.matchUserID(_users, Uid);
        if (user is null) return Results.NotFound();

        var list = Helpers.matchListID(user.Lists, ListId);
        if (list is null) return Results.NotFound();

        user.Lists.Remove(list);

        return Results.NoContent();
    }
}