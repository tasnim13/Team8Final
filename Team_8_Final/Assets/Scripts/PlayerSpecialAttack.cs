using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : MonoBehaviour
{
    public GameObject roarProjectile;
    public GameObject falconProjectile;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float flaconProjSpd = 10f;

    public bool canSpecial = true;
    public bool useSpecial;
    public float specialCooldown = 10f;

    private float cooldownStartTime = -Mathf.Infinity;

    private Collider2D coll;
    private SpriteRenderer messageRend;
    public GameObject messageEffect;

    void Start() {
        coll = GetComponent<Collider2D>();

        //Cache message effect
        if (messageEffect != null) {
            messageRend = messageEffect.GetComponent<SpriteRenderer>();
            Color c = messageRend.color;
            c.a = 0f;
            messageRend.color = c;
        }
    }

    void LateUpdate() {
        if (messageEffect != null) {
            messageEffect.transform.rotation = Quaternion.identity;
        }
    }

    public void defaultSpatk() {
        if (canSpecial) {
            // Check for contact with "Cat"
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            Collider2D[] results = new Collider2D[5];
            int count = coll.OverlapCollider(filter, results);

            bool touchingCat = false;
            for (int i = 0; i < count; i++) {
                if (results[i] != null && results[i].CompareTag("Cat")) {
                    touchingCat = true;
                    break;
                }
            }

            if (!touchingCat) {
                NoCatMessage();
            }

            StartCoroutine(PetPossible());
            StartCoroutine(StartSpecialCooldown());
        }
    }

    private void NoCatMessage() {
        if (messageEffect == null || messageRend == null) return;

        // Detach from player to prevent flipping and lock rotation
        messageEffect.transform.SetParent(null);
        messageEffect.transform.rotation = Quaternion.identity;

        // Reset position to current world location (optional)
        messageEffect.transform.position = transform.position;

        // Make fully visible
        Color c = messageRend.color;
        c.a = 1f;
        messageRend.color = c;

        // Reset scale before tween
        messageEffect.transform.localScale = Vector3.one;

        // Fade out alpha
        LeanTween.value(messageEffect, 1f, 0f, 2.5f).setOnUpdate((float a) => {
            Color fade = messageRend.color;
            fade.a = a;
            messageRend.color = fade;
        }).setEase(LeanTweenType.linear);

        // Scale up and reparent after complete
        LeanTween.scale(messageEffect, new Vector3(1.75f, 1.75f, 1f), 2.5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => {
                // Reparent to player to clean up hierarchy
                messageEffect.transform.SetParent(this.transform);
            });
    }

    public void lionessSpatk() {
        if (canSpecial) {
            StartCoroutine(multiRoar());
            StartCoroutine(StartSpecialCooldown());
        }
    }

    public void falconSpatk() {
        if (canSpecial) {
            falconSpecial();
            StartCoroutine(StartSpecialCooldown());
        }
    }

    private IEnumerator StartSpecialCooldown() {
        canSpecial = false;
        cooldownStartTime = Time.time;
        yield return new WaitForSeconds(specialCooldown);
        canSpecial = true;
    }

    private IEnumerator PetPossible() {
        useSpecial = true;
        yield return new WaitForSeconds(2f);
        useSpecial = false;
    }

    public float GetSpecialCooldownPercent() {
        float elapsed = Time.time - cooldownStartTime;
        if (elapsed >= specialCooldown) return 0f;
        return Mathf.Clamp01(1f - (elapsed / specialCooldown));
    }

    private void basicAttack(int num) {
        Vector2 fwd = Vector2.up;

        for (int i = 0; i < num; i++) {
            Vector3 orb_placement = new Vector3(0, 0, i * (360f / num));
            GameObject projectile = Instantiate(roarProjectile, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse);
            projectile.GetComponentInChildren<SpriteRenderer>().transform.Rotate(orb_placement);
            fwd = Quaternion.AngleAxis(360f / num, Vector3.forward) * fwd;
        }
    }

    IEnumerator multiRoar() {
        basicAttack(8);
        yield return new WaitForSeconds(0.15f);
        basicAttack(8);
        yield return new WaitForSeconds(0.15f);
        basicAttack(8);
    }

    void falconSpecial() {
        Vector2 direction = GetComponent<PlayerMove>().LastDirection;
        if (direction == Vector2.zero) direction = new Vector2(1, 0);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        GameObject proj = Instantiate(falconProjectile, firePoint.position, rot);

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null) {
            projRb.velocity = direction.normalized * flaconProjSpd;
        }
    }
}
