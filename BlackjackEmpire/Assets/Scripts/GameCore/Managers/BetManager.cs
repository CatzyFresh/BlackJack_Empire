using UnityEngine;

public class BetManager : MonoBehaviour
{
   
    public int PlayerBalance { get; private set; } = 1000;
    public int CurrentBet { get; private set; } = 0;
    

    public void InitializeBalance(int initialBalance)
    {
        PlayerBalance = initialBalance;
        Debug.Log($"Player balance initialized to: {PlayerBalance}");
    }

    public void SetBalanceFromPlayerData(int savedBalance)
    {
        PlayerBalance = savedBalance;
        Debug.Log($"BetManager: Balance set to {PlayerBalance} from saved data.");
    }

    public void PlaceBet(int amount)
    {
        if (amount > PlayerBalance)
        {
            Debug.Log("Not enough balance to place the bet.");
            return;
        }

        CurrentBet += amount;
        PlayerBalance -= amount;
        Debug.Log($"Player placed a bet of ${amount}. Current bet: ${CurrentBet}. Remaining balance: ${PlayerBalance}");
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
        Debug.Log($"Player wins ${winnings}! New balance: ${PlayerBalance}");
    }

    public void LoseBet()
    {
        Debug.Log($"Player lost the bet of ${CurrentBet}. Remaining balance: ${PlayerBalance}");
    }

    public void DeductBet(int amount)
    {
        PlayerBalance -= amount;
    }

    public void ResetBet()
    {
        CurrentBet = 0;
    }

    public void CancelBet()
    {
        PlayerBalance += CurrentBet;
        CurrentBet = 0;
    }

    public void AllIn()
    {
        if (PlayerBalance > 0)
        {
            CurrentBet += PlayerBalance;
            Debug.Log($"Player goes All In with ${PlayerBalance}. Current bet is now ${CurrentBet}.");
            PlayerBalance = 0;
        }
        else
        {
            Debug.Log("Player cannot go All In with a balance of $0.");
        }
    }
}
