using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarriorAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public int Damage = 10;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public HealthBar healthBar;
    private int currentHealth;
    private int maxHealth = 100;
    private IEnumerator coroutine;
    private bool canAttack = true;
    bool Check;
    public bool isDead = false;
    bool attacking = false;
    [SerializeField] private float currentStamina;
    private int maxStamina = 10;
    public SPbar sp;
    bool recharging = false;
    float regeneration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Check = GetComponent<WarriorMovement>().jumping;
        recharging = GetComponent<WarriorMovement>().canRecharge;
        // //Debug.Log("Next Attack: " + nextAttackTime + ", canAttack: " + canAttack);
        if (Time.time >= nextAttackTime)
        {
            if (canAttack)
            {
                if ((currentStamina != 0 || currentStamina < 0))
                {
                    attacking = true;
                    animator.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
        if (recharging)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += regeneration * Time.fixedDeltaTime;
                if (currentStamina > maxStamina)
                {
                    currentStamina = maxStamina;
                    recharging = false;
                }
                sp.SetStamina((int)currentStamina);
            }
        }
        else sp.SetStamina((int)currentStamina);
    }

    public void Attack()
    {

        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemys)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponent<EnemyBossFSM>().TakeDamage(Damage, true);
            }
            if (enemy.tag == "EnemyMCTS")
            {
                enemy.GetComponent<EnemyBoss>().TakeDamage(Damage, true);
            }
            // if (enemy.name == "NPC")
            // {
            //     enemy.GetComponent<Enemy>().TakeDamage(Damage);
            // }
        }
        canAttack = true;
    }
    //GET HIT
    public void TakeDamage(int Damage, bool anim)
    {
        //Debug.Log(currentHealth);
        canAttack = false;
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth != 0)
        {
            currentHealth -= Damage;
            animator.SetTrigger("Hurt"); healthBar.SetHealth(currentHealth);
            //healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                coroutine = Die(0.3f);
                StartCoroutine(coroutine);
            }
            canAttack = true;
        }
    }

    void Die()
    {
        //Debug.Log("Enemys Dead");
        //animator.SetBool("IsDead", true);
        animator.SetTrigger("Death");
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        isDead = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("WhatStage"));
    }
    private IEnumerator Die(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("Enemys Dead");
        // animator.SetBool("IsDead", true);
        animator.SetTrigger("Death");
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        isDead = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("WhatStage"));
    }

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
        healthBar.SetHealth(currentHealth);
        sp.SetStamina(currentStamina);
    }

    //GIZMOS
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}