using UnityEngine;

public class PyramidClickHandler : MonoBehaviour
{
    public Canvas HomeVillage;  // Drag "Canvas" here
    public Canvas Canvas;   // Drag your new Canvas here (e.g., PyramidCanvas)

    private void OnMouseDown()
    {
        if (HomeVillage != null && Canvas != null)
        {
            HomeVillage.gameObject.SetActive(false);  // Hide current canvas
            Canvas.gameObject.SetActive(true);    // Show new canvas
        }
    }
}
