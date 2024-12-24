using System;
using UnityEngine;

public class LevelSystemManager : MonoBehaviour
{
    public static LevelSystemManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int baseXP = 100; // XP needed for the first level
    [SerializeField] private float xpGrowthRate = 1.2f; // Multiplier for each level
    [SerializeField] private int maxLevel = 50;

    public float XPProgress => (float)CurrentXP / XPForNextLevel;


    public int CurrentLevel { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;
    public int XPForNextLevel => Mathf.RoundToInt(baseXP * Mathf.Pow(xpGrowthRate, CurrentLevel - 1));

    public float BaseXP { get; internal set; }
    public float XPGrowthRate { get; internal set; }

    public event EventHandler<XPEventArgs> OnXPChanged;
    public event Action OnLevelUp;
   

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
        GuestLoginManager.OnPlayerDataUpdated += InitializeFromPlayerData; 
    }

    private void InitializeFromPlayerData()
    {
        var playerData = GuestLoginManager.Instance.CurrentPlayerData;
        if (playerData != null)
        {
            CurrentLevel = playerData.CurrentLevel;
            CurrentXP = playerData.CurrentXP;

            Debug.Log($"LevelSystemManager initialized: Level {CurrentLevel}, XP {CurrentXP}/{XPForNextLevel}");

            // Trigger necessary events for UI refresh
            OnXPChanged?.Invoke(this, new XPEventArgs(CurrentXP, "Startup")); // Use actual XP
            OnLevelUp?.Invoke(); // Trigger UI update for the level
        }
    }

    public void AddXP(int amount, string source)
    {
        CurrentXP += amount;
        Debug.Log($"Added {amount} XP from {source}. Current XP: {CurrentXP}/{XPForNextLevel}, Level: {CurrentLevel}");
        OnXPChanged?.Invoke(this, new XPEventArgs(amount, source));
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (CurrentLevel >= maxLevel) return;
        while (CurrentXP >= XPForNextLevel)
        {
            CurrentXP -= XPForNextLevel;
            CurrentLevel++;
            OnLevelUp?.Invoke();
        }
    }

    public void SyncPlayerData()
    {
        var playerData = GuestLoginManager.Instance.CurrentPlayerData;
        if (playerData != null)
        {
            playerData.CurrentXP = CurrentXP;
            playerData.CurrentLevel = CurrentLevel;
        }
    }
}
