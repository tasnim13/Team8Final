using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class InteractableDoor : MonoBehaviour{
    public string NextLevel = "MainMenu";
    public GameObject msgPressE;
    public GameObject msgNeedKey;

    private GameHandler gh;

    public bool canPressE = true;

    private float msgTimer = 0f;
    private float msgDuration = 2f;

    public FMOD.Studio.Bus musicBus;

    void Start()
    {
        gh = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        msgPressE.SetActive(false);
        msgNeedKey.SetActive(false);
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/MX");
    }

    void Update()
    {
        if (canPressE && Input.GetKeyDown(KeyCode.Q))
        {


            Debug.Log(
                "hasKey = " + GameHandler.hasKey
            );


            if (GameHandler.hasKey)  // CHANGED HERE
            {
                EnterDoor();
            }
            else
            {
                ShowNeedKeyMessage();
            }
        }

        // Timer to hide the "Need Key" message
        if (msgNeedKey.activeSelf)
        {
            msgTimer += Time.deltaTime;
            if (msgTimer > msgDuration)
            {
                msgNeedKey.SetActive(false);
                msgTimer = 0f;
            }
        }
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            msgPressE.SetActive(true);
            canPressE = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            msgPressE.SetActive(false);
            canPressE = false;
            msgNeedKey.SetActive(false);
            msgTimer = 0f;
        }
    }

    void ShowNeedKeyMessage()
    {
        msgNeedKey.SetActive(true);
        msgTimer = 0f;
    }

    public void EnterDoor()
    {
        SceneManager.LoadScene(NextLevel);
    }
}