using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmuletIcon : MonoBehaviour
{
    public int id;
    public bool isSelected;
    public bool isUnlocked;
    public Image selectbox;
    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        selectbox.enabled = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unlock() {
        gameObject.SetActive(true);
        selectbox.enabled = true;
    }

    public void select(int target) {
        if (target == id) {
            isSelected = true;
            selectbox.enabled = true;
            Debug.Log("selected");
        } else {
            isSelected = false;
            selectbox.enabled = false;
        }
    }
}
