using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLadderCollider : MonoBehaviour
{

    private PlatformEffector2D effector;
    public float movingDownWaitTime; 

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S)) 
        {
            movingDownWaitTime = 0.5f;
        }
        
        if (Input.GetKey(KeyCode.S)) 
        {
            if (movingDownWaitTime <= 0)
            {
                effector.rotationalOffset = 180f; 
                movingDownWaitTime = 0.5f;
            }
            else {
                movingDownWaitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.W)) 
        {
            effector.rotationalOffset = 0f; 
        }
    }
}
