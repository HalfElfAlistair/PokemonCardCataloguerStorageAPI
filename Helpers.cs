public class Helpers
{
    public static User matchUserID(List<User> users, Guid Uid)
    {
        return users.FirstOrDefault(u => u.Uid == Uid);
    }

    public static Card matchCardID(List<Card> cards, string CardId)
    {
        return cards.FirstOrDefault(c => c.CardId == CardId);
    }

    public static CardsList matchListID(List<CardsList> lists, string ListId)
    {
        return lists.FirstOrDefault(l => l.ListId == ListId);
    }
}