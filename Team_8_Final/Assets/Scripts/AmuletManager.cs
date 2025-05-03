using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    public static AmuletManager Instance { get; private set; }

    public int totalAmuletsCollected = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Optional if levels persist across scenes
        }
    }

    public void CollectAmulet()
    {
        totalAmuletsCollected++;
        Debug.Log("Amulet collected! Total: " + totalAmuletsCollected);
    }
}
