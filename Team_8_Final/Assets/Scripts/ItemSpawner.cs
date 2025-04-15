using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;    // This should be a prefab with SpriteRenderer
    public Sprite[] possibleSprites; // Drag all your item sprites here in inspector
    public int maxItems = 3;

    void Start()
    {
        List<int> usedIndexes = new List<int>();

        for (int i = 0; i < maxItems; i++)
        {
            int randomIndex;

            // Ensure no duplicate sprites
            do {
                randomIndex = Random.Range(0, possibleSprites.Length);
            } while (usedIndexes.Contains(randomIndex));

            usedIndexes.Add(randomIndex);

            SpawnItem(possibleSprites[randomIndex]);
        }
    }

    void SpawnItem(Sprite spriteToUse)
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(-8f, 8f),
            Random.Range(-4f, 4f)
        );

        GameObject newItem = Instantiate(itemPrefab, randomPosition, Quaternion.identity);
        newItem.GetComponent<SpriteRenderer>().sprite = spriteToUse;
    }
}
