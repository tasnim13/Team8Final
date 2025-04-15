using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScorpion : EnemyParent
{
    [Header ("Scorpion-Specific Stats")]
    public Material poisonMat;
    private SpriteRenderer playerSpriteRend;
    public float speedDecreaseScale = 0.4f;

    //FixedUpdate is same as parent except for poison function in attack range statement
    public override void Update() {
        //Deal damage when player is within attack range
        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
                // GetComponent<AudioSource>().Play();
                StartCoroutine(PoisonPlayer());
                GameHandler.playerHealth -= damage;
                //gameHandler.playerGetHit(damage);
                lastAttackTime = Time.time;
        }
    }

    private IEnumerator PoisonPlayer() {
        Debug.Log("Player poisoned!");

        // Get the player sprite renderer
        playerSpriteRend = target.GetComponentInChildren<SpriteRenderer>();

        // Cache the original shared material
        Material originalMat = playerSpriteRend.sharedMaterial;

        // Apply poison material (cloned to this instance)
        playerSpriteRend.material = poisonMat;

        // Slow player movement
        PlayerMove playerMoveScript = target.GetComponent<PlayerMove>();
        float originalSpeed = playerMoveScript.moveSpeed;
        playerMoveScript.moveSpeed = originalSpeed * 0.5f;

        yield return new WaitForSeconds(5f);

        Debug.Log("Player unpoisoned!");

        // Reset material correctly
        playerSpriteRend.material = originalMat;

        // Restore movement speed
        playerMoveScript.moveSpeed = originalSpeed;
    }

}