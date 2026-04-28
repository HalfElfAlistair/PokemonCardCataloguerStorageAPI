public class User
{
    // User data
    public Guid Uid { get; set; }
    public string Name { get; set; } = "";
    public List<Card> Cards { get; set; } = new();
    public List<CardsList> Lists { get; set; } = new();
}