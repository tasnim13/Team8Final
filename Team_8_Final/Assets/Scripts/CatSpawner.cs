using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    [Header("Cat Settings")]
    public GameObject[] catPrefabs;
    public int maxCats = 1;

    [Header("Spawn Area Bounds")]
    public Vector2 spawnAreaMin = new Vector2(-10, -5);
    public Vector2 spawnAreaMax = new Vector2(10, 5);

    [Header("Space Checking")]
    public float checkRadius = 0.5f;
    public LayerMask obstacleLayer; 

    [Header("Audio")]
    public AudioSource spawnAudio;

    void Start()
    {
        Debug.Log("CatSpawner Start() called");

        if (catPrefabs.Length == 0)
        {
            Debug.LogWarning("No cat prefabs assigned!");
            return;
        }

        int catsSpawned = 0;
        int safetyCounter = 0;
        int maxAttempts = 100;

        while (catsSpawned < maxCats && safetyCounter < maxAttempts)
        {
            if (TrySpawnCat())
            {
                catsSpawned++;
            }
            safetyCounter++;
        }

        if (spawnAudio != null)
        {
            spawnAudio.Play();
        }

        Debug.Log($"Spawned {catsSpawned} cat(s) after {safetyCounter} attempts.");
    }

    bool TrySpawnCat()
    {
        Vector2 randomPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Check for space using OverlapCircle
        Collider2D hit = Physics2D.OverlapCircle(randomPos, checkRadius, obstacleLayer);
        if (hit != null)
        {
            // Space is occupied
            return false;
        }

        // Instantiate cat if space is free
        GameObject catToSpawn = catPrefabs[Random.Range(0, catPrefabs.Length)];
        Instantiate(catToSpawn, randomPos, Quaternion.identity);
        Debug.Log("Spawned cat at: " + randomPos);
        return true;
    }
}
