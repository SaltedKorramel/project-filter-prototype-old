using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyPinItem : MonoBehaviour
{
    
    [SerializeField] private float turnSpeed; // this for rotating the monocle in scene
    public static int pwrAttackDamage;

    // Update is called once per frame
    private void Start()
    {
        pwrAttackDamage = 5;
    }

    void Update()
    {
        {
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //on collision with player, the player will access all variables inside this script and 
            //make changes in the values it calls
            collision.GetComponent<Player>().Equip(this);
        }
    }
}
