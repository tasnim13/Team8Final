using System.Collections;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    private bool hasInteracted = false;
    private bool playerInRange = false;
    private PlayerMove playerMove;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerMove = other.GetComponent<PlayerMove>();
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
        if (playerInRange && !hasInteracted && Input.GetKeyDown(KeyCode.M))
        {
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
        Debug.Log("Cat interaction started.");
        yield return new WaitForSeconds(2f); // Match pet animation duration

        GameHandler.playerHealth += 10;

        GameHandler gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();

        if (gameHandler.playerHealthBar != null)
        {
            gameHandler.playerHealthBar.UpdateHealthBar();
            Debug.Log("Health bar updated!");
        }
        else
        {
            Debug.LogWarning("PlayerHealthBar is null in GameHandler.");
        }

        Destroy(gameObject);
    }
}
