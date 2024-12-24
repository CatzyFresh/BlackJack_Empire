using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI popupLevelText;
    [SerializeField] private TextMeshProUGUI popupXPText;
    [SerializeField] private Slider popupXpSlider;

    private void Start()
    {
        if(LevelSystemManager.Instance != null)
        {
            LevelSystemManager.Instance.OnXPChanged += UpdateXPUI;
            LevelSystemManager.Instance.OnLevelUp += UpdateLevelUI;
        }
    }

    private void OnDestroy()
    {
        LevelSystemManager.Instance.OnXPChanged -= UpdateXPUI;
        LevelSystemManager.Instance.OnLevelUp -= UpdateLevelUI;
    }

    public void UpdateLevelInfoInUserProfilePopup()
    {    
        popupLevelText.text = levelText.text; 
        popupXPText.text = xpText.text;
        popupXpSlider.value = xpSlider.value;
    }


    private void UpdateLevelUI()
    {
        levelText.text = $"{LevelSystemManager.Instance.CurrentLevel}";
        UpdateXPUI(null, new XPEventArgs(0, ""));
    }

    private void UpdateXPUI(object sender, XPEventArgs e)
    {
        int currentXP = LevelSystemManager.Instance.CurrentXP;
        int nextLevelXP = LevelSystemManager.Instance.XPForNextLevel;

        xpText.text = $"{currentXP}/{nextLevelXP} XP";
        xpSlider.value = (float)currentXP / nextLevelXP;
    }
}
