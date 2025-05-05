using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonsTween_AlphaScale : MonoBehaviour{
       public AnimationCurve curveScale = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
       public AnimationCurve curveAlpha = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
       float elapsed = 0f;
       Vector3 startScale;
       Image thisImage;
       private Text buttonText; // legacy version

       public bool isButton1 = false;
       bool doButton1 = false;
       public bool isButton2 = false;
       bool doButton2 = false;
       public bool isButton3 = false;
       bool doButton3 = false;
       public bool isButton4 = false;
       bool doButton4 = false;

       float timer = 0;
       float button1Timer = 1f;
       float button2Timer = 5f;
       float button3Timer = 10f;
       float button4Timer = 11f;
       // float button1Timer = 10f;
       // float button2Timer = 17f;
       // float button3Timer = 24f;
       // float button4Timer = 24f + 0.5f;

       void Start(){
              startScale = transform.localScale;
              thisImage = GetComponent<Image>();
              thisImage.color = new Color(2.55f, 2.55f, 2.55f, 0f);
              buttonText = GetComponentInChildren<Text>(); // legacy version
              buttonText.color = new Color(2.55f, 2.55f, 2.55f, 0f);
       }

       void FixedUpdate () {
              timer += Time.deltaTime;
              if (timer >= button1Timer){doButton1 = true;}
              if (timer >= button2Timer){doButton2 = true;}
              if (timer >= button3Timer){doButton3 = true;}
              if (timer >= button4Timer){doButton4 = true;}

              if (
                     ((isButton1) && (doButton1))
                     || ((isButton2) && (doButton2))
                     || ((isButton3) && (doButton3))
                     || ((isButton4) && (doButton4))
              ){
               // Tween Move:
            //    transform.localScale = startScale * curveScale.Evaluate(elapsed);

               // Tween Alpha:
               if (elapsed <= 1f){
                    float newAlpha = curveAlpha.Evaluate(elapsed);
                    thisImage.color = new Color(2.55f, 2.55f, 2.55f, newAlpha);
                    buttonText.color = new Color(2.55f, 2.55f, 2.55f, newAlpha);
                }
                elapsed += Time.deltaTime;
              }
       }
}
