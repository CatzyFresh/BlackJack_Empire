// ScriptableObject for Card Data
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public int value; // Card value (e.g., 1 for Ace, 11 for Jack, etc.)
    public Suit suit; // Suit of the card (Hearts, Diamonds, Clubs, Spades)
    public Sprite sprite; // Sprite for the card face

    public int GetBlackjackValue()
    {
        if (value > 10) return 10; // Face cards are worth 10
        return value; // Other cards retain their face value (including Ace as 1 initially)
    }

}