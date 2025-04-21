using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSlide : MonoBehaviour {
    public float slideSpeed = 2f;
    [Range(0f, 360f)] public float slideAngle = 0f;//Angle in degrees, clockwise from local right

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy")) {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null) {
                Vector2 slideDir = Quaternion.Euler(0, 0, slideAngle) * transform.right;
                slideDir.Normalize();

                //Project current velocity onto slide direction
                float currentSpeedAlongSlide = Vector2.Dot(rb.velocity, slideDir);

                //Only apply slide speed if it's slower than the desired slide speed
                if (currentSpeedAlongSlide < slideSpeed) {
                    float speedDiff = slideSpeed - currentSpeedAlongSlide;
                    rb.velocity += slideDir * speedDiff;
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Vector3 dir = Quaternion.Euler(0, 0, slideAngle) * transform.right;
        Vector3 start = transform.position;
        Vector3 end = start + dir.normalized * 2f;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.05f);
    }
}
