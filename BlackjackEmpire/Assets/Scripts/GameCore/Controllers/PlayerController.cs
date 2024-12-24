// PlayerController Class
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    public List<CardData> Hand { get; private set; } = new List<CardData>();
    public int BetAmount { get; set; }

    

    public void AddCardToHand(CardData card)
    {
        Hand.Add(card);
        Debug.Log(Hand);
    }

    public int GetHandValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (CardData card in Hand)
        {
            if (card.value == 1) aceCount++; // Count Aces separately
            else value += card.GetBlackjackValue(); // Use computed Blackjack value
        }

        // Adjust Ace values as needed
        while (aceCount > 0)
        {
            if (value + 11 <= 21) value += 11; // Treat Ace as 11 if it doesn't bust
            else value += 1; // Otherwise, treat Ace as 1
            aceCount--;
        }

        return value;
    }


    public void ResetHand()
    {
        Hand.Clear();
    }
}