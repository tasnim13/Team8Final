using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkhLives : MonoBehaviour
{
    public float yOffset = 20f;
    public float duration = 1f;
    public LeanTweenType easeType = LeanTweenType.easeInOutSine;
    public float delayBetween = 0.2f; // Delay between each child's start

    void Start()
    {
        // Loop through each child RectTransform
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            if (child != null)
            {
                StartBounce(child, i * delayBetween);
            }
        }
    }

    void StartBounce(RectTransform rectTransform, float delay)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0f, yOffset);

        LeanTween.move(rectTransform, targetPos, duration)
            .setEase(easeType)
            .setDelay(delay)
            .setLoopPingPong();
    }
}
