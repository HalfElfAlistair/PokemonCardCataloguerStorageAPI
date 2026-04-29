using FluentValidation;
public class CardCreateDtoValidator : CardCountRules<CardCreateDto>
{
    public CardCreateDtoValidator()
    {
        RuleFor(c => c.CardName).NotEmpty().WithMessage("A name is required.");
        AddCountRules(c => c.Count);
    }
}