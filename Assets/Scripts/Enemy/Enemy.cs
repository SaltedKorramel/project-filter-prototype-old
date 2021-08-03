using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Cached component references
    Animator enemyAnimator;
    Color originalColor;

    //State
    public int health;
    [SerializeField] int damage = 1;
    [SerializeField] float flashTime; //how long the sprite flash upon taking damage will last
    public SpriteRenderer enemyRenderer;
    private float timeBetweenAttacks = 0.5f;
    private float attackAnimationDuration = 0.1f;

    private float nextAttackTime;
    private float endAttackAnimationTime;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;
    }

    private void Update()
    {
        ResetValues();
    }
    private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.tag == "Player" && (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking")) && (Time.time > nextAttackTime))
         {
            enemyAnimator.SetBool("Attacking", true);
            collision.GetComponent<Player>().TakeDamage(damage);
            nextAttackTime = Time.time + timeBetweenAttacks;
            endAttackAnimationTime = Time.time + attackAnimationDuration;
        }
        
    }  

    public void TakeDamage(int damageFromPlayer) {
        health -= damageFromPlayer;
        FlashWhite();
        Debug.Log("Enemy took Damage !! Current health:" + health);
        if (health <= 0)
        {

            Destroy(gameObject);
        }
    }

    private void ResetValues()
    {
        //used to reset any values that we don't need to keep active after executing something 
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && (Time.time > endAttackAnimationTime))
        {
            enemyAnimator.SetBool("Attacking", false);
        }
    }
    private void FlashWhite()
    {
        enemyRenderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    private void ResetColor()
    {
        enemyRenderer.color = originalColor;
    }

}
