using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LeanTweenType easeType;
    public float scale;
    public float durationIn;
    public float durationOut;

    void OnEnable() {
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        LeanTween.scale(gameObject, transform.localScale * scale, durationIn).setEase(easeType);
    }

    public void OnPointerExit(PointerEventData eventData) {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), durationOut);
    }
}
