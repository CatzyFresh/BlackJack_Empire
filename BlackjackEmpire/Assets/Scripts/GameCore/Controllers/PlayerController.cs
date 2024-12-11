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
    }

    public int GetHandValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (CardData card in Hand)
        {
            if (card.value > 10) value += 10; // Face cards
            else if (card.value == 1) aceCount++; // Ace
            else value += card.value;
        }

        while (aceCount > 0)
        {
            if (value + 11 <= 21) value += 11;
            else value += 1;
            aceCount--;
        }

        return value;
    }

    public void ResetHand()
    {
        Hand.Clear();
    }
}