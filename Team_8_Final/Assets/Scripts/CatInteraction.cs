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
            Debug.Log("Pet cat!");
            hasInteracted = true;

            if (playerMove != null)
            {
                playerMove.Pet();
            }

            StartCoroutine(BoostHealth());
        }
    }

    IEnumerator BoostHealth()
    {
        yield return new WaitForSeconds(2f); // Match pet animation duration

        if (GameHandler.playerHealth < 100)
        {
            GameHandler.playerHealth += 10;
        }

        playerHealthBar.UpdateHealthBar();

        if (spatk != null)
        {
            spatk.useSpecial = false;
        }

        Destroy(gameObject);
    }
}
