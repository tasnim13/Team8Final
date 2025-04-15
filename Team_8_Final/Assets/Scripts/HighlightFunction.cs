using UnityEngine;

public class PyramidClickHandler : MonoBehaviour
{
    public Canvas Canvas; // Assign the new Canvas in Inspector
    public Canvas currentCanvas; // Assign the current Canvas in Inspector

    private void OnMouseDown()
    {
        // When player clicks on this GameObject
        if (newCanvas != null && currentCanvas != null)
        {
            currentCanvas.enabled = false; // Hide current canvas
            newCanvas.enabled = true;      // Show new canvas
        }
    }
}
