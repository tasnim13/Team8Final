using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    public FMODUnity.EventReference musicEvent;
    private FMOD.Studio.EventInstance instance;

    public FMOD.Studio.PARAMETER_ID layeringParamID;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        instance.start();
    }

    public void EnterCombat()
    {
        instance.setParameterByName("isCombat", 1f, false);
    }

    public void ExitCombat()
    {
        instance.setParameterByName("isCombat", 0f, false);
    }
}
