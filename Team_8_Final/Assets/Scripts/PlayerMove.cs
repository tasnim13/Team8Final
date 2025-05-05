using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.animations;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 42f;//player movement speed
    private Vector3 change;//player movement direction
    private Rigidbody2D rb2d;
    private Animator anim;

    private Renderer rend;
    // private Renderer rendRam;
    // private Renderer rendCobra;
    // private Renderer rendFalcon;
    // private Renderer rendLioness;

    // private Renderer[] rendarr;

    public SpriteRenderer sprend;

    private bool isAlive = true;
    [Header("Poison Settings")]
    public Material poisonMat;
    private Material originalMat;
    private bool isPoisoned = false;
    private Coroutine poisonRoutine;
    public float poisonDuration = 5f;
    public float poisonSpeedMultiplier = 0.1f;
    private float poisonEffectMultiplier = 1f;

    private Vector2 lastDirection = Vector2.right;
    public Vector2 LastDirection => lastDirection;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();
        rb2d = GetComponent<Rigidbody2D>();
        originalMat = rend.material;

        if (sprend == null)
        {
            Debug.Log("UH OH! sprend is null!");
        }
        // Debug.Log(anim.name);
        // Debug.Log(anim.runtimeAnimatorController.name);
        // rendRam = rendarr[0];
        // rendCobra = rendarr[0];
        // rendFalcon = rendarr[0];


        rb2d = GetComponent<Rigidbody2D>();
        originalMat = rend.material;
        // Debug.Log("> Start got loaded! <");
    }

    void Update()
    {
        if (!isAlive) return;

        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Pet();
        }
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        UpdateAnimationAndMove();

        if (change.x > 0) {
            Vector3 newScale = transform.localScale;
            if (newScale.x < 0) newScale.x *= -1;
            transform.localScale = newScale;
        } else if (change.x < 0) {
            Vector3 newScale = transform.localScale;
            if (newScale.x > 0) newScale.x *= -1;
            transform.localScale = newScale;
        }

        if (change != Vector3.zero)
        {
            lastDirection = change.normalized;
        }
    }

    void UpdateAnimationAndMove() {
        if (!isAlive) return;

        if (change != Vector3.zero) {
            Vector3 moveDelta = change.normalized * moveSpeed * poisonEffectMultiplier * Time.deltaTime;
            rb2d.MovePosition(transform.position + moveDelta);
            anim.SetBool("Walk", true);
        } else {
            anim.SetBool("Walk", false);
        }
    }

    public void changePlayerSprite(Sprite formSprite, RuntimeAnimatorController formAnim) {
        sprend = GetComponentInChildren<SpriteRenderer>();
        // if (sprend == null) {
        //     Debug.Log("UH OH! sprend.sprite is null");
        // }
        // if (formSprite == null) {
        //     Debug.Log("UH OH! formSprite is null");
        // }
        sprend.sprite = formSprite;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = formAnim;
    }

    public void playerHit() {
        if (isAlive) {
            anim.SetTrigger("Hurt");
            StopCoroutine(ChangeColor());
            StartCoroutine(ChangeColor());
        }
    }

    public void playerDie() {
        if (!isAlive) return;

        anim.SetTrigger("Dead");
        isAlive = false;
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator ChangeColor() {
        rend.material.color = new Color(2.0f, 1.0f, 0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        rend.material.color = Color.white;
    }

    public void ApplyPoison() {
        Debug.Log(">>> ApplyPoison() called");
        if (poisonRoutine != null) {
            StopCoroutine(poisonRoutine);
        }
        poisonRoutine = StartCoroutine(HandlePoison());
    }

    private IEnumerator HandlePoison() {
        isPoisoned = true;

        rend.material = poisonMat;
        poisonEffectMultiplier = poisonSpeedMultiplier;
        Debug.Log("Speed multiplier set to: " + poisonEffectMultiplier);

        yield return new WaitForSeconds(poisonDuration);

        rend.material = originalMat;
        poisonEffectMultiplier = 1f;
        Debug.Log("Poison ended. Speed reset to normal.");
        isPoisoned = false;
        poisonRoutine = null;
    }

    public void Pet()
    {
        if (!isAlive) return;

        isPetting = true;
        anim.SetTrigger("Pet");
        StartCoroutine(EndPetAfterDelay(5f));
    }

    private IEnumerator EndPetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPetting = false;
    }
}

