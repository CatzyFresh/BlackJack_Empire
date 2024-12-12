using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    public IEnumerator SlideAndFlipCard(GameObject card, Transform targetParent ,Vector3 targetPosition, Sprite cardFrontSprite, bool flip = true)
    {
        // Slide to target position
        
        float duration = 0.5f;
        Vector3 startPosition = card.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.SetParent(targetParent, true);
        card.transform.position = targetPosition;
        
        // Flip card if necessary
        if (flip)
        {
            yield return StartCoroutine(FlipCard(card, targetParent, cardFrontSprite));
        }
    }

    public IEnumerator FlipCard(GameObject card, Transform targetParent, Sprite cardFrontSprite)
    {
        card.transform.SetParent(targetParent, true);
        float flipDuration = 0.3f;
        Vector3 scale = card.transform.localScale;

        // Shrink to "flip"
        for (float t = 0; t < flipDuration; t += Time.deltaTime)
        {
            float progress = t / flipDuration;
            card.transform.localScale = new Vector3(Mathf.Lerp(scale.x, 0, progress), scale.y, scale.z);
            yield return null;
        }

        // Change sprite
        card.GetComponent<UnityEngine.UI.Image>().sprite = cardFrontSprite;

        //Expand back to original size
        for (float t = 0; t < flipDuration; t += Time.deltaTime)
        {
            float progress = t / flipDuration;
            card.transform.localScale = new Vector3(Mathf.Lerp(0, scale.x, progress), scale.y, scale.z);
            yield return null;
        }
        card.transform.localScale = scale;
    }

    public IEnumerator SlideChip(GameObject chip, Transform targetParent,Vector3 targetPosition)
    {
        chip.transform.SetParent(targetParent, true);
        float duration = 0.5f;
        Vector3 startPosition = chip.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            chip.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        chip.transform.position = targetPosition;
    }
}
