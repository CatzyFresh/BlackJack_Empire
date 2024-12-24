using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainmenuUI : MonoBehaviour
{
    [Header("Player Stats Display")]
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private TextMeshProUGUI totalGamesPlayedText;
    [SerializeField] private TextMeshProUGUI winRateText;
    [SerializeField] private TextMeshProUGUI chipsAmountText;
    [SerializeField] private TextMeshProUGUI gameIDText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject profilePopup;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] LevelSystemUI levelSystemUI;


    private void OnEnable()
    {
        GuestLoginManager.OnPlayerDataUpdated += UpdatePlayerStatsUI;       
    }

    private void OnDisable()
    {
        GuestLoginManager.OnPlayerDataUpdated -= UpdatePlayerStatsUI;
    }

    private void Start()
    {
        UpdatePlayerStatsUI();
    }

    public void UpdatePlayerStatsUI()
    {
        if (GuestLoginManager.Instance.CurrentPlayerData == null)
        {
            Debug.Log("No Playerdata Found");
            return;
        }
           
        var playerData = GuestLoginManager.Instance.CurrentPlayerData;

        winsText.text = playerData.Wins.ToString();
        totalGamesPlayedText.text = playerData.TotalGamesPlayed.ToString();
        winRateText.text = $"{playerData.WinRate:F1}%";
        chipsAmountText.text = playerData.Chips.ToString();
        gameIDText.text = playerData.UUID;
        playerNameText.text = playerData.PlayerName;

        Debug.Log($"Playerdats current level {playerData.CurrentLevel}");
    }

    public void OnUserProfileButtonClicked()
    {
        profilePopup.SetActive(true);
        levelSystemUI.UpdateLevelInfoInUserProfilePopup();
    }

    public void OnUserProfileCloseButtonClicked()
    {
        profilePopup.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        if (!GameManager.Instance.IsGameInitialized)
        {
            GameManager.Instance.InitializeGame();
        } 
        gameplayCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void OnInstantChipsButtonClicked()
    {
        AdManager.Instance.ShowRewardedAd();
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
