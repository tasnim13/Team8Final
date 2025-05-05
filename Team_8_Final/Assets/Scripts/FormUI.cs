using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormUI : MonoBehaviour {

    public GameObject[] glows;
    public GameObject[] forms;
    public GameObject[] formsDisabled;

    public Image cooldownOverlayLioness;
    public Image cooldownOverlayFalcon;

    private int currentIndex = -1;
    public LeanTweenType easeType;
    public float yOffset;
    public float duration;

    private Vector2[] formBasePositions;
    private Vector2[] glowBasePositions;
    private Vector2[] disabledBasePositions;

    private PlayerForms playerForms;

    private bool[] isFormUp;

    void Awake() {
        formBasePositions = new Vector2[forms.Length];
        glowBasePositions = new Vector2[glows.Length];
        disabledBasePositions = new Vector2[formsDisabled.Length];
        isFormUp = new bool[forms.Length];

        for (int i = 0; i < forms.Length; i++) {
            formBasePositions[i] = forms[i].GetComponent<RectTransform>().anchoredPosition;
            glowBasePositions[i] = glows[i].GetComponent<RectTransform>().anchoredPosition;
            disabledBasePositions[i] = formsDisabled[i].GetComponent<RectTransform>().anchoredPosition;
            isFormUp[i] = false;
        }
    }

    void Start() {
        playerForms = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerForms>();

        for (int i = 0; i < forms.Length; i++) {
            forms[i].GetComponent<RectTransform>().anchoredPosition = formBasePositions[i];
            glows[i].GetComponent<RectTransform>().anchoredPosition = glowBasePositions[i];
            formsDisabled[i].GetComponent<RectTransform>().anchoredPosition = disabledBasePositions[i];
        }

        for (int i = 0; i < glows.Length; i++) {
            glows[i].SetActive(false);
        }

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

        if (GameHandler.currForm >= 3 && GameHandler.currForm <= 4) {
            int uiIndex = GameHandler.currForm - 1;
            glows[uiIndex].SetActive(true);
            AnimateFormUp(uiIndex);
            currentIndex = uiIndex;
            isFormUp[uiIndex] = true;
        }

        // Init overlays
        cooldownOverlayLioness.type = Image.Type.Filled;
        cooldownOverlayLioness.fillMethod = Image.FillMethod.Vertical;
        cooldownOverlayLioness.fillOrigin = (int)Image.OriginVertical.Top;
        cooldownOverlayLioness.fillAmount = 0f;

        cooldownOverlayFalcon.type = Image.Type.Filled;
        cooldownOverlayFalcon.fillMethod = Image.FillMethod.Vertical;
        cooldownOverlayFalcon.fillOrigin = (int)Image.OriginVertical.Top;
        cooldownOverlayFalcon.fillAmount = 0f;
    }

    void Update() {
        if (!playerForms.startBS) {
            if (GameHandler.currForm != 0) {
                playerForms.ChangeForm(GameHandler.currForm, false);
                AnimateFormUp(GameHandler.currForm - 1);
            }
            playerForms.startBS = true;
        }

        for (int i = 2; i < forms.Length; i++) {
            bool unlocked = GameHandler.formUnlocked[i];
            bool wasPreviouslyUnlocked = playerForms.formUnlockedPreviously[i - 2];

            if (unlocked && !wasPreviouslyUnlocked) {
                forms[i].SetActive(true);
                glows[i].SetActive(true);
                formsDisabled[i].SetActive(false);
                AnimateFormUp(i);
                playerForms.formUnlockedPreviously[i - 2] = true;
                if (i == 2) {
                    glows[3].SetActive(false);
                    AnimateFormDown(3);
                }
                currentIndex = i;
                playerForms.ChangeForm(i + 1, false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                if (unlocked && wasPreviouslyUnlocked) {
                    HandleSelection(i);
                } else {
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

        PlayerForms pf = GameObject.FindWithTag("Player").GetComponent<PlayerForms>();
        float fill = pf.GetFormsCooldownPercent();

        if (forms[2].activeSelf) {
            RectTransform iconRect = forms[2].GetComponent<RectTransform>();
            cooldownOverlayLioness.transform.position = iconRect.position;
            cooldownOverlayLioness.rectTransform.sizeDelta = new Vector2(100f, 100f);
            cooldownOverlayLioness.fillAmount = fill;
        }

        if (forms[3].activeSelf) {
            RectTransform iconRect = forms[3].GetComponent<RectTransform>();
            cooldownOverlayFalcon.transform.position = iconRect.position;
            cooldownOverlayFalcon.rectTransform.sizeDelta = new Vector2(100f, 100f);
            cooldownOverlayFalcon.fillAmount = fill;
        }
    }

    public void HandleSelection(int newIndex) {
        if (!GameHandler.transformCooldownOver) return;

        if (currentIndex == newIndex) {
            glows[newIndex].SetActive(false);
            AnimateFormDown(newIndex);
            currentIndex = -1;
            playerForms.ChangeForm(0, false);
        } else {
            if (currentIndex != -1) {
                glows[currentIndex].SetActive(false);
                AnimateFormDown(currentIndex);
            }
            glows[newIndex].SetActive(true);
            AnimateFormUp(newIndex);
            currentIndex = newIndex;
            playerForms.ChangeForm(newIndex + 1, false);
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
