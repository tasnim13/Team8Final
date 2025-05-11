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
        // private Slider sliderVolumeCtrl;
        public Slider sliderVolumeCtrl;

        public static float currVol = 1.0f;

        void Awake(){
                GameisPaused = false; // RESET static flag when scene loads

                if (pauseMenuUI == null) {
                        Debug.LogWarning("pauseMenuUI was not assigned in the Inspector.");
                } else {
                        pauseMenuUI.SetActive(false); // Ensure it starts hidden
                }

                SetLevel(currVol);

                // GameObject sliderTemp = GameObject.FindGameObjectWithTag("PauseMenuSlider");
                // if (sliderTemp != null){
                //         sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
                sliderVolumeCtrl.value = currVol;
                // }
        }

        void Start(){
                pauseMenuUI.SetActive(false);
                GameisPaused = false;
                SetLevel(currVol);
                // Slider slider = GameObject.FindGameObjectWithTag("PauseMenuSlider").GetComponent<Slider>();
                // slider.value = currVol;

                // GameObject sliderTemp = GameObject.FindGameObjectWithTag("PauseMenuSlider");
                // Debug.Log("TRYING TO FIND SLIDER");
                // if (sliderTemp != null){
                //         Debug.Log("FOUND SLIDER");
                //         sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
                sliderVolumeCtrl.value = currVol;
                // }
        }

        void Update(){
                if (Input.GetKeyDown(KeyCode.Escape)){
                        Debug.Log("PRESSING ESCAPE");
                        Pause();
                        // if (GameisPaused){
                        //         Resume();
                        // }
                        // else{
                        //         Pause();
                        // }
                }
        }

        public void PauseButton() {
                Debug.Log("PAUSE BUTTON PRESSED");
                Pause();
                // if (GameisPaused) {
                //         Resume();
                // } else {
                //         Pause();
                // }
        }

        public void Pause() {
                Debug.Log("PAUSING");
                if (!GameisPaused) {
                        GameisPaused = true;
                        pauseMenuUI.SetActive(true);
                        Canvas canvas = pauseMenuUI.GetComponent<Canvas>();
                        if (canvas != null) {
                                Debug.Log("pauseMenuUI canvas enabled: " + canvas.enabled);
                        } else {
                                Debug.Log("pauseMenuUI has no Canvas component.");
                        }
                        Time.timeScale = 0f;
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Logic/Pause");
                } else {
                        Resume();
                }
                Debug.Log("pauseMenuUI activeSelf: " + pauseMenuUI.activeSelf);
        }

        public void Resume(){
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GameisPaused = false;
                FMODUnity.RuntimeManager.PlayOneShot("event:/Logic/Unpause");
        }

        public void SetLevel(float sliderValue){
                currVol = sliderValue;
                //mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
                //volumeLevel = sliderValue;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterVol", currVol, false);
        }
}