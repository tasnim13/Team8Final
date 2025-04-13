using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Function to call when the image is clicked")]
    public UnityEvent onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
