using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForms : MonoBehaviour
{
    public int currForm;
    public bool shouldChangeSprite = false;
    private SpriteRenderer spriteRenderer;
    public Sprite[] formSprites;
    private PlayerMove playermove;
    // private PlayerMoveNoAnim playermove;

    private bool isUnlockedCobra;
    private bool isUnlockedRam;
    private bool isUnlockedFalcon;
    private bool isUnlockedLioness;

    private float cooldownTime;
    private bool cooldownOver;
    private float baseSpeed;

    public AmuletIcon ramIcon;
    public AmuletIcon cobraIcon;
    public AmuletIcon falconIcon;
    public AmuletIcon lionessIcon;

    // Start is called before the first frame update
    void Start()
    {
        currForm = 0;
        playermove = GetComponent<PlayerMove>();
        // playermove = GetComponent<PlayerMoveNoAnim>();
        baseSpeed = playermove.moveSpeed;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        isUnlockedCobra = false;
        isUnlockedRam = false;
        isUnlockedFalcon = false;
        isUnlockedLioness = false;

        cooldownTime = 1f;
        cooldownOver = true;
    }

    public void unlock(int id) {
        switch (id) {
            case 0:
                break;
            case 1:
                isUnlockedCobra = true;
                cobraIcon.unlock();
                break;
            case 2:
                isUnlockedRam = true;
                ramIcon.unlock();
                break;
            case 3:
                isUnlockedFalcon = true;
                falconIcon.unlock();
                break;
            case 4:
                isUnlockedLioness = true;
                lionessIcon.unlock();
                break;
            default:
                Debug.Log("Uh OH! unlock error");
                break;
        }
        ramIcon.select(id);
        cobraIcon.select(id);
        falconIcon.select(id);
        lionessIcon.select(id);
    }


    IEnumerator ChangeFormWithCooldown(int id) {
        cooldownOver = false;
        // TODO: do I need safety checks or whatever?
        ChangeForm(id);
        yield return new WaitForSeconds(cooldownTime);
        cooldownOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownOver) {
            if (isUnlockedCobra && Input.GetKeyDown("1")) {
                // cooldownOver = false;
                // ChangeForm(1);
                StartCoroutine(ChangeFormWithCooldown(1));
            } else if (isUnlockedRam && Input.GetKeyDown("2")) {
                StartCoroutine(ChangeFormWithCooldown(2));
            } else if (isUnlockedFalcon && Input.GetKeyDown("3")) {
                StartCoroutine(ChangeFormWithCooldown(3));
            } else if (isUnlockedLioness && Input.GetKeyDown("4")) {
                StartCoroutine(ChangeFormWithCooldown(4));
            } 
            // StartCoroutine(holdup(cooldownTime));
        }
    }

    private void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite; 
    }


    public void ChangeForm(int id) {
        if (0 <= id && id <= 4) {
            if (shouldChangeSprite) {
                ChangeSprite(formSprites[id]);
            }

            ramIcon.select(id);
            cobraIcon.select(id);
            falconIcon.select(id);
            lionessIcon.select(id);

            currForm = id;
            switch (id) {
                case 0:
                    Debug.Log("TRANSFORMING: GENERIC");
                    playermove.moveSpeed = baseSpeed;
                    break;
                case 1:
                    Debug.Log("TRANSFORMING: Cobra");
                    playermove.moveSpeed = baseSpeed;
                    break;
                case 2:
                    Debug.Log("TRANSFORMING: Ram");
                    playermove.moveSpeed = baseSpeed * 1.5f;
                    break;
                case 3:
                    Debug.Log("TRANSFORMING: Falcon");
                    playermove.moveSpeed = baseSpeed / 3f;
                    break;
                case 4:
                    Debug.Log("TRANSFORMING: Lioness");
                    playermove.moveSpeed = baseSpeed * 2f;
                    break;
                default:
                    break;
            }
        } else {
            Debug.Log("Uh oh! Unknown form ID");
        }
    }
}
