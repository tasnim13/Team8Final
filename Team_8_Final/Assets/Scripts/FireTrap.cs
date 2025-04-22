using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour {
    [Header("Trap Settings")]
    public float warningDuration = 3f;
    public float fireDuration = 1f;
    public float cooldownDuration = 10f;

    [Header("References")]
    public SpriteRenderer holeRenderer;
    public SpriteRenderer glowRenderer;
    public Color hotColor = Color.red;
    public GameObject fireBurstPrefab;
    public Transform fireSpawnPoint;

    private Color originalColor;
    private bool isActive = false;

    void Start() {
        //Cache original color of the hole
        originalColor = holeRenderer.color;

        //Start fire trap cycle
        StartCoroutine(FireCycle());
    }

    IEnumerator FireCycle() {
        while (true) {
            //Fade in glow
            HeatUpHole();

            //Wait for heat-up duration
            yield return new WaitForSeconds(warningDuration);

            //Spawn fire and make it active
            GameObject fire = Instantiate(fireBurstPrefab, fireSpawnPoint.position, Quaternion.identity);
            fire.transform.SetParent(transform);
            isActive = true;

            //Destroy the fire after its active time
            Destroy(fire, fireDuration);

            //Wait while fire is active
            yield return new WaitForSeconds(fireDuration);
            isActive = false;

            //Fade out glow
            CoolDownHole();

            //Wait for full cooldown
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    void HeatUpHole() {
        //Fade in the glow
        LeanTween.value(glowRenderer.gameObject, 0f, 1f, warningDuration).setOnUpdate((float a) => {
            Color c = glowRenderer.color;
            c.a = a;
            glowRenderer.color = c;
        });
    }

    void CoolDownHole() {
        //Fade out the glow
        LeanTween.value(glowRenderer.gameObject, 1f, 0f, 1f).setOnUpdate((float a) => {
            Color c = glowRenderer.color;
            c.a = a;
            glowRenderer.color = c;
        });
    }
}