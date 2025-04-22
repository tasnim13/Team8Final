using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormUI : MonoBehaviour
{
    public GameObject[] glows;
    public GameObject[] forms;
    public GameObject[] formsDisabled;

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

        // Enable only unlocked forms and disable others
        for (int i = 0; i < forms.Length; i++)
        {
            bool unlocked = GameHandler.formUnlocked[i];

            forms[i].SetActive(unlocked);
            formsDisabled[i].SetActive(!unlocked);
        }
    }

    void Update()
    {
        for (int i = 0; i < forms.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) )
            {
                if (GameHandler.formUnlocked[i])
                {
                    HandleSelection(i);
                } else {
                    // Shake the disabled icon horizontally
                    RectTransform disabledRect = formsDisabled[i].GetComponent<RectTransform>();
                    if (disabledRect != null)
                    {
                        LeanTween.cancel(disabledRect);
                        Vector3 originalPos = disabledRect.anchoredPosition;

                        // Wiggle back and forth
                        float shakeAmount = 5f;
                        float shakeDuration = 0.3f;

                        LeanTween.moveLocalX(disabledRect.gameObject, originalPos.x + shakeAmount, shakeDuration / 4f)
                            .setLoopPingPong(2)
                            .setOnComplete(() => {
                                disabledRect.anchoredPosition = originalPos;
                            });
                    }
                }

            }
        }
    }

    void HandleSelection(int newIndex)
    {
        // if (!GameHandler.transformCooldownOver) { return; }

        if (newIndex == currentIndex)
        {
            glows[newIndex].SetActive(false);
            AnimateFormDown(newIndex);
            currentIndex = -1;
        }
        else
        {
            if (currentIndex != -1)
            {
                glows[currentIndex].SetActive(false);
                AnimateFormDown(currentIndex);
            }

            glows[newIndex].SetActive(true);
            AnimateFormUp(newIndex);
            currentIndex = newIndex;
        }
    }

    void AnimateFormUp(int index)
    {
        RectTransform formRect = forms[index].GetComponent<RectTransform>();
        RectTransform glowRect = glows[index].GetComponent<RectTransform>();
        LeanTween.cancel(formRect);
        LeanTween.cancel(glowRect);

        Vector2 startPos = formRect.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0f, yOffset);

        LeanTween.move(formRect, targetPos, duration).setEase(easeType);
        LeanTween.move(glowRect, targetPos, duration).setEase(easeType);
    }

    void AnimateFormDown(int index)
    {
        RectTransform formRect = forms[index].GetComponent<RectTransform>();
        RectTransform glowRect = glows[index].GetComponent<RectTransform>();
        LeanTween.cancel(formRect);
        LeanTween.cancel(glowRect);

        Vector2 startPos = formRect.anchoredPosition;
        Vector2 targetPos = startPos - new Vector2(0f, yOffset);

        LeanTween.move(formRect, targetPos, duration).setEase(easeType);
        LeanTween.move(glowRect, targetPos, duration).setEase(easeType);
    }
}