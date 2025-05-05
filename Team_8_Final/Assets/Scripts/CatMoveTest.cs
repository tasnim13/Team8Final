using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMoveTest : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (anim != null)
        {
            anim.Play("PurpleCatAnim"); 
        }
        else
        {
            Debug.LogWarning("No Animator found on cat!");
        }
    }
}
