using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScorpion : EnemyParent {
    [Header("Scorpion-Specific Stats")]
    public Material poisonMat;
    private SpriteRenderer playerSpriteRend;
    public float speedDecreaseScale = 0.4f;

    //Update uses the original damage logic (immediate if cooldown passed)
    public override void Update() {
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            StartCoroutine(PoisonPlayer());
            GameHandler.playerHealth -= damage;
            lastAttackTime = Time.time;
        }
    }

    //Override OnTriggerEnter2D to skip delay behavior
    public override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            isAttacking = true;
            //No cooldown reset here to preserve original behavior
        }
    }

    private IEnumerator PoisonPlayer() {
        Debug.Log("Player poisoned!");

        playerSpriteRend = target.GetComponentInChildren<SpriteRenderer>();
        Material originalMat = playerSpriteRend.sharedMaterial;

        playerSpriteRend.material = poisonMat;

        PlayerMove playerMoveScript = target.GetComponent<PlayerMove>();
        float originalSpeed = playerMoveScript.moveSpeed;
        playerMoveScript.moveSpeed = originalSpeed * 0.5f;

        yield return new WaitForSeconds(5f);

        Debug.Log("Player unpoisoned!");
        playerSpriteRend.material = originalMat;
        playerMoveScript.moveSpeed = originalSpeed;
    }
}