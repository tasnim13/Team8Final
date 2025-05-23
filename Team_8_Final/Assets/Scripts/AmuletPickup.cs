using UnityEngine;
using UnityEngine.SceneManagement;

public class AmuletPickup : MonoBehaviour
{
    private bool alreadyCollected = false;
    public int levelNum;

    void Start()
    {
        // int levelIndex = SceneManager.GetActiveScene().buildIndex;

        // Disable amulet if already collected in this level
        Debug.Log("levelCompleted: " + GameHandler.levelCompleted[levelNum - 1]);

        if (GameHandler.levelCompleted[levelNum - 1])
        {
            alreadyCollected = true;
            GameHandler.hasKey = true;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyCollected) return;

        if (other.CompareTag("Player"))
        {
            // AmuletManager.Instance.CollectAmulet();
            GameHandler.totalAmuletsCollected++;
            Debug.Log("Amulet collected! Total: " + GameHandler.totalAmuletsCollected);

            // Mark this level as completed
            // int levelIndex = SceneManager.GetActiveScene().buildIndex;
            GameHandler.levelCompleted[levelNum - 1] = true;

            Debug.Log("levelCompleted: " + GameHandler.levelCompleted[levelNum - 1]);


            // PlayerPrefs.SetInt("LevelCompleted_" + levelIndex, 1);
            // PlayerPrefs.Save();

            // this is so scuffed
            GameHandler.hasKey = true;
            Debug.Log(
                "hasKey = " + GameHandler.hasKey
            );

            Destroy(gameObject);
            alreadyCollected = true;
        }
    }
}
