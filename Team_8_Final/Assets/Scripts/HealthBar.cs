using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Sprite[] healthSprites;
    private SpriteRenderer spriteRenderer;
    public Transform enemyParent;

    public int totalSegments = 5;
    private Vector3 barOffset;
    public float upOffset = 0f;

    public void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        barOffset = new Vector3(0f, upOffset, 0f);

        //Start with full health
        UpdateBar(1f);
    }

    public void UpdateBar(float healthPercent)
    {
        //Calculate index based on health percentage and total health bar sprites
        int index = Mathf.Clamp(Mathf.FloorToInt((1f - healthPercent) * totalSegments), 0, totalSegments - 1);
        spriteRenderer.sprite = healthSprites[index];
    }

    public void LateUpdate() {
        //Maintains a constant height above the parent
        transform.position = enemyParent.position + barOffset;
        //Maintains rotation independent of the parent
        transform.rotation = Quaternion.identity;
    }
}
