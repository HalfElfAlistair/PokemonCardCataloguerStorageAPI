using FluentValidation;
public class UserCreateDtoValidator : UserNameRules<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        AddNameRules(u => u.Name);
    }
}