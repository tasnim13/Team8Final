using System.Collections;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    private bool hasInteracted = false;
    private bool playerInRange = false;
    private PlayerMove playerMove;
    private PlayerSpecialAttack spatk;
    private PlayerHealthBar playerHealthBar;

    [Header("Heart Effect")]
    public GameObject heartEffect;
    private SpriteRenderer loveRenderer;

    void Start()
    {
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
        
        //Cache heart effect
        if (heartEffect != null) {
            loveRenderer = heartEffect.GetComponent<SpriteRenderer>();
            Color c = loveRenderer.color;
            c.a = 0f;
            loveRenderer.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("[CatInteraction] Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            // Debug.Log("[CatInteraction] Player entered trigger.");
            playerInRange = true;
            playerMove = other.GetComponent<PlayerMove>();
            spatk = other.GetComponent<PlayerSpecialAttack>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("[CatInteraction] Player exited trigger.");
            playerInRange = false;
            playerMove = null;
        }
    }

    void Update()
    {
        // Logging state for debugging
        // Debug.Log($"[CatInteraction] InRange: {playerInRange}, HasInteracted: {hasInteracted}, UseSpecial: {spatk?.useSpecial}");

        if (playerInRange && !hasInteracted && spatk.useSpecial)
        {
            // Debug.Log("[CatInteraction] Interacting with cat...");
            hasInteracted = true;

            if (playerMove != null)
            {
                playerMove.Pet(); // Trigger animation
            }

            StartCoroutine(BoostHealth());

            KittyLove();
        }
    }

    private void KittyLove() {
        if (heartEffect == null || loveRenderer == null) return;

        //Make fully visible
        Color c = loveRenderer.color;
        c.a = 1f;
        loveRenderer.color = c;

        //Reset scale to normal in case it's reused
        heartEffect.transform.localScale = Vector3.one;

        //Fade out alpha
        LeanTween.value(heartEffect, 1f, 0f, 2.5f).setOnUpdate((float a) => {
            Color fade = loveRenderer.color;
            fade.a = a;
            loveRenderer.color = fade;
        }).setEase(LeanTweenType.linear);

        //Scale up heart
        LeanTween.scale(heartEffect, new Vector3(1.75f, 1.75f, 1f), 2.5f).setEase(LeanTweenType.easeOutCubic);
    }

    IEnumerator BoostHealth()
    {
        yield return new WaitForSeconds(2f); // Wait for animation

        int oldHealth = GameHandler.playerHealth;

        if (GameHandler.playerHealth < 100)
        {
            GameHandler.playerHealth = Mathf.Min(GameHandler.playerHealth + 40, 100);
            // Debug.Log($"[CatInteraction] Player health increased from {oldHealth} to {GameHandler.playerHealth}.");
        }
        else
        {
            // Debug.Log("[CatInteraction] Player health already full. No health added.");
        }

        playerHealthBar.UpdateHealthBar();

        if (spatk != null)
        {
            spatk.useSpecial = false;
        }

        // Debug.Log("[CatInteraction] Cat has been petted and will now disappear.");
        Destroy(gameObject);
    }
}
