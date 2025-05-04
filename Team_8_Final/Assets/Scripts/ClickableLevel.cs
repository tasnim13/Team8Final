using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectClickable : MonoBehaviour
{
    public string sceneToLoad = "Level1";

    public GameObject highlightObject;

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
        if (!string.IsNullOrEmpty(sceneToLoad))
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
