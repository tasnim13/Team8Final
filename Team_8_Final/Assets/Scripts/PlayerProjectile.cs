using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour{

      public int damage = 45;
    //   public GameObject hitEffectAnim;
      public float SelfDestructTime = 0.3f;
      public float SelfDestructVFX = 0.5f;
      public SpriteRenderer projectileArt;
      public GameObject splatterPrefab;

      void Start(){
           projectileArt = GetComponentInChildren<SpriteRenderer>();
           StartCoroutine(SelfDestruct());
      }

      //if the bullet hits a collider, play the explosion animation, then destroy the effect and the bullet
      void OnTriggerEnter2D(Collider2D other){
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                  //gameHandlerObj.playerGetHit(damage);
                  other.gameObject.GetComponent<EnemyParent>().TakeDamage(damage);
            }
           if (other.gameObject.tag != "Player" && other.gameObject.tag != "Projectile") {
                Debug.Log("collided with " + other.gameObject.name);
                  //    if (other.gameObject.tag != "Enemy" || other.gameObject.tag != "Player") {
                  //gameObject.GetComponent<BoxCollider2D>().enabled = false;
                  //   GameObject animEffect = Instantiate (hitEffectAnim, transform.position, Quaternion.identity);
                  //   projectileArt.SetActive(false);
                  //Destroy (animEffect, 0.5);
                  //   StartCoroutine(SelfDestructHit(animEffect));
                  StartCoroutine(SelfDestruct());
            }


      }

      IEnumerator SelfDestructHit(GameObject VFX){
            //MakeSplat();
            yield return new WaitForSeconds(SelfDestructVFX);
            Destroy (VFX);
            Destroy (gameObject);
      }

      IEnumerator SelfDestruct(){
            yield return new WaitForSeconds(SelfDestructTime);
            Destroy (gameObject);
      }

      /*
      //Make a mark on the ground where the projectile hit
      void MakeSplat(){
            GameObject splat = Instantiate (splatterPrefab, transform.position, Quaternion.identity);
            float zRotation = Random.Range(0f,179f);
            splat.transform.eulerAngles = new Vector3(0, 0, zRotation);
            float size = Random.Range(0.5f,0.9f);
            splat.transform.localScale = new Vector3(splat.transform.localScale.x * size, splat.transform.localScale.y * size, 1);
      }
      */
} 