using UnityEngine;

public class CatInteractTest : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange)
        {
            Debug.Log("Player is within cat range");
        }

        if (playerInRange && Input.GetKey(KeyCode.M))
        {
            Debug.Log("Interacting with cat");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered cat trigger");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited cat trigger");
            playerInRange = false;
        }
    }
}
