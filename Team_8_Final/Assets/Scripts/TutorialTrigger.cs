using UnityEngine;

public class TutorialTrigger : MonoBehaviour {
    [TextArea]
    public string tutorialText;//message to display

    public Sprite extraArtwork;//optional sprite

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (hasTriggered) return;
        if (other.CompareTag("Player")) {
            hasTriggered = true;
            TutorialManager.Instance.StartTutorial(tutorialText, extraArtwork);
            gameObject.SetActive(false);//disable trigger
        }
    }
}
