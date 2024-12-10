using System.Collections.Generic;

public static class CardFactory
{
    public static List<Card> CreateFullDeck()
    {
        List<Card> deck = new List<Card>();
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            for (int value = 1; value <= 13; value++)
            {
                deck.Add(new Card(value, suit));
            }
        }
        return deck;
    }

    public static List<Card> Shuffle(List<Card> deck)
    {
        System.Random random = new System.Random();
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
        return deck;
    }
}
