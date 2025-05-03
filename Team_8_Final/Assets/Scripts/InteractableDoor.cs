using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class InteractableDoor : MonoBehaviour{

        public string NextLevel = "MainMenu";
        public GameObject msgPressE;
        public bool canPressE =false;
        public FMOD.Studio.Bus musicBus;

       void Start(){
              msgPressE.SetActive(false);
              musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/MX");
        }

       void Update(){
              if ((canPressE == true) && (Input.GetKeyDown(KeyCode.E))){
                     EnterDoor();
              }
        }

        void OnTriggerEnter2D(Collider2D other){
              if (other.gameObject.tag == "Player"){ ;
                     msgPressE.SetActive(true);
                     canPressE =true;
              }
        }

        void OnTriggerExit2D(Collider2D other){
              if (other.gameObject.tag == "Player"){
                     msgPressE.SetActive(false);
                     canPressE = false;
              }
        }

      public void EnterDoor(){
            if (NextLevel == "level4_2") // allows for stopping old music to make room for new music at specific scene
            {
                musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                Debug.Log("stopping music" + NextLevel);
            }
            SceneManager.LoadScene (NextLevel);
      }

}