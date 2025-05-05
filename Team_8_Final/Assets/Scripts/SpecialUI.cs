using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUI : MonoBehaviour
{
    public GameObject[] icons;
    private int curr = -1;

    void Start() {
        for (int i = 0; i < 3; i++) {
            icons[i].SetActive(false);
        }
    }

    void Update()
    {
        if (GameHandler.currForm == 0) curr = 0;
        if (GameHandler.currForm == 3) curr = 1;
        if (GameHandler.currForm == 4) curr = 2;
        for (int i = 0; i < 3; i++) {
            if (curr == i) {
                icons[i].SetActive(true);
            } else {
                icons[i].SetActive(false);
            }
        }
    }
}