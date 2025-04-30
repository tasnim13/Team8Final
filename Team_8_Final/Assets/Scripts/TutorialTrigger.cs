using UnityEngine;

public class TutorialTrigger : MonoBehaviour {
    [TextArea]
    public string tutorialMessage;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            TutorialManager.Instance.ShowMessage(tutorialMessage);
            gameObject.SetActive(false);//disable after trigger
        }
    }
}