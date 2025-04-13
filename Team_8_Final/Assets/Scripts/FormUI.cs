using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormUI : MonoBehaviour
{
    public GameObject[] glows;
    public GameObject[] forms;

    private int currentIndex = -1;
    public LeanTweenType easeType;
    public float yOffset;
    public float duration;

    void Start()
    {
        // Deactivate all glows
        for (int i = 0; i < glows.Length; i++)
        {
            glows[i].SetActive(false);
        }
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                HandleSelection(i);
            }
        }
    }

    void HandleSelection(int newIndex)
    {
        if (newIndex == currentIndex)
        {
            // Toggle off the currently active one
            glows[newIndex].SetActive(false);
            AnimateFormDown(newIndex);
            currentIndex = -1;
        }
        else
        {
            // Animate previous one down if any
            if (currentIndex != -1)
            {
                glows[currentIndex].SetActive(false);
                AnimateFormDown(currentIndex);
            }

            // Activate new one
            glows[newIndex].SetActive(true);
            AnimateFormUp(newIndex);
            currentIndex = newIndex;
        }
    }

    void AnimateFormUp(int index)
    {
        RectTransform rect = forms[index].GetComponent<RectTransform>();
        LeanTween.cancel(rect);

        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0f, yOffset);

        LeanTween.move(rect, targetPos, duration).setEase(easeType);
    }

    void AnimateFormDown(int index)
    {
        RectTransform rect = forms[index].GetComponent<RectTransform>();
        LeanTween.cancel(rect);

        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startPos - new Vector2(0f, yOffset);

        LeanTween.move(rect, targetPos, duration).setEase(easeType);
    }
}
