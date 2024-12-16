using UnityEngine;
using System.Collections;
using TMPro;

public class FeedbackUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText; // Reference to the feedback text component
    [SerializeField] private float feedbackDisplayDuration = 2.0f; // Duration to show the feedback message

    private Coroutine feedbackCoroutine;

    /// <summary>
    /// Updates and displays feedback text to the user.
    /// </summary>
    /// <param name="message">The feedback message to display.</param>
    public void ShowFeedback(string message)
    {
        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine); // Stop any existing coroutine to prevent overlap
        }

        feedbackCoroutine = StartCoroutine(ShowFeedbackCoroutine(message));
    }

    /// <summary>
    /// Coroutine to display feedback text for a limited duration.
    /// </summary>
    /// <param name="message">The feedback message to display.</param>
    /// <returns></returns>
    private IEnumerator ShowFeedbackCoroutine(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true); // Show the feedback text

        yield return new WaitForSeconds(feedbackDisplayDuration); // Wait for the duration

        feedbackText.text = string.Empty; // Clear the feedback text
        feedbackText.gameObject.SetActive(false); // Hide the feedback text
        feedbackCoroutine = null; // Reset the coroutine reference
    }
}
