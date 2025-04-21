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
    private PlayerSpecialAttack spatk;

    private GameHandler gh;
    private float cooldownTime;
    private bool cooldownOver;
    private float baseSpeed;

    public AmuletIcon ramIcon;
    public AmuletIcon cobraIcon;
    public AmuletIcon falconIcon;
    public AmuletIcon lionessIcon;

    void Start()
    {
        currForm = 0;
        playermove = GetComponent<PlayerMove>();
        spatk = GetComponent<PlayerSpecialAttack>();
        gh = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        baseSpeed = playermove.moveSpeed;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        cooldownTime = 1f;
        cooldownOver = true;
    }

    public void unlock(int id) {
        if (id < 1 || id > 4) {
            Debug.Log("Uh OH! unlock error");
            return;
        }

        GameHandler.formUnlocked[id - 1] = true;

        switch (id) {
            case 1:
                cobraIcon.unlock();
                break;
            case 2:
                ramIcon.unlock();
                break;
            case 3:
                falconIcon.unlock();
                break;
            case 4:
                lionessIcon.unlock();
                break;
        }

        ramIcon.select(id);
        cobraIcon.select(id);
        falconIcon.select(id);
        lionessIcon.select(id);
    }

    IEnumerator ChangeFormWithCooldown(int id) {
        cooldownOver = false;
        ChangeForm(id);
        yield return new WaitForSeconds(cooldownTime);
        cooldownOver = true;
    }

    void Update()
    {
        if (cooldownOver) {
            if (GameHandler.formUnlocked[2] && Input.GetKeyDown("1")) {
                StartCoroutine(ChangeFormWithCooldown(3));
            } else if (GameHandler.formUnlocked[3] && Input.GetKeyDown("2")) {
                StartCoroutine(ChangeFormWithCooldown(4));
            } else if (GameHandler.formUnlocked[0] && Input.GetKeyDown("3")) {
                StartCoroutine(ChangeFormWithCooldown(1));
            } else if (GameHandler.formUnlocked[1] && Input.GetKeyDown("4")) {
                StartCoroutine(ChangeFormWithCooldown(2));
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            switch (currForm) {
                case 1:
                    spatk.snakeAttack();
                    break;
                case 2:
                    spatk.ramAttack();
                    break;
                case 3:
                    spatk.falconAttack();
                    break;
                case 4:
                    spatk.roarAttack();
                    break;
            }
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
            }
        } else {
            Debug.Log("Uh oh! Unknown form ID");
        }
    }
}