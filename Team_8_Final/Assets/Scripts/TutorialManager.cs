using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public static TutorialManager Instance;

    public GameObject tutorialUI;
    public TextMeshProUGUI messageText;
    public GameObject nextButton;
    public float typeSpeed = 0.03f;

    private string fullMessage;
    private bool isTyping = false;
    private bool hasFinishedTyping = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        tutorialUI.SetActive(false);
    }

    public void ShowMessage(string message) {
        Time.timeScale = 0f;
        fullMessage = message;
        messageText.text = "";
        tutorialUI.SetActive(true);
        nextButton.SetActive(true);
        StartCoroutine(TypeMessage());
    }

    IEnumerator TypeMessage() {
        isTyping = true;
        hasFinishedTyping = false;
        foreach (char c in fullMessage) {
            messageText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        isTyping = false;
        hasFinishedTyping = true;
    }

    public void OnNextButtonClicked() {
        if (isTyping) {
            StopAllCoroutines();
            messageText.text = fullMessage;
            isTyping = false;
            hasFinishedTyping = true;
        } else if (hasFinishedTyping) {
            tutorialUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
