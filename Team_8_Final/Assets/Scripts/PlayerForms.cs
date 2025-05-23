using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.animations;
using FMODUnity;

public class PlayerForms : MonoBehaviour {

    [Header("Forms Stat Multipliers")]
    public float falconSpdMult = 1f;
    public float lionessSpdMult = 1f;
    //Attack assigned in FalconAttack prefab
    public int falconAtk = 0;
    public int lionessAtk = 7;
    public int falconRng = 0;
    public float lionessRng = 3f;

    [Header("Form Art")]
    private bool shouldChangeSprite = true;
    public Sprite[] formSprites;
    public RuntimeAnimatorController[] formAnims;

    //Tracks which forms were previously unlocked to detect new unlocks
    [HideInInspector]
    public bool[] formUnlockedPreviously = new bool[4];
    public bool startBS = false;

    private PlayerMove playermove;
    private PlayerSpecialAttack spatk;
    private PlayerAttack playerAttack;

    //private GameHandler gh;
    private float baseSpeed;
    private int baseAttack;
    private int baseRange = 1;
    private float transformCooldownStartTime = -Mathf.Infinity;
    
    void Start() {
        playermove = GetComponent<PlayerMove>();
        //gh = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        baseSpeed = playermove.moveSpeed;
        baseAttack = 10;
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
        transformCooldownStartTime = Time.time;
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Swap");
            } else if (GameHandler.formUnlocked[1] && Input.GetKeyDown("2")) {
                StartCoroutine(ChangeFormWithCooldown(2, true));
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Swap");
            } else if (GameHandler.formUnlocked[2] && Input.GetKeyDown("3")) {
                StartCoroutine(ChangeFormWithCooldown(3, true));
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Swap");
            } else if (GameHandler.formUnlocked[3] && Input.GetKeyDown("4")) {
                StartCoroutine(ChangeFormWithCooldown(4, true));
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Swap");
            }
        }

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
        } */
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
                    playerAttack.attackDamage = baseAttack;
                    playerAttack.attackRange = baseRange;
                    break;
                case 3:
                    playermove.moveSpeed = baseSpeed * falconSpdMult;
                    playerAttack.attackDamage = falconAtk;
                    playerAttack.attackRange = falconRng;
                    break;
                case 4:
                    playermove.moveSpeed = baseSpeed * lionessSpdMult;
                    playerAttack.attackDamage = lionessAtk;
                    playerAttack.attackRange = lionessRng;
                    break;
            }
        } else {
            Debug.Log("Uh oh! Unknown form ID");
        }
    }

    //Returns 1 when cooldown just started, 0 when done
    public float GetFormsCooldownPercent() {
        if (GameHandler.transformCooldownOver) return 0f;
        float elapsed = Time.time - transformCooldownStartTime;
        float percent = Mathf.Clamp01(1f - (elapsed / GameHandler.transformCooldownTime));
        return percent;
    }

} 