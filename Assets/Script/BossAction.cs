using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAction : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anims;
    [Header("Attack")]
    public LayerMask enemyLayers;
    public float attackRange = 3f;
    public bool GetHurt = false;
    public Transform attackPoint;
    [Header("Healthbar")]
    public HealthBar healthBar;
    [SerializeField] int maxHealth = 100;
    [SerializeField] public int currentHealth;
    private IEnumerator coroutine;
    public GameObject deathEffect;
    [Header("Jump")]
    public LayerMask whatIsGround;
    public Transform groundPosition;
    public float checkRadius;
    public float jumpForce;
    bool jumping = false;
    bool doJump = false;
    public Transform Orb;
    public bool findOrb;
    Rigidbody2D rb;
    bool IsGrounded;
    public bool playerisDead;
    public bool isDead = false;
    Transform player;
    public bool doChase = false;
    float nextAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        anims = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking Player ada atau tidak.
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (GameObject.FindGameObjectWithTag("PlayerW") != null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerW").transform;
        }
        //Checking Orb ada atau tidak.
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
        //check player untuk attack
        if (Time.time >= nextAttackTime)
        {
            if (Vector2.Distance(player.position, rb.position) <= 1f && GetHurt == false)
             {
                 anims.SetTrigger("Attack1");
                 nextAttackTime = Time.time + 1f / 2f;
             }

         }
        //check posisi antara player dan enemy
        if (Vector2.Distance(rb.position,player.position) <= 1f)
        {
            anims.SetBool("doIdle",true);
            doChase = false;
        }
        else{
            anims.SetBool("doIdle",false);
            doChase = true;
        }

        IsGrounded = Physics2D.OverlapCircle(groundPosition.position, checkRadius, whatIsGround);
        playanims();


    }
    void playanims()
    {
        anims.SetBool("Grounded", IsGrounded);
    }
    public void GiveHealth(int count)
    {
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }
        else currentHealth = currentHealth + count;
        anims.SetBool("Idle", false);
        healthBar.SetHealth(currentHealth);
    }


    public void TakeDamage(int Damage, bool anim)
    {
        GetHurt = true;
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth != 0)
        {
            currentHealth -= Damage;
            anims.SetTrigger("Hurt");
            healthBar.SetHealth(currentHealth);
            //healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        GetHurt = false;
    }
    public void Attack()
    {

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

    void Die()
    {
        isDead = true;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundPosition.position, checkRadius);

    }
    public void jump()
    {
        if (IsGrounded && Orb != null)
        {
            float distanceFromPlayer = Orb.position.x - rb.position.x;
            rb.AddForce(new Vector2(distanceFromPlayer, jumpForce), ForceMode2D.Impulse);
            //rb.velocity = Vector2.up * jumpForce;
            
        }
        else anims.SetBool("Idle", false);

    }

    public void checkplayer()
    {
        playerisDead = GetComponent<PlayerAttacks>().isDead;
    }

}