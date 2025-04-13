using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private GameObject player;
    public static int playerHealth = 100;
    public int StartPlayerHealth = 100;
    public GameObject healthText;

    public static int gotTokens = 0;
    public GameObject tokensText;

    public bool isDefending = false;

    public static bool stairCaseUnlocked = false;
    public static string lastLevelDied;

    [Header("Enemies to Defeat")]
    public GameObject[] enemiesToDefeat;

    private string sceneName;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainMenu")
        {
            playerHealth = StartPlayerHealth;
        }

        updateStatsDisplay();
    }

    public void playerGetTokens(int newTokens)
    {
        gotTokens += newTokens;
        updateStatsDisplay();
    }

    public void playerGetHit(int damage)
    {
        playerHealth -= damage;
        if (playerHealth >= 0)
        {
            updateStatsDisplay();
        }
        /*if (damage > 0)
        {
            player.GetComponent<PlayerHurt>().playerHit();
        }*/

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            updateStatsDisplay();
            playerDies();
        }
    }

    public void updateStatsDisplay()
    {
        Text healthTextTemp = healthText.GetComponent<Text>();
        healthTextTemp.text = "Player Health: " + playerHealth;
    }

    public void playerDies()
    {
        player.GetComponent<PlayerHurt>().playerDead();
        lastLevelDied = sceneName;
        StartCoroutine(DeathPause());
    }

    IEnumerator DeathPause()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("EndLose");
    }

    public void CheckEnemiesStatus()
    {
        foreach (GameObject enemy in enemiesToDefeat)
        {
            if (enemy != null)
            {
                return; // At least one enemy still alive
            }
        }

        // All enemies are dead and player is alive
        if (playerHealth > 0)
        {
            SceneManager.LoadScene("EndWin");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("MainMenu");
        playerHealth = StartPlayerHealth;
    }

    public void ReplayLastLevel()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene(lastLevelDied);
        playerHealth = StartPlayerHealth;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
