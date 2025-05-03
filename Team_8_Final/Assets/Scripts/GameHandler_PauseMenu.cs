 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using FMODUnity;
using FMODUnity;

public class GameHandler_PauseMenu : MonoBehaviour {

        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        public AudioMixer mixer;
        public static float volumeLevel = 1.0f;
        private Slider sliderVolumeCtrl;

        void Awake(){
                if (pauseMenuUI == null) {
                        Debug.LogWarning("pauseMenuUI was not assigned in the Inspector.");
                } else {
                        pauseMenuUI.SetActive(true); // Activate once to configure slider
                }

                SetLevel(volumeLevel);

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

        public void PauseButton() {
                if (GameisPaused) {
                        Resume();
                } else {
                        Pause();
                }
        }

        public void Pause() {
                if (!GameisPaused) {
                        pauseMenuUI.SetActive(true);
                        Debug.Log("pauseMenuUI activeSelf: " + pauseMenuUI.activeSelf);
                        Canvas canvas = pauseMenuUI.GetComponent<Canvas>();
                        if (canvas != null) {
                                Debug.Log("pauseMenuUI canvas enabled: " + canvas.enabled);
                        } else {
                                Debug.Log("pauseMenuUI has no Canvas component.");
                        }
                        Time.timeScale = 0f;
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Logic/Pause");
                FMODUnity.RuntimeManager.PlayOneShot("event:/Logic/Pause");
                GameisPaused = true;
                } else {
                        Resume();
                }
        }

        public void Resume(){
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GameisPaused = false;
                FMODUnity.RuntimeManager.PlayOneShot("event:/Logic/Unpause");
        }

        public void SetLevel(float sliderValue){
                //mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
                //volumeLevel = sliderValue;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterVol", sliderValue, false);
        }
}