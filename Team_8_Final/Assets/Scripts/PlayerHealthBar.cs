using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image fillBar;     // Blue
    public Image damageBar;   // White overlay

    public float flashDelay = 0.1f;
    public float shrinkTime = 0.5f;

    private float previousHealth;
    private float maxHealth;

    private GameHandler gameHandler;

    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        maxHealth = gameHandler.StartPlayerHealth;
        previousHealth = GameHandler.playerHealth;
        UpdateBarInstant();
    }

    public void UpdateHealthBar()
    {
        float currentHealth = GameHandler.playerHealth;

        float previousPercent = previousHealth / maxHealth;
        float currentPercent = currentHealth / maxHealth;

        fillBar.fillAmount = currentPercent;

        damageBar.fillAmount = previousPercent;
        damageBar.color = Color.white;

        LeanTween.cancel(damageBar.gameObject);
        LeanTween.delayedCall(damageBar.gameObject, flashDelay, () => {
            LeanTween.value(damageBar.gameObject, previousPercent, currentPercent, shrinkTime)
                .setOnUpdate((float val) => {
                    damageBar.fillAmount = val;
                })
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() => damageBar.color = fillBar.color);
        });

        previousHealth = currentHealth;
    }


    private void UpdateBarInstant()
    {
        float percent = GameHandler.playerHealth / maxHealth;
        fillBar.fillAmount = percent;
        damageBar.fillAmount = percent;
    }
}
