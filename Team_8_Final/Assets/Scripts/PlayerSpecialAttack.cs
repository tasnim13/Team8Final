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

    public void defaultSpatk() {
        if (canSpecial) {
            useSpecial = true;
            StartCoroutine(StartSpecialCooldown());
        }
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
