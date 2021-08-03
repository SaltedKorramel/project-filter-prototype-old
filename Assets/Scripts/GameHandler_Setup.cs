using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameHandler_Setup : MonoBehaviour
{
    public GameObject chargeWindow;
    public GameObject filterOverlay;
    public GameObject filterLadder;


    private void Awake()
    {
        chargeWindow = GameObject.Find("Charge Window");
        chargeWindow.SetActive(false); //Charge Window game object and thus ChargeWindow script are set as inactive 
        //so that they can only become active once the safety pin is picked up by the PC

        filterOverlay = GameObject.Find("Stage Filter"); //set filter as inactive
        filterOverlay.SetActive(false);

        filterLadder = GameObject.Find("Filter Ladder"); //set filter ladder as inactive
        filterLadder.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
