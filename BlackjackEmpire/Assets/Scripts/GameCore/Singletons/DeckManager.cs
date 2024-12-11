// DeckManager Class
using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [SerializeField] private CardDatabase cardDatabase;
    private Stack<CardData> deck;

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
        cardDatabase.Shuffle();
        deck = new Stack<CardData>(cardDatabase.cards);
    }

    public CardData DrawCard()
    {
        if (deck.Count > 0)
            return deck.Pop();
        else
        {
            Debug.LogError("Deck is empty!");
            return null;
        }
    }

    public void ResetDeck()
    {
        ShuffleDeck();
    }

    public void DealInitialCards(PlayerController player, DealerController dealer)
    {
        player.AddCardToHand(DrawCard());
        player.AddCardToHand(DrawCard());

        dealer.AddCardToHand(DrawCard());
        dealer.AddCardToHand(DrawCard());
    }
}
