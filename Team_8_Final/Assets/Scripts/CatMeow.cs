using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CatMeow : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audioSource != null && audioSource.clip != null)
            {
                //audioSource.Play();
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Meow");
            }
        }
    }
}
