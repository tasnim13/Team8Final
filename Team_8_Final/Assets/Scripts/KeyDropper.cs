using UnityEngine;

public class KeyDropper : MonoBehaviour
{
    public EnemyParent targetEnemy;   // Works with Croc, Beetle, Phoenix, etc.
    public GameObject keyPrefab;

    private bool hasDropped = false;

    void Update()
    {
        if (!hasDropped && targetEnemy != null && targetEnemy.IsDead())
        {
            DropKey();
        }
    }

    void DropKey()
    {
        hasDropped = true;
        Instantiate(keyPrefab, transform.position, Quaternion.identity);
    }
}
