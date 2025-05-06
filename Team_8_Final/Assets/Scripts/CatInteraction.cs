using System.Collections;
using UnityEngine;
using FMODUnity;

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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pettingSound;     // Sound during interaction
    public AudioClip disappearSound;   // Sound when cat disappears

    void Start()
    {
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();

        // Cache heart effect
        if (heartEffect != null)
        {
            loveRenderer = heartEffect.GetComponent<SpriteRenderer>();
            Color c = loveRenderer.color;
            c.a = 0f;
            loveRenderer.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerMove = other.GetComponent<PlayerMove>();
            spatk = other.GetComponent<PlayerSpecialAttack>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerMove = null;
        }
    }

    void Update()
    {
        if (playerInRange && !hasInteracted && spatk != null && spatk.useSpecial)
        {
            hasInteracted = true;

            // Play petting sound
            if (audioSource != null && pettingSound != null)
            {
                audioSource.PlayOneShot(pettingSound);
            }

            if (playerMove != null)
            {
                playerMove.Pet(); // Trigger animation
            }

            StartCoroutine(BoostHealth());
            KittyLove();
        }
    }

    private void KittyLove()
    {
        if (heartEffect == null || loveRenderer == null) return;

        // Make fully visible
        Color c = loveRenderer.color;
        c.a = 1f;
        loveRenderer.color = c;

        // Reset scale to normal
        heartEffect.transform.localScale = Vector3.one;

        // Fade out heart
        LeanTween.value(heartEffect, 1f, 0f, 2.5f).setOnUpdate((float a) =>
        {
            Color fade = loveRenderer.color;
            fade.a = a;
            loveRenderer.color = fade;
        }).setEase(LeanTweenType.linear);

        // Scale up heart
        LeanTween.scale(heartEffect, new Vector3(1.75f, 1.75f, 1f), 2.5f).setEase(LeanTweenType.easeOutCubic);

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Meow");
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

        // Play disappear sound before destroying
        if (audioSource != null && disappearSound != null)
        {
            audioSource.PlayOneShot(disappearSound);
            Destroy(gameObject, disappearSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
