using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoCarousel : MonoBehaviour {
    [Header("Panels & Displays")]
    public GameObject enemyInfoPanel;
    public GameObject categoryPanel;         // Holds the 3 category buttons
    public GameObject carouselPanel;         // Holds the image, left/right buttons, etc.
    public Image cardImageDisplay;

    [Header("Card Data")]
    public List<Sprite> enemyCards;
    public List<Sprite> formCards;
    public List<Sprite> controlCards;

    [Header("Buttons")]
    public Button leftButton;
    public Button rightButton;
    public Button closeButton;
    public Button enemyButton;
    public Button formButton;
    public Button controlButton;

    private List<Sprite> currentCards;
    private int currentIndex = 0;

    void Start() {
        enemyInfoPanel.SetActive(false);
        leftButton.onClick.AddListener(ShowPrevious);
        rightButton.onClick.AddListener(ShowNext);
        closeButton.onClick.AddListener(ClosePanel);
        enemyButton.onClick.AddListener(() => OpenCategory(enemyCards));
        formButton.onClick.AddListener(() => OpenCategory(formCards));
        controlButton.onClick.AddListener(() => OpenCategory(controlCards));
    }

    public void OpenPanel() {
        enemyInfoPanel.SetActive(true);
        categoryPanel.SetActive(true);
        carouselPanel.SetActive(false);
    }

    public void ClosePanel() {
        enemyInfoPanel.SetActive(false);
        categoryPanel.SetActive(false);
        carouselPanel.SetActive(false);
    }

    private void OpenCategory(List<Sprite> cards) {
        currentCards = cards;
        currentIndex = 0;
        categoryPanel.SetActive(false);
        carouselPanel.SetActive(true);
        UpdateDisplay();
    }

    void ShowPrevious() {
        if (currentCards == null || currentCards.Count == 0) return;
        currentIndex = (currentIndex - 1 + currentCards.Count) % currentCards.Count;
        UpdateDisplay();
    }

    void ShowNext() {
        if (currentCards == null || currentCards.Count == 0) return;
        currentIndex = (currentIndex + 1) % currentCards.Count;
        UpdateDisplay();
    }

    void UpdateDisplay() {
        if (currentCards == null || currentCards.Count == 0) return;
        cardImageDisplay.sprite = currentCards[currentIndex];
    }
}