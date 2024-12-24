using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private GameObject loginScreenCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Slider progressBarSlider;
    [SerializeField] private TextMeshProUGUI progressText;

    private void Start()
    {
        ShowLoadingScreen();
    }

    /// <summary>
    /// Displays the loading screen and determines user navigation.
    /// </summary>
    private void ShowLoadingScreen()
    {
        // Activate loading screen
        loadingScreenCanvas.SetActive(true);
        loginScreenCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);

        // Start the loading process
        StartCoroutine(HandleLoadingProcess());
    }

    /// <summary>
    /// Handles the loading process with progress bar updates.
    /// </summary>
    private IEnumerator HandleLoadingProcess()
    {
        float progress = 0f;
        float totalLoadingTime = 2f; // Adjust the total loading duration if needed

        while (progress < 1f)
        {
            progress += Time.deltaTime / totalLoadingTime;

            // Update progress bar and text
            progressBarSlider.value = progress;
            progressText.text = $"Loading... {Mathf.FloorToInt(progress * 100)}%";

            yield return null;
        }

        // Determine user navigation after loading completes
        NavigateBasedOnUser();
        AdManager.Instance.InitializeAds();
    }

    /// <summary>
    /// Navigates to the appropriate screen based on the user type.
    /// </summary>
    private void NavigateBasedOnUser()
    {
        // Check if GuestLoginManager has existing player data
        if (GuestLoginManager.Instance != null)
        {
            if (GuestLoginManager.Instance.HasExistingPlayerData()) // User has data saved
            {
                Debug.Log("Returning user detected. Navigating to Main Menu...");
                GuestLoginManager.Instance.AutoLogin(); // Load user data
                ShowMainMenu();
            }
            else
            {
                Debug.Log("First-time user detected. Navigating to Login Screen...");
                ShowLoginScreen();
            }
        }
        else
        {
            Debug.LogError("GuestLoginManager instance is missing. Defaulting to Login Screen.");
            ShowLoginScreen();
        }

        // Hide the loading screen after navigation
        loadingScreenCanvas.SetActive(false);
    }

    /// <summary>
    /// Displays the login screen.
    /// </summary>
    private void ShowLoginScreen()
    {
        loginScreenCanvas.SetActive(true);
    }

    /// <summary>
    /// Displays the main menu screen.
    /// </summary>
    private void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
    }
}
