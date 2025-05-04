using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Amulet : MonoBehaviour{

      public GameHandler gameHandler;
      //public playerVFX playerPowerupVFX;
      public bool isHealthPickUp = true;
      public bool isSpeedBoostPickUp = false;

      public int amuletID = 0;
    //   1 is Wadjet,  Cobra
    //   2 is Khnum,   Ram
    //   3 is Horus,   Falcon
    //   4 is Sekhmet, Lioness

      public int healthBoost = 50;
      public float speedBoost = 2f;
      public float speedTime = 2f;

      void Start(){
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
            //playerPowerupVFX = GameObject.FindWithTag("Player").GetComponent<playerVFX>();
      }

      public void OnTriggerEnter2D (Collider2D other){
            if (other.gameObject.tag == "Player"){
                  PlayerForms forms = other.gameObject.GetComponent<PlayerForms>();
                  GetComponent<Collider2D>().enabled = false;
                  // TODO: the audio clip here gets cut off. Also
                  // we're using a TF2 one right now
                  GetComponent<AudioSource>().Play();
                  StartCoroutine(DestroyThis());

                  switch (amuletID) {
                    case 0:
                        // Debug.Log("picked up generic amulet!");
                        break;
                    case 1:
                        forms.unlock(1);
                        forms.ChangeForm(1);
                        // Debug.Log("Picked up Amulet of Wadjet!");
                        break;
                    case 2:
                        forms.unlock(2);
                        forms.ChangeForm(2);
                        // Debug.Log("Picked up Amulet of Khnum!");
                        break;
                    case 3:
                        forms.unlock(3);
                        forms.ChangeForm(3, false);
                        // Debug.Log("Picked up Amulet of Horus");
                        break;
                    case 4:
                        forms.unlock(4);
                        forms.ChangeForm(4, false);
                        // Debug.Log("Picked up Amulet of Sekhmet!");
                        break;
                    default:
                        Debug.Log("UH OH! Unknown Amulet type");
                        break;
                  }

                //   if (isHealthPickUp == true) {
                //         // gameHandler.playerGetHit(healthBoost * -1);
                //         //playerPowerupVFX.powerup();
                //   }

                //   if (isSpeedBoostPickUp == true) {
                //         // other.gameObject.GetComponent<PlayerMove>().speedBoost(speedBoost, speedTime);
                //         //playerPowerupVFX.powerup();
                //   }
            }
      }

      IEnumerator DestroyThis(){
        // TODO: why wait?
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
      }

}