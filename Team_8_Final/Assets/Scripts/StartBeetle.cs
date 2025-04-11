using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBeetle : MonoBehaviour
{
    public Transform[] pathPts;
    public float moveSpeed = 2.5f;
    private int ptsIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pathPts[ptsIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (ptsIndex <= pathPts.Length - 1) {
            transform.position = Vector2.MoveTowards(transform.position, 
                                                     pathPts[ptsIndex].transform.position, 
                                                     moveSpeed * Time.deltaTime);

            if (transform.position == pathPts[ptsIndex].transform.position) {
                ptsIndex++;
            }
        }
        if (ptsIndex == pathPts.Length) {
            ptsIndex = 0;
        }
    } 
}
