using System.Collections.Generic;
using UnityEngine;
using System.Collections;
 
// Quits the player when the user hits escape
 
public class QuitFunction : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}