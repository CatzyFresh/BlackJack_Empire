using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<Card> Hand { get; private set; } = new List<Card>();
    public int BetAmount { get; set; }

    public void AddCardToHand(Card card)
    {
        Hand.Add(card);
    }

    public int GetHandValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (Card card in Hand)
        {
            if (card.Value > 10) value += 10; // Face cards
            else if (card.Value == 1) aceCount++; // Ace
            else value += card.Value;

            while (aceCount > 0)
            {
                if (value + 11 <= 21) value += 11;
                else value += 1;
                aceCount--;
            }
        }
        return value;
    }

    public void ResetHand()
    {
        Hand.Clear();
    }
}
