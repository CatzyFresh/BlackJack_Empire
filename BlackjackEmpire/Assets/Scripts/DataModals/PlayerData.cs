using System;

[Serializable]
public class PlayerData
{
    public string UUID;
    public string PlayerName;
    public int Chips;
    public int PiggyBank;
    public int Wins;
    public int Losses;
    public int BlackJacks;
    public int TotalGamesPlayed; // Calculated as (Wins / Total Games) * 100
    public string[] CollectedDecks; // Placeholder for future feature
    public int CurrentLevel = 1;
    public int CurrentXP = 0;


    public float WinRate => TotalGamesPlayed > 0 ? (float)Wins / TotalGamesPlayed * 100 : 0;

    public PlayerData(string uuid, string playerName)
    {
        UUID = uuid;
        PlayerName = playerName;
        Chips = 1000; // Default chips for new players
        PiggyBank = 0;
        Wins = 0;
        Losses = 0;
        CollectedDecks = new string[0]; // Empty array for now
        CurrentLevel = 1;
        CurrentXP = 0;
        BlackJacks = 0;
    }
}
