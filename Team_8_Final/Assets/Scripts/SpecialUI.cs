using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUI : MonoBehaviour
{
    public GameObject[] icons; // icon[0] = default, [1] = lioness, [2] = falcon
    public Image cooldownOverlay;

    private int curr = -1;
    private RectTransform overlayRect;
    private PlayerSpecialAttack spatk;

    void Start() {
        spatk = GameObject.FindWithTag("Player").GetComponent<PlayerSpecialAttack>();
        overlayRect = cooldownOverlay.GetComponent<RectTransform>();

        cooldownOverlay.type = Image.Type.Filled;
        cooldownOverlay.fillMethod = Image.FillMethod.Vertical;
        cooldownOverlay.fillOrigin = (int)Image.OriginVertical.Top;
        cooldownOverlay.fillAmount = 0f;

        for (int i = 0; i < 3; i++) {
            icons[i].SetActive(false);
        }
    }

    void Update() {
        // Determine active icon
        if (GameHandler.currForm == 0) curr = 0;
        else if (GameHandler.currForm == 3) curr = 1;
        else if (GameHandler.currForm == 4) curr = 2;
        else curr = -1;

        // Show correct icon and position overlay
        for (int i = 0; i < icons.Length; i++) {
            bool isActive = (i == curr);
            icons[i].SetActive(isActive);
        }

        // Update overlay fill amount
        if (curr != -1) {
            float fill = spatk.GetSpecialCooldownPercent();
            cooldownOverlay.fillAmount = fill;
        } else {
            cooldownOverlay.fillAmount = 0f;
        }
    }
}