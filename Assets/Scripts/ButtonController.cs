using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool isActivated;
    public Animator buttonAnimator;
    private GameObject gameplay;
    private GameObject filterOverlay;
    private GameObject filterLadder;
    private GameObject unclimbableLadder;

    public void Start()
    {
        gameplay = GameObject.Find("Gameplay");
        GameHandler_Setup gameHandlerScript = gameplay.GetComponent<GameHandler_Setup>();
        filterOverlay = gameHandlerScript.filterOverlay;

        filterLadder = gameHandlerScript.filterLadder;
        unclimbableLadder = GameObject.Find("Unclimbable Ladder");
    }
    public void activateButton()
    { 
        if (!isActivated) //i.e., the button is not yet activated
        {
            isActivated = true;
            Debug.Log("Button is activated!");
            buttonAnimator.SetBool("isActivated", isActivated);
            activateFilter();
        }
    }

    private void activateFilter()
    {
        filterOverlay.SetActive(true);
        filterLadder.SetActive(true);
        unclimbableLadder.SetActive(false);
    }


}
