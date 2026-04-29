using FluentValidation;
public class UserUpdateDtoValidator : UserNameRules<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        AddNameRules(u => u.Name);
    }
}