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
        // if (!hasUnlocked && AmuletManager.Instance.totalAmuletsCollected >= amuletsRequiredToUnlock)
        if (!hasUnlocked && GameHandler.totalAmuletsCollected >= amuletsRequiredToUnlock)
        {
            hasUnlocked = true;
            UpdateVisual();
        }
    }

    public bool isUnlocked() {
        return hasUnlocked;
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
