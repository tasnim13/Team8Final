using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 42f; // player movement speed
    private Vector3 change; // player movement direction
    private Rigidbody2D rb2d;
    private Animator anim;
    private Renderer rend;
    private bool isAlive = true;

    [Header("Wall Collision")]
    public LayerMask wallLayer;
    public float skinWidth = 0.05f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");

            UpdateAnimationAndMove();

            // Flip sprite direction
            if (change.x > 0)
            {
                Vector3 newScale = transform.localScale;
                if (newScale.x < 0) newScale.x *= -1;
                transform.localScale = newScale;
            }
            else if (change.x < 0)
            {
                Vector3 newScale = transform.localScale;
                if (newScale.x > 0) newScale.x *= -1;
                transform.localScale = newScale;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Attack");
            }
        }
    }

    void UpdateAnimationAndMove()
    {
        if (!isAlive) return;

        if (change != Vector3.zero)
        {
              Vector2 moveDelta = change.normalized * moveSpeed * Time.deltaTime;

              // Perform a lookahead to prevent movement into walls
              RaycastHit2D[] hitResults = new RaycastHit2D[1];
              int hitCount = rb2d.Cast(moveDelta.normalized,
              new ContactFilter2D
              {
                     layerMask = wallLayer,
                     useLayerMask = true,
                     useTriggers = false
              },
              hitResults,
              moveDelta.magnitude + skinWidth);

              if (hitCount == 0)
              {
              rb2d.MovePosition(rb2d.position + moveDelta);
              }


            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    public void playerHit()
    {
        if (isAlive)
        {
            anim.SetTrigger("Hurt");
            StopCoroutine(ChangeColor());
            StartCoroutine(ChangeColor());
        }
    }

    public void playerDie()
    {
        if (!isAlive) return;

        anim.SetTrigger("Dead");
        isAlive = false;
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator ChangeColor()
    {
        rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        rend.material.color = Color.white;
    }
}