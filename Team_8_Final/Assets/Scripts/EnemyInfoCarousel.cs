using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoCarousel : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject enemyInfoPanel;
    public Image enemyImageDisplay;
    public List<Sprite> enemyCards; // List of enemy info images
    public Button leftButton;
    public Button rightButton;
    public Button closeButton;

    private int currentIndex = 0;

    void Start()
    {
        enemyInfoPanel.SetActive(false); // Start hidden

        leftButton.onClick.AddListener(ShowPrevious);
        rightButton.onClick.AddListener(ShowNext);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        enemyInfoPanel.SetActive(true);
        currentIndex = 0;
        UpdateDisplay();
    }

    public void ClosePanel()
    {
        enemyInfoPanel.SetActive(false);
    }

    void ShowPrevious()
    {
        currentIndex = (currentIndex - 1 + enemyCards.Count) % enemyCards.Count;
        UpdateDisplay();
    }

    void ShowNext()
    {
        currentIndex = (currentIndex + 1) % enemyCards.Count;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        enemyImageDisplay.sprite = enemyCards[currentIndex];
    }
}
