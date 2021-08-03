using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    public int numberOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite brokenHeart;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        //checks to make sure that the number of hearts(spoons) is equal to player health 
        //if not we will get error , so it is IMPORTANT that this matches.
        if (player.health > numberOfHearts)
        {
            player.health = numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }

            if (i < player.health)
            {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = brokenHeart;
            }

        }
    }

}
