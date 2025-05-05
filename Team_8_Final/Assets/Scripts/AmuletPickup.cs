using UnityEngine;
using UnityEngine.SceneManagement;

public class AmuletPickup : MonoBehaviour
{
    private bool alreadyCollected = false;

    void Start()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;

        // Disable amulet if already collected in this level
        if (GameHandler.levelCompleted[levelIndex])
        {
            alreadyCollected = true;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyCollected) return;

        if (other.CompareTag("Player"))
        {
            AmuletManager.Instance.CollectAmulet();

            // Mark this level as completed
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            GameHandler.levelCompleted[levelIndex] = true;
            PlayerPrefs.SetInt("LevelCompleted_" + levelIndex, 1);
            PlayerPrefs.Save();

            Destroy(gameObject);
            alreadyCollected = true;
        }
    }
}
