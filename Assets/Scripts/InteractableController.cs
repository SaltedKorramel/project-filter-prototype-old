using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{
    public bool isInRange; //whether the PC is in range to interact with the object
    public KeyCode interactKey;
    public UnityEvent interactAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange) // If PC is in range to interact
        {
            if (Input.GetKeyDown(interactKey)) // And player presses key
            {
                interactAction.Invoke(); // fire event
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is now in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is no longer in range");
        }
    }
}
