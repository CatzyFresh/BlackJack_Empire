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


    private void OnEnable()
    {
        GuestLoginManager.OnPlayerDataUpdated += UpdatePlayerStatsUI;
    }

    private void OnDisable()
    {
        GuestLoginManager.OnPlayerDataUpdated -= UpdatePlayerStatsUI;
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
    }

    public void OnUserProfileButtonClicked()
    {
        profilePopup.SetActive(true);
    }

    public void OnUserProfileCloseButtonClicked()
    {
        profilePopup.SetActive(false);
    }
}
