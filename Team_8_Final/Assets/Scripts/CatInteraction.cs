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
        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("Pet"))
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
            Debug.Log("Cat interaction started.");
            hasInteracted = true;

            if (playerMove != null)
            {
                playerMove.Pet(); // Trigger the petting animation
            }

            StartCoroutine(BoostHealth());
        }
    }

    IEnumerator BoostHealth()
    {
        yield return new WaitForSeconds(5f); // Wait for animation to finish

        int oldHealth = GameHandler.playerHealth;

        if (GameHandler.playerHealth < 100)
        {
            GameHandler.playerHealth = Mathf.Min(GameHandler.playerHealth + 10, 100);
            Debug.Log($"Player health increased from {oldHealth} to {GameHandler.playerHealth}.");
        }
        else
        {
            Debug.Log("Player health is already full. No health added.");
        }

        playerHealthBar.UpdateHealthBar();

        if (spatk != null)
        {
            spatk.useSpecial = false;
        }

        Debug.Log("Cat has been petted and will now disappear.");
        Destroy(gameObject);
    }
}
