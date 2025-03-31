using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;  // Adjustable speed in Inspector
    private Rigidbody2D rb;
    // private PlayerForms forms;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
        // forms = GetComponent<PlayerForms>();
    }

    void Update()
    {
        // Get input from WASD keys (Horizontal = A/D, Vertical = W/S)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Apply movement to Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
