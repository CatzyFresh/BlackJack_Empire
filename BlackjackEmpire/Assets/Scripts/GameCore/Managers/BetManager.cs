using UnityEngine;

public class BetManager : MonoBehaviour
{
    public int PlayerBalance { get; private set; } = 1000;

    public void CollectBets()
    {
        // Collect bets from players
    }

    public void PayoutWinnings(int amount)
    {
        PlayerBalance += amount;
    }

    public void DeductBet(int amount)
    {
        PlayerBalance -= amount;
    }

    public void ResetBets()
    {
        // Reset all bets
    }
}
