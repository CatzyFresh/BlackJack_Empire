// ScriptableObject for Card Database
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "ScriptableObjects/CardDatabase", order = 2)]
public class CardDatabase : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();

    public void Shuffle()
    {
        System.Random random = new System.Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public CardData GetCard(int value, Suit suit)
    {
        return cards.Find(card => card.value == value && card.suit == suit);
    }
}