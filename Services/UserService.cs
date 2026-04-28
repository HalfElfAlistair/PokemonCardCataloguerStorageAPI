public class UserService
{
    private readonly List<User> _users;
    public UserService(List<User> users)
    {
        _users = users;
    }

    // Mapping Functions
    // User Output
    private static UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            Uid = user.Uid,
            Name = user.Name
        };
    }

    // User Input
    private static User FromCreateDto(UserCreateDto dto)
    {
        return new User
        {
            Uid = Guid.NewGuid(),
            Name = dto.Name,
            Cards = new List<Card>(),
            Lists = new List<CardsList>()
        };
    }

    // IResults
    // User outputs
    public IResult GetAllUsers()
    {
        var dtos = _users.Select(ToUserDto).ToList();
        return Results.Ok(dtos);
    }

    public IResult GetUser(Guid uid)
    {
        var user = Helpers.matchUserID(_users, uid);
        return user is not null ? Results.Ok(ToUserDto(user)) : Results.NotFound();
    }

    // User inputs
    // POST
    public IResult CreateUser(UserCreateDto dto)
    {
        var user = FromCreateDto(dto);
        _users.Add(user);

        return Results.Created($"/users/{user.Uid}", ToUserDto(user));
    }

    // PUT
    public IResult UpdateUser(Guid uid, UserUpdateDto dto)
    {
        var user = Helpers.matchUserID(_users, uid);
        if (user is null) return Results.NotFound();

        user.Name = dto.Name;

        return Results.Ok(ToUserDto(user));
    }

    // DELETE
    public IResult DeleteUser(Guid uid)
    {
        var user = Helpers.matchUserID(_users, uid);
        if (user is null) return Results.NotFound();

        _users.Remove(user);
        return Results.NoContent();
    }
}