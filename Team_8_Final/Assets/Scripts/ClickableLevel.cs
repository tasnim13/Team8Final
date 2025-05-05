using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableLevel : MonoBehaviour
{
    public string sceneToLoad = "Level1";

    public GameObject highlightObject;
    public PyramidUnlocker pylock;
    // TODO: highlight only if unlocked?

    void Start()
    {
        // Hide highlight at start
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        // Debug.Log(sceneToLoad);
        // return;
        if (!string.IsNullOrEmpty(sceneToLoad) && pylock.isUnlocked())
        {
            Debug.Log("Loading scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene name set in LevelSelectClickable.");
        }
    }
}
