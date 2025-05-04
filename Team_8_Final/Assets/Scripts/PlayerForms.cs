using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForms : MonoBehaviour {

    [Header("Forms Speed Multipliers")]
    public float falconSpd = 1f;
    public float lionessSpd = 1f;
    public bool shouldChangeSprite = true;
    public Sprite[] formSprites;
    public RuntimeAnimatorController[] formAnims;

    //Tracks which forms were previously unlocked to detect new unlocks
    [HideInInspector]
    public bool[] formUnlockedPreviously = new bool[4];

    private PlayerMove playermove;
    private PlayerSpecialAttack spatk;
    private PlayerAttack playerAttack;

    private GameHandler gh;
    private float baseSpeed;
    private bool startBS = false;

    void Start() {
        playermove = GetComponent<PlayerMove>();
        spatk = GetComponent<PlayerSpecialAttack>();
        gh = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        baseSpeed = playermove.moveSpeed;
        playerAttack = GetComponent<PlayerAttack>();

        GameHandler.transformCooldownTime = 1f;
        GameHandler.transformCooldownOver = true;

        //Initialize previous unlocks to false
        for (int i = 0; i < 4; i++) {
            formUnlockedPreviously[i] = false;
        }

    }

    //Unlocks a form without activating it
    public void unlock(int id) {
        if (id < 1 || id > 4) {
            Debug.Log("Uh OH! unlock error");
            return;
        }

        GameHandler.formUnlocked[id - 1] = true;
    }

    IEnumerator ChangeFormWithCooldown(int id, bool allowToggle = false) {
        GameHandler.transformCooldownOver = false;
        ChangeForm(id, allowToggle);
        yield return new WaitForSeconds(GameHandler.transformCooldownTime);
        GameHandler.transformCooldownOver = true;
    }

    void Update() {
        //Only run this once to restore form state on load
        if (!startBS) {
            if (GameHandler.currForm != 0) {
                ChangeForm(GameHandler.currForm, false);
            }
            startBS = true;
        }

        /* if (GameHandler.transformCooldownOver) {
            if (GameHandler.formUnlocked[0] && Input.GetKeyDown("1")) {
                StartCoroutine(ChangeFormWithCooldown(1, true));
            } else if (GameHandler.formUnlocked[1] && Input.GetKeyDown("2")) {
                StartCoroutine(ChangeFormWithCooldown(2, true));
            } else if (GameHandler.formUnlocked[2] && Input.GetKeyDown("3")) {
                StartCoroutine(ChangeFormWithCooldown(3, true));
            } else if (GameHandler.formUnlocked[3] && Input.GetKeyDown("4")) {
                StartCoroutine(ChangeFormWithCooldown(4, true));
            }
        } */

        if (Input.GetKeyDown(KeyCode.F)) {
            switch (GameHandler.currForm) {
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

    //Changes the form and handles sprite + speed
    public void ChangeForm(int id, bool allowToggle = true) {
        Debug.Log("Change form called for form #: " + id);
        if (id == 0 || id == 3 || id == 4) {
            /* if (allowToggle && GameHandler.currForm == id) {
                id = 0;
            } */

            if (shouldChangeSprite) {
                playermove.changePlayerSprite(formSprites[id], formAnims[id]);
            }

            GameHandler.currForm = id;

            switch (id) {
                case 0:
                    playermove.moveSpeed = baseSpeed;
                    break;
                case 3:
                    playermove.moveSpeed = baseSpeed * falconSpd;
                    break;
                case 4:
                    playermove.moveSpeed = baseSpeed * lionessSpd;
                    break;
            }
        } else {
            Debug.Log("Uh oh! Unknown form ID");
        }
    }
} 