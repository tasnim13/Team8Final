using UnityEngine;

public class PyramidUnlocker : MonoBehaviour
{
    public int amuletsRequiredToUnlock = 1;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    private SpriteRenderer spriteRenderer;
    private bool hasUnlocked = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    void Update()
    {
        if (!hasUnlocked && AmuletManager.Instance.totalAmuletsCollected >= amuletsRequiredToUnlock)
        {
            hasUnlocked = true;
            UpdateVisual();
        }
    }

    void UpdateVisual()
    {
        if (hasUnlocked)
        {
            spriteRenderer.sprite = unlockedSprite;
        }
        else
        {
            spriteRenderer.sprite = lockedSprite;
        }
    }
}
