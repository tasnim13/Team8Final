using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public GameObject healthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Text textTemp = healthText.GetComponent<Text>();
        textTemp.text = "Player Health: " + GameHandler.playerHealth;
    }
}
