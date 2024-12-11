// UIManager Class
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Card Display")]
    [SerializeField] private Transform playerHandArea; // Parent for player card sprites
    [SerializeField] private Transform dealerHandArea; // Parent for dealer card sprites
    [SerializeField] private GameObject cardPrefab; // Prefab for card UI

    [Header("Action Buttons")]
    [SerializeField] private Button hitButton;
    [SerializeField] private Button standButton;
    [SerializeField] private Button confirmBetButton;

    [Header("Betting UI")]
    [SerializeField] private Transform chipsArea; // Parent for chip buttons
    [SerializeField] private TextMeshProUGUI playerBalanceText;
    [SerializeField] private TextMeshProUGUI currentBetText;
    [SerializeField] private Button tenDollarsChipButton;
    [SerializeField] private Button fiftyDollarsChipButton;
    [SerializeField] private Button hundredDollarsChipButton;
    [SerializeField] private Button fivehundredDollarsChipButton;
    [SerializeField] private GameObject bettingPanel;


    [Header("Card Database")]
    [SerializeField] private CardDatabase cardDatabase; // Reference to the card database

    [Header("Card Back")]
    [SerializeField] private Sprite cardBackSprite;

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
        EnableActionButtons(false);
        //UpdateBettingUI(0, GameManager.Instance.GetPlayerBalance());
    }

    public void UpdateBettingUI(int currentBet, int playerBalance)
    {
        currentBetText.text = $"Bet: ${currentBet}";
        playerBalanceText.text = $"Balance: ${playerBalance}";
    }


    public void UpdatePlayerHand(List<CardData> hand)
    {
        UpdateHandUI(playerHandArea, hand);
    }

    public void UpdateDealerHand(List<CardData> hand, bool revealAll = false)
    {
        UpdateHandUI(dealerHandArea, hand, revealAll);
    }

    private void UpdateHandUI(Transform handArea, List<CardData> hand, bool revealAll = true)
    {
        foreach (Transform child in handArea)
        {
            Destroy(child.gameObject); // Clear old cards
        }

        foreach (CardData card in hand)
        {
            GameObject cardGO = Instantiate(cardPrefab, handArea);
            Image cardImage = cardGO.GetComponent<Image>();

            if (revealAll || hand.IndexOf(card) != 1) // Reveal all or only upcard for the dealer
            {
                cardImage.sprite = card.sprite;
            }
            else
            {
                cardImage.sprite = GetCardBackSprite();
            }
        }
    }

    private Sprite GetCardBackSprite()
    {
        return cardBackSprite; // Use the preassigned sprite
    }


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
