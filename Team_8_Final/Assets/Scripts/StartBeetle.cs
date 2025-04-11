using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBeetle : MonoBehaviour
{
    public RectTransform[] pathPts;
    public float moveSpeed = 200f;
    private int ptsIndex = 0;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pathPts[ptsIndex].anchoredPosition;
    }

    void Update()
    {
        if (ptsIndex < pathPts.Length)
        {
            Vector2 currentPos = rectTransform.anchoredPosition;
            Vector2 targetPos = pathPts[ptsIndex].anchoredPosition;

            // Move beetle
            rectTransform.anchoredPosition = Vector2.MoveTowards(
                currentPos,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            // Calculate direction and rotate toward target
            Vector2 direction = targetPos - currentPos;
            if (direction.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rectTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Adjust -90° to face top if needed
            }

            // Advance to next point when close enough
            if (Vector2.Distance(currentPos, targetPos) < 1f)
            {
                ptsIndex++;
                if (ptsIndex == pathPts.Length)
                {
                    ptsIndex = 0;
                }
            }
        }
    }
}