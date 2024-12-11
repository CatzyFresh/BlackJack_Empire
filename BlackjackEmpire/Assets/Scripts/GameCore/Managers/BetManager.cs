using UnityEngine;

public class BetManager : MonoBehaviour
{
    public int PlayerBalance { get; private set; } = 1000;
    public int CurrentBet { get; private set; } = 0;

    public void PlaceBet(int amount)
    {
        if (amount > PlayerBalance)
        {
            Debug.Log("Not enough balance to place the bet.");
            return;
        }

        CurrentBet = amount;
        PlayerBalance -= amount;
        Debug.Log($"Player placed a bet of {amount}. Remaining balance: {PlayerBalance}");
    }

    public void ConfirmBet()
    {
        if (CurrentBet > 0)
        {
            Debug.Log($"Bet confirmed: ${CurrentBet}");
        }
        else
        {
            Debug.Log("No bet placed.");
        }
    }

    public void CollectBets()
    {
        // Collect bets from players
    }

    public void PayoutWinnings(float multiplier)
    {
        int winnings = Mathf.RoundToInt(CurrentBet * multiplier);
        PlayerBalance += winnings;
        Debug.Log($"Player wins {winnings}! New balance: {PlayerBalance}");
    }

    public void LoseBet()
    {
        Debug.Log($"Player lost the bet of {CurrentBet}. Remaining balance: {PlayerBalance}");
    }

    public void DeductBet(int amount)
    {
        PlayerBalance -= amount;
    }

    public void ResetBet()
    {
        CurrentBet = 0;
    }
}
