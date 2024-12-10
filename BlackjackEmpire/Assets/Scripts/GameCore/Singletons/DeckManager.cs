using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    private Stack<Card> deck;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShuffleDeck()
    {
        List<Card> cards = CardFactory.CreateFullDeck();
        cards = CardFactory.Shuffle(cards);
        deck = new Stack<Card>(cards);
    }

    public Card DrawCard()
    {
        if (deck.Count > 0)
            return deck.Pop();
        else
            Debug.LogError("Deck is empty!");
        return null;
    }

    public void ResetDeck()
    {
        ShuffleDeck();
    }

    public void DealInitialCards(PlayerController player, DealerController dealer)
    {
        // Each player and the dealer get two cards
        player.AddCardToHand(DrawCard());
        player.AddCardToHand(DrawCard());

        dealer.AddCardToHand(DrawCard());
        dealer.AddCardToHand(DrawCard());
    }
}
