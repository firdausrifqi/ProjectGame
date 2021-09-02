using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossFSM : MonoBehaviour
{
    //bug setelah ambil orb (jika di ground2)

    [Header("Base")]
    [SerializeField] bool IsGrounded;
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
    public OrbHealth1 orbHealth1;
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
    float SpeedEsc = 4.5f;
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
    float regen = 1f;
    public SPbar sp;

    [Header("Attack")]
    bool canattack = false;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    float nextActionTime = 0f;
    public LayerMask enemyLayers;
    public float attackRange = 3f;
    // public bool GetHurt = false;
    public Transform attackPoint;

    [Header("Block")]
    bool isPlayerAttack;
    bool blocking;
    int PlayerHealth;

    [Header("Evade")]
    private float range;
    private float minDistance=3f;
    bool majumundur = false;
    bool recharging = false;
    bool dontchase = false;


    // Start is called before the first frame update
    void Start()
    {
        checktarget();
        random = Random.Range(0f, 9f);
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        boss = GetComponent<Boss>();
        PlayerAttacking = Target.GetComponent<PlayerAttacks>();
        anim = GetComponent<Animator>();
        chaseplayer = false;
        currentStamina = maxStamina;
        healthBar.SetMaxHealth(currentHealth);
        sp.SetMaxStamina(currentStamina);
    }

    // Update is called once per frame
    void Update()
    {
        checkorb();
        isPlayerAttack = PlayerAttacking.IsAttack();
        PlayerHealth = PlayerAttacking.GetHealthStats();
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

        if (currentStamina <= 0)  // Kondisi Isi Stamina
        {
            //Debug.Log("IsiStamina");
            dontchase = true;
            IsiStamina(Target);
        }

        if (currentHealth <= 100 && currentHealth > 30 && !GetOrb && !dontchase) //darah di atas 31 kejar player
        {
            //Debug.Log("Kejar Player");
            checktarget(); //check player
            if (Target != null)
            {
               // //Debug.Log(chaseplayer);
                chaseplayer = true;
                chaseobject(Target, false);
            }
        }
       /* else if (currentHealth <= 50 && currentHealth < PlayerHealth && !dontchase)  // Kondisi Menghindar
        {
            Evade(Target);
        } */
        else if (PlayerHealth <= currentHealth && currentHealth <= 50 && !dontchase)
        {
            checktarget(); //check player
            if (Target != null)
            {
              //  //Debug.Log(chaseplayer);
                chaseplayer = true;
                chaseobject(Target, false);
            }
        }
        else if (currentHealth <= 30 && currentHealth <= PlayerHealth && !dontchase) //darah di bawah 30 ambil orb dan lebih kecil dari player
        {
            checkorb(); //check orb ada atau tidak
            if (findOrb == false)
            {
                checktarget();
                if (Target != null)
                {
                    chaseplayer = true;
                    chaseobject(Target, false);
                }
            }
            else
            {
                chaseplayer = false;
                chaseobject(Orb, true);
            }
             
        }

        if (recharging)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += regen * Time.fixedDeltaTime;
                if (currentStamina > maxStamina)
                {
                    currentStamina = maxStamina;
                    recharging = false;
                    dontchase = false;
                }
                sp.SetStamina((int)currentStamina);
            }
        }
        else sp.SetStamina((int)currentStamina);


        //Attack or block Logic
        if (Vector2.Distance(rb.position, Target.position) <= 1f && chaseplayer && Time.time >= nextAttackTime)
        {
            
            random = Random.Range(0f, 9f);
            if (random <= 1f)
            {
                if (currentStamina > 2)
                {
                    majumundur = false;
                    //Debug.Log("block");
                    blocking = true;
                    anim.SetInteger("State", 2);
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
            else
            {
                if (currentStamina > 0)
                {
                    majumundur = false;
                    anim.SetInteger("State", 0);
                    // //Debug.Log("attack");
                    blocking = false;
                    anim.SetTrigger("Attack1");
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
        // if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        // {
        //     //Debug.Log("Player Attack");  
        // }

    }
    //end of void update

    //Basic Logics
    public void GiveHealth(int count)
    {
        currentHealth = currentHealth + count;
        currentStamina = currentStamina + 10;
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
        sp.SetStamina(currentStamina);

        GetOrb = true;
        nextActionTime = Time.time + 1f / 1f;
        rb.position = this.rb.position;
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
        }
        if (GameObject.FindGameObjectWithTag("PlayerW") != null)
        {
            Target = GameObject.FindGameObjectWithTag("PlayerW").transform;
        }
    }

    public void checkplayer()
    {
        playerisDead = GetComponent<PlayerAttacks>().isDead;
    }

    void checkorb()
    {
        if (GameObject.FindGameObjectWithTag("Orb") != null)
        {
            findOrb = true;
            Orb = GameObject.FindGameObjectWithTag("Orb").transform;
            //Debug.Log("ada orb");
        }
        else
        {
            //Debug.Log("tdk ada orb");
            findOrb = false;
            Orb = null;
        }
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

    public void Attack()
    {
        currentStamina -= 1f;
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

    // StateLogics //

    // State Mengejar Player/User
    void chaseobject(Transform theobject, bool isOrb)
    {
        boss.LookAtOrb(theobject);
        Vector2 target = new Vector2(theobject.position.x, rb.position.y);
        if (isOrb == true)
        {
            if (theobject.GetComponent<OrbHealth>() != null)
            {
                orbHealth = theobject.GetComponent<OrbHealth>();
                if (orbHealth.TAGS == 1 )
                {
                    anim.SetInteger("State", 1);
                    rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
                }
                else if (orbHealth.TAGS == 2 )
                {
                    if (Vector2.Distance(rb.position, theobject.position) < 5.0)
                    {
                        rb.position = this.rb.position;
                        anim.SetInteger("State", 0);
                        anim.SetTrigger("idling");
                        anim.SetBool("Idle", true);
                    }
                    else
                    {
                        anim.SetInteger("State", 1);
                        rb.position = Vector2.MoveTowards(rb.position, target, SpeedEsc * Time.deltaTime);
                    }
                }
            }
            else if (theobject.GetComponent<OrbHealth1>() != null)
            {
                orbHealth1 = theobject.GetComponent<OrbHealth1>();
                if (orbHealth1.TAGS == 1)
                {
                    anim.SetInteger("State", 1);
                    rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
                }
                else if (orbHealth1.TAGS == 2)
                {
                    if (Vector2.Distance(rb.position, theobject.position) < 5.0)
                    {
                        rb.position = this.rb.position;
                        anim.SetInteger("State", 0);
                        anim.SetTrigger("idling");
                        anim.SetBool("Idle", true);
                    }
                    else
                    {
                        anim.SetInteger("State", 1);
                        rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
                    }
                }
            }
        }
        else
        {
            if (Vector2.Distance(rb.position, theobject.position) <= 1f)
            {
                canattack = true;
                if (!blocking)
                {
                    anim.SetInteger("State", 0);
                }
                rb.position = this.rb.position;
                anim.SetBool("doIdle", true);
            }
            else
            {
                canattack = false;
                anim.SetInteger("State", 1);
                rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
            }
        }
    }

    //State Block Attack
    public void TakeDamage(int Damage, bool check)
    {
        recharging = false;
        dontchase = false;
        //GetHurt = true;
        if (blocking)
        {
            anim.SetTrigger("Blocking");
            blocking = false;
            random = Random.Range(0f, 9f);
            if (random <= 1f)
            {
                anim.SetInteger("State", 0);
               // //Debug.Log("Counter Attack");
                blocking = false;
                anim.SetTrigger("Attack1");
                nextAttackTime = Time.time + 1f / attackRate;
            }
            currentStamina -= 2f;
            sp.SetStamina(currentStamina);
        }
        else if (majumundur)
        {
            //Debug.Log("majumundur");
            checktarget();
            if (Target != null)
            {
                chaseplayer = true;
                chaseobject(Target, false);
            }
            range = Vector2.Distance(rb.position, Target.position);
            if (range > minDistance)
            {
                boss.LookAtOrb(Target);
                rb.position = this.rb.position;
                anim.SetInteger("State", 0);
            }
            else
            {
                boss.DontLook(Target);
                anim.SetInteger("State", 1);
                rb.position = Vector2.MoveTowards(rb.position, Target.position, -1 * Speed * Time.deltaTime);
            }
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

    //State Menghindar
    public void Evade(Transform theobject)
    {
        range = Vector2.Distance(rb.position, theobject.position);
        if (range > minDistance)
        {
            checkorb();
            boss.LookAtOrb(theobject);
            rb.position = this.rb.position;
            anim.SetInteger("State", 0);
            if (findOrb == false)
            {
                checktarget();
                if (Target != null)
                {
                    chaseplayer = true;
                    chaseobject(Target, false);
                }
            }
            else
            {
                chaseplayer = false;
                chaseobject(Orb, true);
            }
        }
        else if (Orb == null)
        {
            checktarget();
            if (Target != null)
            {
                chaseplayer = true;
                chaseobject(Target, false);
            }
            else
            {
                chaseplayer = false;
                chaseobject(Orb, true);
            }

        }
        else
        {
            checkorb();
            boss.DontLook(theobject);
            anim.SetInteger("State", 1);
            rb.position = Vector2.MoveTowards(rb.position, theobject.position, -1 * Speed * Time.deltaTime);
            if (findOrb == false)
            {
                checktarget();
                if (Target != null)
                {
                    chaseplayer = true;
                    chaseobject(Target, false);
                }
            }
            else
            {
                chaseplayer = false;
                chaseobject(Orb, true);
            }
        }
    }

    
    //State Mengisi Stamina
    public void IsiStamina(Transform theobject)
    {
        range = Vector2.Distance(rb.position, theobject.position);
        if (range >= minDistance)
        {
            boss.LookAtOrb(theobject);
            rb.position = this.rb.position;
            anim.SetInteger("State", 0);
            recharging = true;
        }
        else
        {
            recharging = false;
            boss.DontLook(theobject);
            anim.SetInteger("State", 1);
            rb.position = Vector2.MoveTowards(rb.position, theobject.position, -1 * SpeedEsc * Time.deltaTime);
        }
    }


    void Die()
    {
        isDead = true;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    //End Of States
    //Misc
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
