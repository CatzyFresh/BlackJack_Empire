using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private DeckManager deckManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BetManager betManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DealerController dealerController;


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
        InitializeGame();
    }

    private void InitializeGame()
    {
        deckManager.ShuffleDeck();
       // uiManager.ShowMainMenu();
    }

    public void StartRound()
    {
        betManager.CollectBets();
        DeckManager.Instance.DealInitialCards(playerController, dealerController);
        uiManager.UpdateGameUI();
    }

    public void EndRound()
    {
        uiManager.ShowResults();
        ResetRound();
    }

    private void ResetRound()
    {
        deckManager.ResetDeck();
        betManager.ResetBets();
        uiManager.ResetUI();
    }
}
