using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScorpion : EnemyParent {
    [Header("Scorpion-Specific Stats")]
    public Material poisonMat;
    public float speedDecreaseScale = 0.4f;

    //Tracks whether the first attack has already occurred
    private bool hasAttackedOnce = false;

    public override void Update() {
        if (isDead) return;

        //Only allow attack if cooldown has passed or it's the first contact
        if (isAttacking && (Time.time >= lastAttackTime + attackCooldown || !hasAttackedOnce)) {
            lastAttackTime = Time.time;
            hasAttackedOnce = true;

            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
            TriggerAttackEffect();

            //Apply poison effect using the PlayerMove script
            PlayerMove playerMoveScript = target.GetComponent<PlayerMove>();
            if (playerMoveScript != null) {
                playerMoveScript.poisonMat = poisonMat;
                playerMoveScript.poisonSpeedMultiplier = speedDecreaseScale;
                playerMoveScript.ApplyPoison();
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision) {
        if (isDead) return;

        //Enable attacking when the player enters the trigger
        if (collision.CompareTag("Player")) {
            isAttacking = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision) {
        //Stop attacking when the player exits the trigger
        if (collision.CompareTag("Player")) {
            isAttacking = false;
        }
    }
}