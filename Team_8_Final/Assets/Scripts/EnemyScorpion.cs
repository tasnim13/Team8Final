using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScorpion : EnemyParent {
    [Header("Scorpion-Specific Stats")]
    public Material poisonMat;
    public float speedDecreaseScale = 0.4f;

    public override void Update() {
        if (isDead) return;

        if (isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            lastAttackTime = Time.time;
            gameHandler.playerGetHit(damage);
            playerHealthBar.UpdateHealthBar();
            TriggerAttackEffect();

            //Apply poison through PlayerMove script
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

        if (collision.CompareTag("Player")) {
            isAttacking = true;
        }
    }
}