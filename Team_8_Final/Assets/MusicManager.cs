using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    public void EnterCombat()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isCombat", 1f, false);
    }

    public void ExitCombat()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isCombat", 0f, false);
    }
}
