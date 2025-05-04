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

    private PlayerForms playerForms;

    //Tracks if each form is currently tweened "up"
    private bool[] isFormUp;

    void Awake() {
        //Cache base positions before anything moves
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

        //Enable forms based on unlock state
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

        //If a form was auto-selected on pickup, animate UI up and track it
        if (GameHandler.currForm >= 3 && GameHandler.currForm <= 4) {
            int uiIndex = GameHandler.currForm - 1;
            glows[uiIndex].SetActive(true);
            AnimateFormUp(uiIndex);
            currentIndex = uiIndex;
            isFormUp[uiIndex] = true;
        }
    }

    void Update() {
        //Only run this once to restore form state on load
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

            //On unlock: enable and animate upward
            if (unlocked && !wasPreviouslyUnlocked) {
                //Enable form visually
                forms[i].SetActive(true);
                glows[i].SetActive(true);
                formsDisabled[i].SetActive(false);
                //Tween up
                AnimateFormUp(i);
                playerForms.formUnlockedPreviously[i - 2] = true;
                //Tween other form down (should only ever unlock falcon after lioness)
                if (i == 2) {
                    glows[3].SetActive(false);
                    AnimateFormDown(3);
                }
                currentIndex = i;
                playerForms.ChangeForm(i + 1, false);
            }

            //Was 3 or 4 pressed
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
                if (unlocked && wasPreviouslyUnlocked) {
                    //Passed i == 2 or i ==3
                    HandleSelection(i);
                } else {
                    //Shake disabled icon
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
            Debug.Log("currentIndex: " + currentIndex + ", newIndex: " + i);
        }
    }

    //Handles form selection/deselection and UI animation
    //newIndex == 2 or newIndex == 3
    public void HandleSelection(int newIndex) {
        if (!GameHandler.transformCooldownOver) return;

        //Select the same form again (deselect)
        if (currentIndex == newIndex) {
            glows[newIndex].SetActive(false);
            AnimateFormDown(newIndex); //Tween down
            currentIndex = -1;
            playerForms.ChangeForm(0, false);
        //Switching to a different form
        } else {
            //Tween down previous selection if any
            if (currentIndex != -1) {
                glows[currentIndex].SetActive(false);
                AnimateFormDown(currentIndex);
            }
            //Tween up new selection
            glows[newIndex].SetActive(true);
            AnimateFormUp(newIndex);
            currentIndex = newIndex;
            playerForms.ChangeForm(newIndex + 1, false);
        }
    }

    //Toggles the tween state of a form icon (up & down)
    /* void ToggleFormTween(int index) {
        if (isFormUp[index]) {
            AnimateFormDown(index);
        } else {
            AnimateFormUp(index);
        }
        isFormUp[index] = !isFormUp[index];
    } */

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
