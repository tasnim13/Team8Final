using UnityEngine;

public class KeyDropper : MonoBehaviour
{
    public EnemyParent targetEnemy;   // Works with Croc, Beetle, Phoenix, etc.
    public GameObject keyPrefab;

    private bool hasDropped = false;

    void Update()
    {
        if (!hasDropped && targetEnemy != null)
        {
            if (targetEnemy.IsDead())
            {
                Debug.Log($"[KeyDropper] {targetEnemy.name} is dead. Dropping key.");
                DropKey();
            }
            else
            {
                Debug.Log($"[KeyDropper] {targetEnemy.name} is still alive. Key not yet dropped.");
            }
        }
    }

    void DropKey()
    {
        hasDropped = true;

        if (keyPrefab != null && targetEnemy != null)
        {
            Vector3 dropPosition = targetEnemy.transform.position;
            Instantiate(keyPrefab, dropPosition, Quaternion.identity);
            Debug.Log($"[KeyDropper] Key dropped at position {dropPosition}.");
        }
        else
        {
            Debug.LogWarning("[KeyDropper] KeyPrefab or targetEnemy was null! Cannot drop key.");
        }
    }
}
