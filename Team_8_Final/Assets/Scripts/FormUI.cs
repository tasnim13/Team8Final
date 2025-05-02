using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormUI : MonoBehaviour {
    public GameObject[] glows;
    public GameObject[] forms;
    public GameObject[] formsDisabled;

    private int currentIndex = -1;
    public LeanTweenType easeType;
    public float yOffset;
    public float duration;

    private Vector2[] formBasePositions;
    private Vector2[] glowBasePositions;
    private Vector2[] disabledBasePositions;

    void Awake() {
        //Cache base positions before anything moves
        formBasePositions = new Vector2[forms.Length];
        glowBasePositions = new Vector2[glows.Length];
        disabledBasePositions = new Vector2[formsDisabled.Length];

        for (int i = 0; i < forms.Length; i++) {
            RectTransform formRect = forms[i].GetComponent<RectTransform>();
            RectTransform glowRect = glows[i].GetComponent<RectTransform>();
            RectTransform disabledRect = formsDisabled[i].GetComponent<RectTransform>();

            formBasePositions[i] = formRect.anchoredPosition;
            glowBasePositions[i] = glowRect.anchoredPosition;
            disabledBasePositions[i] = disabledRect.anchoredPosition;
        }
    }

    void Start() {
        //Reset transforms to base positions
        for (int i = 0; i < forms.Length; i++) {
            forms[i].GetComponent<RectTransform>().anchoredPosition = formBasePositions[i];
            glows[i].GetComponent<RectTransform>().anchoredPosition = glowBasePositions[i];
            formsDisabled[i].GetComponent<RectTransform>().anchoredPosition = disabledBasePositions[i];
        }

        //Deactivate all glows
        for (int i = 0; i < glows.Length; i++) {
            glows[i].SetActive(false);
        }

        //Enable only forms 2 and 3 (indices 2 and 3)
        for (int i = 0; i < forms.Length; i++) {
            if (i < 2) {
                forms[i].SetActive(false);
                formsDisabled[i].SetActive(false);
            } else {
                bool unlocked = GameHandler.formUnlocked[i];
                forms[i].SetActive(unlocked);
                formsDisabled[i].SetActive(!unlocked);
            }
        }
    }

    void Update() {
        //Only check forms 2 and 3 (keys 3 and 4)
        for (int i = 2; i < forms.Length; i++) {
            bool unlocked = GameHandler.formUnlocked[i];

            //Update UI when a form is unlocked mid-game
            if (unlocked && formsDisabled[i].activeSelf) {
                forms[i].SetActive(true);
                formsDisabled[i].SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                if (unlocked) {
                    HandleSelection(i);
                } else {
                    //Shake the disabled icon
                    RectTransform disabledRect = formsDisabled[i].GetComponent<RectTransform>();
                    if (disabledRect != null) {
                        LeanTween.cancel(disabledRect);
                        Vector3 basePos = disabledBasePositions[i];

                        float shakeAmount = 5f;
                        float shakeDuration = 0.3f;

                        LeanTween.moveLocalX(disabledRect.gameObject, basePos.x + shakeAmount, shakeDuration / 4f)
                            .setLoopPingPong(2)
                            .setOnComplete(() => {
                                disabledRect.anchoredPosition = basePos;
                            });
                    }
                }
            }
        }
    }

    public void HandleSelection(int newIndex) {
        if (newIndex == currentIndex) {
            glows[newIndex].SetActive(false);
            AnimateFormDown(newIndex);
            currentIndex = -1;
        } else {
            if (currentIndex != -1) {
                glows[currentIndex].SetActive(false);
                AnimateFormDown(currentIndex);
            }

            glows[newIndex].SetActive(true);
            AnimateFormUp(newIndex);
            currentIndex = newIndex;
        }
    }

    void AnimateFormUp(int index) {
        RectTransform formRect = forms[index].GetComponent<RectTransform>();
        RectTransform glowRect = glows[index].GetComponent<RectTransform>();
        LeanTween.cancel(formRect);
        LeanTween.cancel(glowRect);

        Vector2 targetFormPos = formBasePositions[index] + new Vector2(0f, yOffset);
        Vector2 targetGlowPos = glowBasePositions[index] + new Vector2(0f, yOffset);

        LeanTween.move(formRect, targetFormPos, duration).setEase(easeType);
        LeanTween.move(glowRect, targetGlowPos, duration).setEase(easeType);
    }

    void AnimateFormDown(int index) {
        RectTransform formRect = forms[index].GetComponent<RectTransform>();
        RectTransform glowRect = glows[index].GetComponent<RectTransform>();
        LeanTween.cancel(formRect);
        LeanTween.cancel(glowRect);

        LeanTween.move(formRect, formBasePositions[index], duration).setEase(easeType);
        LeanTween.move(glowRect, glowBasePositions[index], duration).setEase(easeType);
    }
}
