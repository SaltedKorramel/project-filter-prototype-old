using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config

    [SerializeField] float speed;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] GameObject pwrAttackVFX;
    private float timeBetweenAttacks = 1f;

    //Cached component references
    Rigidbody2D pcRigidbody;
    Animator pcAnimator;
    CapsuleCollider2D pcCollider;
    private GameObject chargeWindow;
    private GameObject gameplay;
    private GameObject playerAudioManager;


    float gravityScaleAtStart; //"at start" refers to the gravity at the Start() method; we have this so we can change gravity during ladder climb and prevent PC from falling down the ladder

    //State
    bool facingRight = true;

    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;


    private int jumpBoost;
    public int extraJumpValue;
    public float jumpForce;

    bool isTouchingFront;
    public Transform frontCheck;
    bool wallJumping;
    public float wallJumpTime;
    public float xWallForce;
    public float yWallForce;
    bool wallSliding;
    public float wallSlidingSpeed;

   

    public int health;
    public int maxHealth = 5;
    public int currenthealth;
    // public HealthBar healthBar;

    private bool attack;
    private bool pwrattack;
    float nextAttackTime;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    public int pcAttackDamage;
    public static int pcPwrAttackDamage;

    //public GameObject blood;
    //public GameObject deathEffect;
    public GameObject pickupEffect;
    //public GameObject swordSwingEffect;
    //public GameObject dropEffect;


    private void Start()
    {
        jumpBoost = extraJumpValue;
        
        //Current health values for UI Bar
        // currenthealth = maxHealth;
        //  healthBar.SetMaxHealth(maxHealth);

        pcRigidbody = GetComponent<Rigidbody2D>();
        pcAnimator = GetComponent<Animator>();
        pcCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = pcRigidbody.gravityScale; //"gravity scale" is listed in the inscpect of the PC's rigidbody 2D component
        gameplay = GameObject.Find("Gameplay");
        GameHandler_Setup gameHandlerScript = gameplay.GetComponent<GameHandler_Setup>();
        chargeWindow = gameHandlerScript.chargeWindow;


        playerAudioManager = GameObject.Find("Audio Manager_Player");


    }

    private void Update()
    {
        HandleInput();
        ClimbLadder();
        HandleAttacks();
        HandlePowerAttacks();
        ResetValues();
        
    }

    private void HandleInput() {
        float input = Input.GetAxisRaw("Horizontal");
        if (!this.pcAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
        {
            pcRigidbody.velocity = new Vector2(input * speed, pcRigidbody.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);


        //time variables and check ensures fairness in attacking across players with different processing powers
        if (Time.time > nextAttackTime)
        {
            if (Input.GetKey(KeyCode.K)) 
            {
                //Confirm that player is attacking
                attack = true;
            }
        }

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKey(KeyCode.LeftShift)) //key for power attack
            {
                
                if (FightCharge.TryRemoveFilledBar())
                {
                    Debug.Log("Bar spent~!");
                    pwrattack = true;
                }
            }
        }

        if (input != 0)
        {
            pcAnimator.SetBool("Running", true);
        }
        else
        {
            pcAnimator.SetBool("Running", false);
        }
        if (input > 0 && facingRight == false)
        {
            Flip();
        }
        else if (input < 0 && facingRight == true)
        {
            Flip();
        }

        if (isGrounded == true)
        {
            jumpBoost = extraJumpValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpBoost > 0)
        {
            pcRigidbody.velocity = Vector2.up * jumpForce;
            jumpBoost--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumpBoost == 0 && isGrounded == true)
        {
            pcRigidbody.velocity = Vector2.up * jumpForce;
        }


        if (isTouchingFront && !isGrounded && input != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }
        
        if (wallSliding)
        {
            pcRigidbody.velocity = new Vector2(pcRigidbody.velocity.x, Mathf.Clamp(pcRigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping)
        {
            pcRigidbody.velocity = new Vector2(xWallForce * -input, yWallForce);
        }

    }
    private void HandleAttacks() {
        if (attack && !this.pcAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
        {
            //Send the message to the Animator to activate the trigger parameter named "Attacking"
            pcAnimator.SetTrigger("Attacking");
            playerAudioManager.GetComponent<AudioManager_Player>().PlayBaseAttackSoundEffect();

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D col in enemiesToDamage)
            {
                col.GetComponent<Enemy>().TakeDamage(pcAttackDamage);
                FightCharge.AddFightCharge(23);
              
            }

            nextAttackTime = Time.time + timeBetweenAttacks;
            pcRigidbody.velocity = Vector2.zero;
        }

        
    }

    private void HandlePowerAttacks() {
        if (pwrattack && !this.pcAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
        {
            pcAnimator.SetTrigger("Attacking");
            TriggerPwrAttackVFX();
            playerAudioManager.GetComponent<AudioManager_Player>().PlayPwrAttackSoundEffect();

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D col in enemiesToDamage)
            {
                col.GetComponent<Enemy>().TakeDamage(pcPwrAttackDamage);
                

            }

            nextAttackTime = Time.time + timeBetweenAttacks;
            pcRigidbody.velocity = Vector2.zero;
        }
    
    }

    private void TriggerPwrAttackVFX()
    { 
        if(!pwrAttackVFX)
        {
            return;
        }
        pwrAttackVFX = Instantiate(pwrAttackVFX, transform.position, transform.rotation);
    
    }


    private void Flip() {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    private void ClimbLadder()
    {
        if(!pcCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            pcAnimator.SetBool("Climbing", false);
            pcRigidbody.gravityScale = gravityScaleAtStart; 
            //whenever you are not on ladder, gravity is as it was at Start(), i.e., gravity scale is set to default
            return; 
        
        }

        float input = Input.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(pcRigidbody.velocity.x, input * climbSpeed);
        pcRigidbody.velocity = climbVelocity;
        pcRigidbody.gravityScale = 0f; //prevents PC from slowly falling down the ladder if not y-axis input is given; the PC holds onto the ladder

        bool playerHasVerticalSpeed = Mathf.Abs(pcRigidbody.velocity.y) > Mathf.Epsilon;
        pcAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        pcAnimator.speed = 1f;

        if (pcRigidbody.gravityScale == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            pcAnimator.SetBool("Climbing", true); //prevents animation from 
            pcAnimator.speed = 0f; //makes PC look like they're actually stopped on the ladder
        }

    }

   private void SetWallJumpingToFalse() {
        wallJumping = false;
    }

    public void TakeDamage(int damage)
    {
        // FindObjectOfType<CameraShake>().Shake();
        health -= damage;
        playerAudioManager.GetComponent<AudioManager_Player>().PlayPCTakingDamageSoundEffect();
        Debug.Log("Player has taken damage! Current health:" + health);

        // healthBar.SetHealth(currenthealth);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        /*
                if (currenthealth <= 0)
                {
                    //Instantiate(deathEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                } else {
                    //Instantiate(blood, transform.position, Quaternion.identity);
                }
                */
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

       
    public void Equip(SafetyPinItem fightChargeSafetyPin)
    {
        // Instantiate(pickupEffect, transform.position, Quaternion.identity);
        pcPwrAttackDamage = SafetyPinItem.pwrAttackDamage;
        Destroy(fightChargeSafetyPin.gameObject); //destroys game object from scene upon pickup
        chargeWindow.SetActive(true); //this line may need to be moved once we introduce other safetypins
        
    }

    
    
    public void Land() {
        Vector2 pos = new Vector2(groundCheck.position.x, groundCheck.position.y + 1);
        //Instantiate(dropEffect, pos, Quaternion.identity);
    }

    private void ResetValues() {
         
        attack = false;
        pwrattack = false;
    }

}
