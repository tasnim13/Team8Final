using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemArt; // Assign file_art here
    public int maxItems = 2; // Limit to 3 spawns

    void Start()
    {
        for (int i = 0; i < maxItems; i++) // Spawn exactly 3 items
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(-8f, 8f),  // X position range
            Random.Range(-4f, 4f)   // Y position range
        );

        Instantiate(itemArt, randomPosition, Quaternion.identity);
    }
}
