// UIManager Class
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Card Display")]
    [SerializeField] private Transform deckArea;
    [SerializeField] private Transform playerHandArea; // Parent for player card sprites
    [SerializeField] private Transform dealerHandArea; // Parent for dealer card sprites
    [SerializeField] private GameObject cardPrefab; // Prefab for card UI

    [Header("Action Buttons")]
    [SerializeField] private Button hitButton;
    [SerializeField] private Button standButton;
    [SerializeField] private Button confirmBetButton;

    [Header("Betting UI")]
    [SerializeField] private Transform chipsArea; // Parent for chip buttons
    [SerializeField] private Transform placingBetArea;
    [SerializeField] private TextMeshProUGUI playerBalanceText;
    [SerializeField] private TextMeshProUGUI currentBetText;
    [SerializeField] private Button tenDollarsChipButton;
    [SerializeField] private Button fiftyDollarsChipButton;
    [SerializeField] private Button hundredDollarsChipButton;
    [SerializeField] private Button fivehundredDollarsChipButton;
    [SerializeField] private GameObject bettingPanel;
    [SerializeField] private TextMeshProUGUI betTextDisplayNearChips;


    [Header("Card Database")]
    [SerializeField] private CardDatabase cardDatabase; // Reference to the card database

    [Header("Card Back")]
    [SerializeField] private Sprite cardBackSprite;

    [Header("UI Animations")]
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private GameObject tenDollarChipPrefab;
    [SerializeField] private GameObject fiftyDollarChipPrefab;
    [SerializeField] private GameObject hundredDollarChipPrefab;
    [SerializeField] private GameObject fiveHundredDollarChipPrefab;

    [Header("Result Display")]
    [SerializeField] private TextMeshProUGUI roundResultText;

    private void OnDisable()
    {
        GameManager.Instance.OnResultDecided -= UpdateResultUI;
    }

    public void InitializeUI()
    {
        ClearHands();
        hitButton.onClick.AddListener(() => GameManager.Instance.OnPlayerHit());
        standButton.onClick.AddListener(() => GameManager.Instance.OnPlayerStand());
        confirmBetButton.onClick.AddListener(() => GameManager.Instance.OnConfirmBet());
        tenDollarsChipButton.onClick.AddListener(() => GameManager.Instance.OnPlaceBet(10));
        fiftyDollarsChipButton.onClick.AddListener(() => GameManager.Instance.OnPlaceBet(50));
        hundredDollarsChipButton.onClick.AddListener(() => GameManager.Instance.OnPlaceBet(100));
        fivehundredDollarsChipButton.onClick.AddListener(() => GameManager.Instance.OnPlaceBet(500));
        GameManager.Instance.OnResultDecided += UpdateResultUI;
        EnableActionButtons(false);
    }

    public void UpdateBettingUI(int currentBet, int playerBalance)
    {
        currentBetText.text = $"Bet: ${currentBet}";
        playerBalanceText.text = $"Balance: ${playerBalance}";
        betTextDisplayNearChips.text = $"${currentBet}";
    }


    public void UpdatePlayerHand(CardData card)
    {
        AddCardToHandUI(playerHandArea, card, true);
    }

    public void UpdateDealerHand(CardData card, bool reveal)
    {
        AddCardToHandUI(dealerHandArea, card, reveal);
    }

    public void AddCardToHandUI(Transform handArea, CardData card, bool flip)
    {
        GameObject cardGO = Instantiate(cardPrefab, deckArea);
        PlayCardSlideAndFlipAnimation(cardGO, handArea, card, flip);
    }

    private Sprite GetCardBackSprite()
    {
        return cardBackSprite; // Use the preassigned sprite
    }

    #region Animation
    public void PlayCardSlideAndFlipAnimation(GameObject cardGO,Transform handArea,CardData card, bool flip)
    {
        Vector3 targetPosition = handArea.position; // Adjust target based on your layout
        animationManager.StartCoroutine(animationManager.SlideAndFlipCard(cardGO, handArea, targetPosition, card.sprite, flip));
    }

    public void FlipDealerHoleCard(Sprite cardFrontSprite)
    {
        if (dealerHandArea.childCount > 1) // Ensure the dealer has at least two cards
        {
            Transform holeCardTransform = dealerHandArea.GetChild(1); // Get the second card (hole card)
            GameObject holeCardGO = holeCardTransform.gameObject;
            animationManager.StartCoroutine(animationManager.FlipCard(holeCardGO, dealerHandArea, cardFrontSprite));
        }
        else
        {
            Debug.LogError("Dealer does not have a second card to flip.");
        }
    }

    public void PlayChipSlideAnimation(int chipValue)
    {
        GameObject chip = null;
        switch (chipValue)
        {
            case 10:
                chip = Instantiate(tenDollarChipPrefab, chipsArea);
                break;
            case 50:
                chip = Instantiate(fiftyDollarChipPrefab, chipsArea);
                break;
            case 100:
                chip = Instantiate(hundredDollarChipPrefab, chipsArea);
                break;
            case 500:
                chip = Instantiate(fiveHundredDollarChipPrefab, chipsArea);
                break;
        }

        if (chip != null)
        {
            Vector3 targetPosition = placingBetArea.position; // Adjust based on your layout
            animationManager.StartCoroutine(animationManager.SlideChip(chip, placingBetArea, targetPosition));
        }
    }
    #endregion

    public void EnableActionButtons(bool enable)
    {
        hitButton.interactable = enable;
        standButton.interactable = enable;
        hitButton.gameObject.SetActive(enable);
        standButton.gameObject.SetActive(enable);
    }

    public void ShowBettingUI(bool setActive)
    {
        bettingPanel.SetActive(setActive);
        chipsArea.gameObject.SetActive(setActive);
        placingBetArea.gameObject.SetActive(setActive);
        betTextDisplayNearChips.gameObject.SetActive(setActive);
        currentBetText.gameObject.SetActive(!setActive);   
    }

    public void UpdateResultUI(string resultText, Color color)
    {
        roundResultText.text = resultText;
        roundResultText.color = color;
        roundResultText.gameObject.SetActive(true);
    }

    public void ClearResultUI()
    {
        roundResultText.text = string.Empty;
        roundResultText.gameObject.SetActive(false);
    }

    public void ClearPlacingBetArea()
    {
        foreach (Transform child in placingBetArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearHands()
    {
        foreach (Transform child in playerHandArea)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in dealerHandArea)
        {
            Destroy(child.gameObject);
        }
    }
}
