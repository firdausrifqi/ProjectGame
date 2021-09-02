using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    //bug setelah ambil orb (jika di ground2)
    
    [Header("Base")]
    [SerializeField] bool IsGrounded;
    MCTSAIV2 mcts;
    public bool playerisDead;
    public bool isDead = false;
    public bool findOrb;
    public float checkRadius;
    public Transform Orb;
    Transform Target;
    Animator anim;
    [SerializeField] Boss boss;
    Rigidbody2D rb;
    public OrbHealth orbHealth;
    float random;
    bool GetOrb = false;
    [SerializeField] PlayerAttacks PlayerAttacking;
    
    [Header("Jump")]
    public LayerMask whatIsGround;
    public Transform groundPosition;
    public float jumpForce;
    bool jumping = false;
    bool doJump = false;

    [Header("Follow")]
    float Speed = 3f;
    bool chaseplayer;

    [Header("HealthBar")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] public int currentHealth;
    public bool GetHurt = false;
    public GameObject deathEffect;
    public HealthBar healthBar;

    [Header("StaminaBar")]
    [SerializeField] int maxStamina = 10;
    [SerializeField] public float currentStamina;
    public SPbar sp;
    bool lowsp = false;
    bool mustRecharge = false;
    float regeneration = 1f;
    bool StartRegenSP = false;

    [Header("Attack")]
    bool canattack = false;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    float nextActionTime = 0f;
    public LayerMask enemyLayers;
    public float attackRange = 0.3f;
   // public bool GetHurt = false;
    public Transform attackPoint;

    [Header("Block")]
    bool isPlayerAttack;
    bool blocking;

    //HEADER MOVE AWAY
    private float minDistance = 3f;
    private float range;
    bool StopRegen = false;
    bool recharging = false;

    // Start is called before the first frame update
    void Start()
    {
        checktarget();
        mcts = GetComponent<MCTSAIV2>();
        random = Random.Range(0f, 9f);
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        boss = GetComponent<Boss>();
        //PlayerAttacking = Target.GetComponent<PlayerAttacks>();
        anim = GetComponent<Animator>();
        chaseplayer = false;
        currentStamina = maxStamina;
        healthBar.SetMaxHealth(currentHealth);
        sp.SetMaxStamina(currentStamina);
    }

    // Update is called once per frame
    void Update()
    {
        checktarget();
        // isPlayerAttack = PlayerAttacking.IsAttack();
         //Debug.Log(isPlayerAttack);
         IsGrounded = Physics2D.OverlapCircle(groundPosition.position, checkRadius, whatIsGround); //check ground
         playanims();
        //check orb sudah di dapat atau belum. 
         if (GetOrb)
         {
             if (Time.time >= nextActionTime)
             {
                 GetOrb = false;
             }
         }
        if(recharging)
        {
            if(currentStamina < maxStamina)
            {
                currentStamina += regeneration * Time.fixedDeltaTime;
                if(currentStamina > maxStamina)
                {
                    currentStamina = maxStamina;
                    recharging = false;
                }
                sp.SetStamina((int)currentStamina);
            }
        }
        else sp.SetStamina((int)currentStamina);
        
    }
    public void isiStamina(bool set)
    {   
        // checktarget();
        // moveaway(Target);
        recharging = set;
    }

    public void giveAction(float actionnumber)
    {
        if(actionnumber == 1f)
        {
            //Debug.Log("CABUT");
            checktarget();
            moveaway(Target);
        }
        else if(actionnumber == 2f)
        {
            //Debug.Log("KEJAR");
            checktarget();
            if(Target != null)
            {
                chaseobject(Target, false);
            }
        }
        else if (actionnumber == 3f)
        {
            //Debug.Log("AMBIL ORB");
            checkorb(); //check orb ada atau tidak
            if(findOrb)
            {
                chaseobject(Orb, true);
            }
        }
    }

    public void GiveHealth(int count)
    {
        currentHealth = currentHealth + count;
        currentStamina = currentStamina + 6;
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }
        if (currentStamina >= 10)
        {
            currentStamina = 10;
        }
        anim.SetBool("Idle", false);
        healthBar.SetHealth(currentHealth);
        sp.SetStamina((int)currentStamina);
        GetOrb = true;
        nextActionTime = Time.time + 1f / 1f;
        rb.position = this.rb.position;
    }

    public void TakeDamage(int Damage, bool check)
    {
        recharging = false;
        //GetHurt = true;
        // if(StartRegenSP == true)
        // {
        //     StartRegenSP = false;
        //     StopRegen = true;
        // }
        blocking = mcts.GetStats();
        //Debug.Log("BLOCKED");
        if (blocking)
        {
            anim.SetTrigger("Blocking");
            mcts.SetStats(false);
            // random = Random.Range(0f, 9f);
            // if (random <= 1.5f)
            // {
            //     anim.SetInteger("State",0);
            //     //Debug.Log("Counter Attack");
            //     blocking = false;
            //     anim.SetTrigger("Attack1");
            //     nextAttackTime = Time.time + 1f / attackRate;
            // }
            currentStamina -= 2;
            sp.SetStamina(currentStamina);
        }
        else
        {
            if (currentHealth <= 0)
            {
                Die();
            }
            else if (currentHealth != 0)
            {
                currentHealth -= Damage;
                anim.SetTrigger("Hurt");
                healthBar.SetHealth(currentHealth);
                //healthBar.SetHealth(currentHealth);

                if (currentHealth <= 0)
                {
                    Die();
                }
            }
        }
        //GetHurt = false;
    }

    void playanims()
    {
        anim.SetBool("Grounded", IsGrounded);
    }

    void checktarget()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerAttacking = Target.GetComponent<PlayerAttacks>();
        }
        if (GameObject.FindGameObjectWithTag("PlayerW") != null)
        {
            Target = GameObject.FindGameObjectWithTag("PlayerW").transform;
            PlayerAttacking = Target.GetComponent<PlayerAttacks>();
        }
    }
    void checkorb()
    {
        if (GameObject.FindGameObjectWithTag("Orb") != null)
        {
            findOrb = true;
            Orb = GameObject.FindGameObjectWithTag("Orb").transform;
        }
        else
        {
            findOrb = false;
            Orb = null;
        }
    }
    //KEJAR PLAYER / ORB
    public void chaseobject(Transform theobject, bool isOrb)
    {   
        boss.LookAtOrb(theobject);
        Vector2 target = new Vector2(theobject.position.x, rb.position.y);
        if (isOrb == true)
        {
            orbHealth = theobject.GetComponent<OrbHealth>();
            if (orbHealth.TAGS == 1)
            {
                anim.SetInteger("State",1);
                rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
            }
            else if (orbHealth.TAGS == 2)
            {
                    if (Vector2.Distance(rb.position, theobject.position) < 5.0)
                    {
                        rb.position = this.rb.position;
                        anim.SetInteger("State",0);
                        anim.SetTrigger("idling");
                        anim.SetBool("Idle", true);
                    }
                    else
                    {
                        anim.SetInteger("State",1);
                        rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
                    }
            }
        }
        else 
        {
            if (Vector2.Distance(rb.position,theobject.position) <= 1f)
            {
                canattack = true;
                if (!blocking)
                {
                    anim.SetInteger("State",0);
                }
                rb.position = this.rb.position;
                anim.SetBool("doIdle",true);
            }
            else
            {
                canattack = false;
                anim.SetInteger("State",1);
                rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
            }
        }
    }
    //MENJAUH DARI PLAYER
    void moveaway(Transform theobject)
    {
        range = Vector2.Distance(rb.position, theobject.position);
        if (range > minDistance)
        {
            boss.LookAtOrb(theobject);
            rb.position = this.rb.position;
            anim.SetInteger("State",0);
            StartRegenSP = true;
            StopRegen = false;
        }
        else
        {
            boss.DontLook(theobject);
            StopRegen = true;
            anim.SetInteger("State",1);
            rb.position = Vector2.MoveTowards(rb.position, theobject.position, -1 * Speed * Time.deltaTime);
        }
    }

    void Die()
    {
        isDead = true;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void jump()
    {
        if (IsGrounded && Orb != null)
        {
            float distanceFromPlayer = Orb.position.x - rb.position.x;
            rb.AddForce(new Vector2(distanceFromPlayer, jumpForce), ForceMode2D.Impulse);
            //rb.velocity = Vector2.up * jumpForce;
            
        }
        else anim.SetBool("Idle", false);

    }

    public void checkplayer()
    {
        playerisDead = GetComponent<PlayerAttacks>().isDead;
    }

    public void Attack()
    {
        currentStamina -= 1;
        sp.SetStamina(currentStamina);
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemys)
        {
            if (enemy.tag == "Player")
            {
                enemy.GetComponent<PlayerAttacks>().TakeDamage(10, true);
            }
            if (enemy.tag == "PlayerW")
            {
                enemy.GetComponent<WarriorAttack>().TakeDamage(10, true);
            }
        }
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //GETSTATUS FOR MCTS
    public int GetHealthStats()
    {
        return currentHealth;
    }
    public int GetStaminaStats()
    {
        return (int)currentStamina;
    }
}
