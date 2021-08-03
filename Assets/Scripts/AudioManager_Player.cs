using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Player : MonoBehaviour
{
    public AudioSource playerAttacksAS; // "AS" stands for "Audio Source" and is included for quick understanding of what the variable is
    public AudioSource playerDamagedAS;

    public AudioClip playerAttackSoundEffect; // sound effect for player's base attack
    public AudioClip playerPwrAttackSoundEffect; // sound effect for player's power attack
    public AudioClip playerDamagedSoundEffect;  // sound effect for when player sustains damage

    // Start is called before the first frame update
    void Start()
    {
        playerAttacksAS = gameObject.AddComponent<AudioSource>();
        playerDamagedAS = gameObject.AddComponent<AudioSource>();

        playerAttacksAS.playOnAwake = false; //make sure the sources don't play upon Awake
        playerDamagedAS.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBaseAttackSoundEffect ()
    {
        playerAttacksAS.clip = playerAttackSoundEffect;
        playerAttacksAS.Play();
    }

    public void PlayPwrAttackSoundEffect()
    {
        playerAttacksAS.clip = playerPwrAttackSoundEffect;
        playerAttacksAS.Play();
    }

    public void PlayPCTakingDamageSoundEffect()
    {
        playerDamagedAS.clip = playerDamagedSoundEffect;
        playerDamagedAS.Play();
    }
   
}
