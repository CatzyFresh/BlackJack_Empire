// ScriptableObject for Card Data
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public int value; // Card value (e.g., 1 for Ace, 11 for Jack, etc.)
    public Suit suit; // Suit of the card (Hearts, Diamonds, Clubs, Spades)
    public Sprite sprite; // Sprite for the card face
}