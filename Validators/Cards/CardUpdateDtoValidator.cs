using FluentValidation;
public class CardUpdateDtoValidator : CardCountRules<CardUpdateDto>
{
    public CardUpdateDtoValidator()
    {
        AddCountRules(u => u.Count);
    }
}