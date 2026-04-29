using FluentValidation;
public class ListNameUpdateDtoValidator : ListNameRules<CardsListNameUpdateDto>
{
    public ListNameUpdateDtoValidator()
    {
        AddNameRules(l => l.ListName);
    }
}