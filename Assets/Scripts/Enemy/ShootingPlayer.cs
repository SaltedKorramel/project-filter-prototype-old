using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ShootingPlayer : MonoBehaviour
{

    //Cached component references
    Animator enemyAnimator;

    //State
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform shotSpawnPoint;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float attackAnimationDuration;

    private float nextShotTime;
    private float endAttackAnimationTime;

    private void Awake()
    {
        enemy = GameObject.Find("Enemy");
        enemyAnimator = enemy.GetComponent<Animator>();
    }
   
    // Update is called once per frame
    void Update()
    {
        
        if (Time.time > nextShotTime)
        {
            Instantiate(fireBall, shotSpawnPoint.position, shotSpawnPoint.rotation);
            enemyAnimator.SetBool("Attacking", true);
            nextShotTime = Time.time + timeBetweenShots;
            endAttackAnimationTime = Time.time + attackAnimationDuration;
        }
        
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && Time.time > endAttackAnimationTime)
        {
            enemyAnimator.SetBool("Attacking", false);
        }
    }
}
