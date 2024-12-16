using UnityEngine;
using TMPro;

public class InfoUI : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject gameplayCanvas; // Gameplay Canvas
    [SerializeField] private GameObject infoCanvas; // Info Canvas

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI rulesText; // Text component to display the rules

    [TextArea(5, 20)]
    [SerializeField] private string rules = @"<b>Detailed Rules of Classic Blackjack</b>
    
        <b>Objective:</b>
        - Beat the dealer by:
          - Having a hand total higher than the dealer's without exceeding 21.
          - The dealer’s hand exceeding 21 (busting).

        <b>Card Values:</b>
        - Number Cards (2-10): Face value.
        - Face Cards (King, Queen, Jack): 10 points each.
        - Ace: Either 1 or 11 points, depending on the hand's total value.

        <b>Game Setup:</b>
        - Decks: 1 to 8 standard 52-card decks.
        - Players: Compete against the dealer.
        - Betting: Place bets before cards are dealt.

        <b>Gameplay:</b>
        - <b>Initial Deal:</b> Players and dealer receive 2 cards.
        - <b>Player Actions:</b>
          - <b>Hit:</b> Request another card.
          - <b>Stand:</b> Keep the current hand.
          - <b>Double Down:</b> Double your bet and receive one card.
          - <b>Split:</b> Split pairs into two hands.
          - <b>Surrender:</b> Forfeit half the bet (optional).
        - <b>Dealer Actions:</b>
          - Reveal hole card after players finish.
          - <b>Hit:</b> Hand total 16 or less.
          - <b>Stand:</b> Hand total 17 or more.

        <b>Outcomes:</b>
        - <b>Win:</b> Hand closer to 21 than dealer or dealer busts.
        - <b>Lose:</b> Hand exceeds 21 or dealer is closer to 21.
        - <b>Push:</b> Both have the same hand total.

        <b>Special Rules:</b>
        - <b>Insurance:</b> Bet against dealer Blackjack.
        - <b>Even Money:</b> 1:1 payout for player Blackjack vs dealer Ace.

        <b>Betting and Payouts:</b>
        - Standard win: 1:1.
        - Blackjack: 3:2.
        - Insurance: 2:1 if dealer has Blackjack.";

    /// <summary>
    /// Toggles the visibility of the Info Canvas and Gameplay Canvas.
    /// </summary>
    public void ToggleInfoPanel()
    {
        bool isInfoActive = infoCanvas.activeSelf;

        // Switch between canvases
        infoCanvas.SetActive(!isInfoActive);
        gameplayCanvas.SetActive(isInfoActive);

        // Update the rules text when the Info Canvas is displayed
        if (!isInfoActive)
        {
            DisplayRules();
        }
    }

    /// <summary>
    /// Displays the rules text in the Info Panel.
    /// </summary>
    private void DisplayRules()
    {
        rulesText.text = rules;
    }
}
