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
    public static bool hasKey = false;

    public static int gotTokens = 0;
    public GameObject tokensText;

    public GameObject[] ankhIcons;

    public bool isDefending = false;

    public static bool stairCaseUnlocked = false;
    public static string lastLevelDied;


    public bool isLastLevel = false;

    public FormUI formUI;
    public static int currForm = 0;
    public static int totalAmuletsCollected = 0;

    public PlayerHealthBar playerHealthBar;

    public static bool[] levelCompleted = new bool[4];
    public static bool[] formUnlocked = new bool[4];
    public static bool transformCooldownOver = true;
    public static float transformCooldownTime = 3f;

    [Header("Enemies to Defeat")]
    public GameObject[] enemiesToDefeat;

    private string sceneName;

    public void Awake()
    {
        // Only initialize if it hasn’t been modified yet
        bool allFalse = true;
        for (int i = 0; i < 4; i++) {
            if (formUnlocked[i]) {
                allFalse = false;
                break;
            }
        }

        if (allFalse) {
            for (int i = 0; i < 4; i++) {
                formUnlocked[i] = false;
            }
        }

        // Load level completion status from PlayerPrefs
        for (int i = 0; i < levelCompleted.Length; i++) {
            levelCompleted[i] = PlayerPrefs.GetInt("LevelCompleted_" + i, 0) == 1;
        }
    }


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainMenu")
        {
            playerHealth = StartPlayerHealth;
        }

        
        if (GameObject.FindWithTag("PlayerFormsUI") != null) {
            formUI = GameObject.FindWithTag("PlayerFormsUI").GetComponent<FormUI>();
        }
        
    }

    public void playerGetTokens(int newTokens)
    {
        gotTokens += newTokens;
    }

    public void playerGetHit(int damage)
    {
        playerHealth -= damage;
        /*if (damage > 0)
        {
            player.GetComponent<PlayerHurt>().playerHit();
        }*/

        playerHealthBar.UpdateHealthBar();

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            playerDies();
        }
    }

    public void playerDies()
    {
        player.GetComponent<PlayerHurt>().playerDead();
        player.GetComponent<PlayerMove>().playerDie();
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
        if (!isLastLevel) { return; }

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
        // SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("OverworldMap");
    }

    public void OpeningCutScene()
    {
        SceneManager.LoadScene("OpeningCutScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("MainMenu");
        playerHealth = StartPlayerHealth;
    }

    public void MarkLevelCompleted()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        levelCompleted[index] = true;
        PlayerPrefs.SetInt("LevelCompleted_" + index, 1);
        PlayerPrefs.Save();
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
