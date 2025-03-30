using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetle : EnemyParent
{

    [Header ("Beetle-Specific Stats")]
    //This are the upper and lower bounds of the randomizer for jump time
    public float jumpTimeMin = 0f;
    public float jumpTimeMax = 10f;
    private float lastJumpTime = 0f;
    //This float will be randomized by the script
    private float jumpCooldown = 0f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
    }

    public override void OnTriggerExit2D(Collider2D collision) {
        base.OnTriggerExit2D(collision);
    }

    public override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();
    }
}
