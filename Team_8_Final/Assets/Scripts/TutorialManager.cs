using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public static TutorialManager Instance;

    public GameObject tutorialUI;
    public TextMeshProUGUI messageText;
    public Button nextButton;

    public Image extraImage;//assign this in Inspector

    private string currentMessage;
    private bool isTyping = false;
    private bool hasTyped = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        tutorialUI.SetActive(false);
        extraImage.gameObject.SetActive(false);//hide artwork initially
    }

    public void StartTutorial(string message, Sprite optionalArt) {
        Time.timeScale = 0f;
        tutorialUI.SetActive(true);
        messageText.text = "";
        currentMessage = message;
        StartCoroutine(TypeText(message));

        if (optionalArt != null) {
            extraImage.sprite = optionalArt;
            extraImage.gameObject.SetActive(true);
        } else {
            extraImage.gameObject.SetActive(false);
        }

        hasTyped = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            OnNextPressed();
        }
    }

    public void OnNextPressed() {
        if (!hasTyped) {
            StopAllCoroutines();
            messageText.text = currentMessage;
            hasTyped = true;
        } else {
            tutorialUI.SetActive(false);
            extraImage.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private IEnumerator TypeText(string message) {
        isTyping = true;
        messageText.text = "";
        foreach (char c in message.ToCharArray()) {
            messageText.text += c;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        isTyping = false;
        hasTyped = true;
    }
}