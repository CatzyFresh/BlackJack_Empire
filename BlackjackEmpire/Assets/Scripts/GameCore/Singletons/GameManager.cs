using System.Collections;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private DeckManager deckManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BetManager betManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DealerController dealerController;

    public event Action<string, Color> OnResultDecided;

    private int localWins = 0;
    private int localLosses = 0;
    private int localTotalGames = 0;

    private enum ResultType
    {
        Win,
        Loss,
        Tie
    }

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
        int savedBalance = GuestLoginManager.Instance.CurrentPlayerData.Chips;
        betManager.InitializeBalance(savedBalance);
        Debug.Log("Game Initialized.");
        deckManager.ShuffleDeck();
        Debug.Log("Deck shuffled.");
        uiManager.InitializeUI();
        StartBettingPhase();
       
    }

    public void OnInfoButtonClick()
    {
        uiManager.ToggleInfoPanel();
    }

    public void SaveGame()
    {
        Debug.Log("Saving game progress...");

        // Update player data
        UpdatePlayerStats();
        UpdatePlayerChips();

        // Persist the data
        SavePlayerData();
        Debug.Log("Game progress saved successfully.");

        // Local function to update player stats
        void UpdatePlayerStats()
        {
            var playerData = GuestLoginManager.Instance.CurrentPlayerData;
            // Update stats in PlayerData
            playerData.Wins += localWins;
            playerData.Losses += localLosses;
            playerData.TotalGamesPlayed += localTotalGames;
            // Reset local stats
            ResetLocalStats();
        }

        // Local function to update chips
        void UpdatePlayerChips()
        {
            var playerData = GuestLoginManager.Instance.CurrentPlayerData;
            // Update chips in PlayerData
            playerData.Chips = betManager.PlayerBalance;
        }

        // Local function to reset local stats
        void ResetLocalStats()
        {
            localWins = 0;
            localLosses = 0;
            localTotalGames = 0;
        }

        // Local function to save player data
        void SavePlayerData()
        {
            GuestLoginManager.Instance.SavePlayerData();
        }
    }


    private void StartBettingPhase()
    {
        Debug.Log("Starting betting phase...");
        uiManager.HideHandValue();
        uiManager.ClearResultUI();
        uiManager.ClearPlacingBetArea();
        uiManager.ShowBettingUI(true);
        uiManager.UpdateBettingUI(0, betManager.PlayerBalance, false);
    }

    public void OnPlaceBet(int chipValue)
    {
        if(betManager.PlayerBalance < chipValue)
        {
            uiManager.ShowFeedback("Not enough balance!");
            return;
        }
        Debug.Log($"Placing bet with chip value: {chipValue}");
        betManager.PlaceBet(chipValue);
        uiManager.UpdateBettingUI(betManager.CurrentBet, betManager.PlayerBalance, false);
        uiManager.PlayChipSlideAnimation(chipValue);
    }

    public void OnCancelBet()
    {
        betManager.CancelBet();
        uiManager.UpdateBettingUI(betManager.CurrentBet, betManager.PlayerBalance, true);
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
            uiManager.ShowFeedback("Not enough balance!");
        }
    }

    public void OnAllIn()
    {
        betManager.AllIn();
        uiManager.UpdateBettingUI(betManager.CurrentBet, betManager.PlayerBalance, false);
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

        int playerHandValue = playerController.GetHandValue();
        int dealerUpCardValue = dealerController.Hand[0].GetBlackjackValue();


        uiManager.ShowHandValue(playerHandValue, 0, dealerUpCardValue, revealDealerHand: false);

        //Debug Logging 
        int initialPlayerValue = playerController.GetHandValue();
        int initialDealerValue = dealerController.GetHandValue();
        Debug.Log($"Player's Hand Value: {initialPlayerValue}");
        Debug.Log($"Dealer's Hand Value: {initialDealerValue}");
     
    }

    public void OnPlayerHit()
    {
        Debug.Log("Player chooses to Hit.");
        var card = deckManager.DrawCard();
        playerController.AddCardToHand(card);
        uiManager.UpdatePlayerHand(card);

        int playerHandValue = playerController.GetHandValue();
        int dealerUpCardValue = dealerController.Hand[0].GetBlackjackValue();
        uiManager.ShowHandValue(playerHandValue, 0, dealerUpCardValue, revealDealerHand: false);

        if (playerController.GetHandValue() > 21)
        {
            Debug.Log("Player Busts! Dealer Wins.");
            OnResultDecided?.Invoke("BUST!", Color.red);
            betManager.LoseBet();
            UpdateRoundStats(ResultType.Loss);
            EndRound();
        }
    }

    public void OnPlayerStand()
    {
        Debug.Log("Player chooses to Stand.");
        uiManager.EnableActionButtons(false);
        StartCoroutine(DealerTurnWithAnimation());
        
    }

    public void OnDoubleDown()
    {
        if (betManager.PlayerBalance >= betManager.CurrentBet)
        {
            Debug.Log("Player chooses to Double Down.");

            // Double the bet
            betManager.PlaceBet(betManager.CurrentBet);

            // Give the player one card
            var card = deckManager.DrawCard();
            playerController.AddCardToHand(card);
            uiManager.UpdatePlayerHand(card);

            //Show hand value in UI
            int playerHandValue = playerController.GetHandValue();
            int dealerUpCardValue = dealerController.Hand[0].GetBlackjackValue();
            uiManager.ShowHandValue(playerHandValue, 0, dealerUpCardValue, revealDealerHand: false);

            // Disable action buttons
            uiManager.EnableActionButtons(false);

            // Check if Player Busts and then Proceed to dealer's turn
            if (playerController.GetHandValue() > 21)
            {
                Debug.Log("Player Busts! Dealer Wins.");
                OnResultDecided?.Invoke("BUST!", Color.red);
                betManager.LoseBet();
                UpdateRoundStats(ResultType.Loss);
                EndRound();
            }
            else
            {
                
                StartCoroutine(DealerTurnWithAnimation());
            }
            
        }
        else
        {
            Debug.Log("Not enough balance to Double Down.");
            uiManager.ShowFeedback("Not enough balance to Double Down!");
        }
    }


    private IEnumerator DealerTurnWithAnimation()
    {
        Debug.Log("Dealer's Turn...");

        // Flip the dealer's hole card first
        uiManager.FlipDealerHoleCard(dealerController.Hand[1].sprite);
        yield return new WaitForSeconds(0.5f); // Wait for the flip animation to complete

        // Update hand values after flipping the hole card
        int dealerHandValue = dealerController.GetHandValue();
        int playerHandValue = playerController.GetHandValue();
        uiManager.ShowHandValue(playerHandValue, dealerHandValue, 0, revealDealerHand: true);


        while (dealerController.GetHandValue() < 17)
        {
            var card = deckManager.DrawCard();
            dealerController.AddCardToHand(card);
            uiManager.UpdateDealerHand(card, true);
            uiManager.ShowHandValue(playerHandValue, dealerHandValue, 0, revealDealerHand: true);
            yield return new WaitForSeconds(0.5f);
        }

        if (dealerController.GetHandValue() > 21)
        {
            Debug.Log("Dealer Busts! Player Wins.");
            if (playerController.GetHandValue() == 21)
            {
                BlackJack();
                UpdateRoundStats(ResultType.Win);
            }
            else
            {
                OnResultDecided?.Invoke("WIN!", Color.yellow);
                betManager.PayoutWinnings(2.0f);
                UpdateRoundStats(ResultType.Win);
            }

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

        uiManager.ShowHandValue(playerValue, dealerValue, 0, revealDealerHand: true);

        Debug.Log($"Player's Hand Value: {playerValue}");
        Debug.Log($"Dealer's Hand Value: {dealerValue}");

        
        if (playerValue > dealerValue)
        {
            if (playerValue == 21)
            {
                BlackJack();
            }
            Debug.Log("Player Wins!");
            OnResultDecided?.Invoke("WIN!", Color.yellow);
            betManager.PayoutWinnings(2.0f);
            UpdateRoundStats(ResultType.Win);
        }
        else if (playerValue < dealerValue)
        {
            Debug.Log("Dealer Wins!");
            OnResultDecided?.Invoke("LOST!", Color.red);
            betManager.LoseBet();
            UpdateRoundStats(ResultType.Loss);

        }
        else
        {
            Debug.Log("It's a Tie!");
            OnResultDecided?.Invoke("PUSH!", Color.white);
            betManager.PayoutWinnings(1.0f);
            UpdateRoundStats(ResultType.Tie);
        }
    }

    private void BlackJack()
    {
        Debug.Log("Player Wins With BlackJack!");
        betManager.PayoutWinnings(2.5f);
        OnResultDecided?.Invoke("BlackJack!", Color.yellow);
        UpdateRoundStats(ResultType.Win);
    }

    private void EndRound()
    {
        Debug.Log("Round Ended.");
        uiManager.EnableActionButtons(false);
        uiManager.UpdateBettingUI(0, betManager.PlayerBalance, true);
        SaveGame();
        StartCoroutine(ShowResultAndReset());
    }

    // Function to update local stats
    private void UpdateRoundStats(ResultType result)
    {
        localTotalGames++; // Increment total games in all cases

        switch (result)
        {
            case ResultType.Win:
                localWins++;
                Debug.Log($"Round Result: Win. Wins: {localWins}, Losses: {localLosses}, Total Games: {localTotalGames}");
                break;
            case ResultType.Loss:
                localLosses++;
                Debug.Log($"Round Result: Loss. Wins: {localWins}, Losses: {localLosses}, Total Games: {localTotalGames}");
                break;
            case ResultType.Tie:
                Debug.Log($"Round Result: Tie. Wins: {localWins}, Losses: {localLosses}, Total Games: {localTotalGames}");
                break;
        }
    }

    private void UpdatePlayerStats(bool win)
    {
        var playerData = GuestLoginManager.Instance.CurrentPlayerData;

        playerData.TotalGamesPlayed++;

        if (win)
        {
            playerData.Wins++;
        }
        else
        {
            playerData.Losses++;
        }

        Debug.Log($"Stats Updated: Wins - {playerData.Wins}, Losses - {playerData.Losses}, Win Rate - {playerData.WinRate}%");

        // Save progress
        GuestLoginManager.Instance.SavePlayerData();
    }

    private IEnumerator PauseAndShowAd()
    {
        // Pause the game during the ad
        Time.timeScale = 0;

        // Show interstitial ad
        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowInterstitialAd();
        }

        // Wait until the ad is done playing (ad logic resumes time)
        while (AdManager.Instance != null && !AdManager.Instance.IsAdClosed)
        {
            yield return null;
        }

        // Resume the game
        Time.timeScale = 1;
    }

    private IEnumerator ShowResultAndReset()
    {
        yield return new WaitForSeconds(2.0f); // Wait for the player to see the dealer's cards
        ResetRound();
    }
    private void ResetRound()
    {
        Debug.Log("Resetting round for next game.");
        StartCoroutine(PauseAndShowAd());
        deckManager.ResetDeck();
        betManager.ResetBet();
        uiManager.ClearHands();
        StartBettingPhase();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
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
