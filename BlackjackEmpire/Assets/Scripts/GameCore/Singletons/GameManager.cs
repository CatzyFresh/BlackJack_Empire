using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private DeckManager deckManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BetManager betManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DealerController dealerController;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Debug.Log("Game Initialized.");
        deckManager.ShuffleDeck();
        Debug.Log("Deck shuffled.");
        uiManager.InitializeUI();
        StartBettingPhase();
       
    }

    private void StartBettingPhase()
    {
        Debug.Log("Starting betting phase...");
        uiManager.ClearPlacingBetArea();
        uiManager.ShowBettingUI(true);
        uiManager.UpdateBettingUI(0, betManager.PlayerBalance);
    }

    public void OnPlaceBet(int chipValue)
    {
        Debug.Log($"Placing bet with chip value: {chipValue}");
        betManager.PlaceBet(chipValue);
        uiManager.UpdateBettingUI(betManager.CurrentBet, betManager.PlayerBalance);
        uiManager.PlayChipSlideAnimation(chipValue);
    }

    public void OnConfirmBet()
    {
        if (betManager.CurrentBet > 0)
        {
            Debug.Log("Bet confirmed. Starting round...");
            betManager.ConfirmBet();
            StartRound();
        }
        else
        {
            Debug.Log("Cannot start round without placing a bet.");
        }
    }

    public void StartRound()
    {
        Debug.Log("Starting a new round...");
        uiManager.ClearHands();
        //betManager.ResetBet();

        //SimulatePlayerBet();
        uiManager.ShowBettingUI(false);

        playerController.ResetHand();
        dealerController.ResetHand();

        StartCoroutine(DealInitialCardsAnimation());

        //uiManager.UpdatePlayerHand(playerController.Hand);
        //uiManager.UpdateDealerHand(dealerController.Hand, revealAll: false);

        uiManager.EnableActionButtons(true);
    }

    private IEnumerator DealInitialCardsAnimation()
    {
        for (int i = 0; i < 2; i++)
        {
            var playerCard = deckManager.DrawCard();
            playerController.AddCardToHand(playerCard);
            uiManager.UpdatePlayerHand(playerCard);
            yield return new WaitForSeconds(0.5f);

            var dealerCard = deckManager.DrawCard();
            dealerController.AddCardToHand(dealerCard);
            uiManager.UpdateDealerHand(dealerCard, i == 0); // Only flip the dealer's up-card
            yield return new WaitForSeconds(0.5f);
        }

        int initialPlayerValue = playerController.GetHandValue();
        int initialDealerValue = dealerController.GetHandValue();
        Debug.Log($"Player's Hand Value: {initialPlayerValue}");
        Debug.Log($"Dealer's Hand Value: {initialDealerValue}");
        //uiManager.UpdatePlayerHand(playerController.Hand);
        //uiManager.UpdateDealerHand(dealerController.Hand, revealAll: false);
    }

    public void OnPlayerHit()
    {
        Debug.Log("Player chooses to Hit.");
        var card = deckManager.DrawCard();
        playerController.AddCardToHand(card);
        uiManager.UpdatePlayerHand(card);

        if (playerController.GetHandValue() > 21)
        {
            Debug.Log("Player Busts! Dealer Wins.");
            betManager.LoseBet();
            EndRound();
        }
    }

    public void OnPlayerStand()
    {
        Debug.Log("Player chooses to Stand.");
        uiManager.EnableActionButtons(false);
        StartCoroutine(DealerTurnWithAnimation());
        
    }

    private IEnumerator DealerTurnWithAnimation()
    {
        Debug.Log("Dealer's Turn...");

        // Flip the dealer's hole card first
        uiManager.FlipDealerHoleCard(dealerController.Hand[1].sprite);
        yield return new WaitForSeconds(0.5f); // Wait for the flip animation to complete

        while (dealerController.GetHandValue() < 17)
        {
            var card = deckManager.DrawCard();
            dealerController.AddCardToHand(card);
            uiManager.UpdateDealerHand(card, true);
            yield return new WaitForSeconds(0.5f);
        }

        if (dealerController.GetHandValue() > 21)
        {
            Debug.Log("Dealer Busts! Player Wins.");
            betManager.PayoutWinnings(2.0f);
        }
        else
        {
            CompareHands();
        }
        EndRound();
    }


    private void CompareHands()
    {
        int playerValue = playerController.GetHandValue();
        int dealerValue = dealerController.GetHandValue();

        Debug.Log($"Player's Hand Value: {playerValue}");
        Debug.Log($"Dealer's Hand Value: {dealerValue}");

        if (playerValue > dealerValue)
        {
            Debug.Log("Player Wins!");
            betManager.PayoutWinnings(2.0f);
        }
        else if (playerValue < dealerValue)
        {
            Debug.Log("Dealer Wins!");
            betManager.LoseBet();
        }
        else
        {
            Debug.Log("It's a Tie!");
            betManager.PayoutWinnings(1.0f);
        }
    }

    private void EndRound()
    {
        Debug.Log("Round Ended.");
        uiManager.EnableActionButtons(false);
        uiManager.UpdateBettingUI(0, betManager.PlayerBalance);
        StartCoroutine(ShowResultAndReset());
    }

    private IEnumerator ShowResultAndReset()
    {
        yield return new WaitForSeconds(2.0f); // Wait for the player to see the dealer's cards
        ResetRound();
    }
    private void ResetRound()
    {
        Debug.Log("Resetting round for next game.");
        deckManager.ResetDeck();
        betManager.ResetBet();
        uiManager.ClearHands();
        StartBettingPhase();
    }


    #region simulation
    //private void SimulatePlayerBet()
    //{
    //    Debug.Log("Player is placing a bet...");
    //    int betAmount = GetMockBetAmount(); // Simulate player input for betting
    //    betManager.PlaceBet(betAmount);
    //}

    //private int GetMockBetAmount()
    //{
    //    // Simulate a bet amount (e.g., between 10 and 100)
    //    return Random.Range(10, 101);
    //}

    //private string GetMockPlayerAction()
    //{
    //    // Simulate player input for now (cycle between Hit and Stand for demonstration)
    //    return Random.Range(0, 2) == 0 ? "H" : "S";
    //}

    //private void LogHands()
    //{
    //    Debug.Log($"Player's Hand: {HandToString(playerController.Hand)} (Value: {playerController.GetHandValue()})");
    //    Debug.Log($"Dealer's Upcard: {dealerController.Hand[0].Value} of {dealerController.Hand[0].Suit}");
    //}

    //private string HandToString(System.Collections.Generic.List<Card> hand)
    //{
    //    string result = "";
    //    foreach (Card card in hand)
    //    {
    //        result += $"{card.Value} of {card.Suit}, ";
    //    }
    //    return result.TrimEnd(',', ' ');
    //}
#endregion
}
