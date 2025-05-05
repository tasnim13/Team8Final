using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class InteractableDoor : MonoBehaviour
{
    public string NextLevel = "MainMenu";
    public GameObject msgPressE;
    public GameObject msgNeedKey;

    private GameHandler gh;

    public bool canPressE = true;

    private float msgTimer = 0f;
    private float msgDuration = 2f; 

    void Start()
    {
        gh = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        msgPressE.SetActive(false);
        msgNeedKey.SetActive(false);
    }
public class InteractableDoor : MonoBehaviour{

        public string NextLevel = "MainMenu";
        public GameObject msgPressE;
        public bool canPressE =false;
        public FMOD.Studio.Bus musicBus;

       void Start(){
              msgPressE.SetActive(false);
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

        void OnTriggerExit2D(Collider2D other){
              if (other.gameObject.tag == "Player"){
                     msgPressE.SetActive(false);
                     canPressE = false;
              }
        }

      public void EnterDoor(){
            if (NextLevel == "level4_2") // allows for stopping old music to make room for new music at specific scene
            {
                musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                Debug.Log("stopping music" + NextLevel);
            }
            SceneManager.LoadScene (NextLevel);
      }

}