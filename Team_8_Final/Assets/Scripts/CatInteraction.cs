using System.Collections;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    private bool hasInteracted = false;
    private bool playerInRange = false;
    private PlayerMove playerMove;
    private PlayerSpecialAttack spatk;
    private PlayerHealthBar playerHealthBar;

    void Start()
    {
        playerHealthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBar>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[CatInteraction] Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("[CatInteraction] Player entered trigger.");
            playerInRange = true;
            playerMove = other.GetComponent<PlayerMove>();
            spatk = other.GetComponent<PlayerSpecialAttack>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[CatInteraction] Player exited trigger.");
            playerInRange = false;
            playerMove = null;
        }
    }

    void Update()
    {
        // Logging state for debugging
        Debug.Log($"[CatInteraction] InRange: {playerInRange}, HasInteracted: {hasInteracted}, UseSpecial: {spatk?.useSpecial}");

        if (playerInRange && !hasInteracted && spatk != null && Input.GetKey(KeyCode.M))
        {
            Debug.Log("[CatInteraction] Interacting with cat...");
            hasInteracted = true;

            if (playerMove != null)
            {
                playerMove.Pet(); // Trigger animation
            }

            StartCoroutine(BoostHealth());
        }
    }

    IEnumerator BoostHealth()
    {
        yield return new WaitForSeconds(5f); // Wait for animation

        int oldHealth = GameHandler.playerHealth;

        if (GameHandler.playerHealth < 100)
        {
            GameHandler.playerHealth = Mathf.Min(GameHandler.playerHealth + 10, 100);
            Debug.Log($"[CatInteraction] Player health increased from {oldHealth} to {GameHandler.playerHealth}.");
        }
        else
        {
            Debug.Log("[CatInteraction] Player health already full. No health added.");
        }

        playerHealthBar.UpdateHealthBar();

        if (spatk != null)
        {
            spatk.useSpecial = false;
        }

        Debug.Log("[CatInteraction] Cat has been petted and will now disappear.");
        Destroy(gameObject);
    }
}
