 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameHandler_PauseMenu : MonoBehaviour {

        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        public AudioMixer mixer;
        public static float volumeLevel = 1.0f;
        private Slider sliderVolumeCtrl;

        void Awake(){
                pauseMenuUI.SetActive(true); // so slider can be set
                SetLevel (volumeLevel);
                GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
                if (sliderTemp != null){
                        sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
                        sliderVolumeCtrl.value = volumeLevel;
                }
        }

        void Start(){
                pauseMenuUI.SetActive(false);
                GameisPaused = false;
        }

        void Update(){
                if (Input.GetKeyDown(KeyCode.Escape)){
                        if (GameisPaused){
                                Resume();
                        }
                        else{
                                Pause();
                        }
                }
        }

        public void Pause(){
              if (!GameisPaused){
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameisPaused = true;}
             else{ Resume ();}
             //NOTE: This added conditional is for a pause button
        }

        public void Resume(){
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GameisPaused = false;
        }

        public void SetLevel(float sliderValue){
                mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
                volumeLevel = sliderValue;
        }
}