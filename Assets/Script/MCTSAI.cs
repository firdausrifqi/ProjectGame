using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTSAI : MonoBehaviour
{
    [Header("Logic Parameter")]
    public float[] parameter1;
    public float[] score;
    float action;
    float actionnum = -1f;
    float actionname;

    [Header("Base")]
    EnemyBoss NPCStats;
    Rigidbody2D rb;
    Animator anim;
    Boss boss;
    
    [Header("Attack")]
    [SerializeField] float nextAttackTime = 0f;
    public float attackRate = 2f;

    [Header("HealthBar")]
    [SerializeField] public int currentHealth;

    [Header("Target Status")]
    Transform Target;
    [SerializeField] PlayerAttacks PlayerStatus;
    int playerHealth;
    bool PlayerAttacking;

    [Header("ORB")]
    //bool findOrb;
    bool isOrb;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        NPCStats = GetComponent<EnemyBoss>();
        checktarget();
        PlayerStatus = Target.GetComponent<PlayerAttacks>();
        parameter1 = new float[4];
        score = new float[4];
    }

    // Update is called once per frame
    void Update()
    {
        //CHECK SEMUA SEBELUM MASUK KE LOGIC
        checktarget();
        checkorb();
        //GET STATUS NPC
        playerHealth = PlayerStatus.GetHealthStats();
        PlayerAttacking = PlayerStatus.IsAttack();
        currentHealth = NPCStats.GetHealthStats();

        //LOGIC SCORE
        //1 CEK STATUS DARAH PLAYER DAN NPC
        if(currentHealth > playerHealth)
        {
            parameter1[0] = 1f;
        }
        else parameter1[0] = 0f;
        //Debug.Log("Parameter 1= " + parameter1[0]);
        //2 CEK PLAYER APAKAH ATTACK ATAU TIDAK
        if(PlayerAttacking)
        {
            parameter1[1] = 1f;
        }
        else parameter1[1] = 0f;
        //Debug.Log("Parameter 2= " + parameter1[1]);
        //3 CEK JARAK ANTARA PLAYER DAN NPC
        if(Vector2.Distance(rb.position,Target.position) >= 1f)
        {
            parameter1[2] = 1f;
        }
        else parameter1[2] = 0f;
        //Debug.Log("Parameter 3= " + parameter1[2]);
        //4 CEK ORB ADA ATAU TIDAK
        if(isOrb)
        {
            parameter1[3] = 1f;
        }
        else parameter1[3] = 0f;
        //Debug.Log("Parameter 4= " + parameter1[3]);
        // kondisi untuk menyerang player
        score[0] = (1f * parameter1[0]) + (0.5f * parameter1[1]) + (0.5f * parameter1[2]) + (0.5f * parameter1[3]); 
        //Debug.Log("Score 1 = " + score[0]);
        // kondisi untuk menjauh dari player
        score[1] = (0.5f * parameter1[0]) + (1f * parameter1[1]) + (0.5f * parameter1[2]) + (1f * parameter1[3]); 
        //Debug.Log("Score 2 = " + score[1]);
        //Kondisi untuk mendekati player
        score[2] = (1f * parameter1[0]) + (0.5f * parameter1[1]) + (1f * parameter1[2]) + (0.5f * parameter1[3]); 
        //Debug.Log("Score 3 = " + score[2]);
        //kondisi ngambil power
        score[3] = (0.5f * parameter1[0]) + (0.5f * parameter1[1]) + (1f * parameter1[2]) + (1f * parameter1[3]); 
        //Debug.Log("Score 4 = " + score[3]);

        // action = Mathf.Max(score);

        // //Debug.Log(action);

        // if (action == 0f)
        // {
        //     //Debug.Log("Attack");
        //     AttackPlayer();
        // }
        // else if(action == 1f || action == 2f || action == 3f)
        // {
        //     //Debug.Log("ACTION 1 2 3");
        //     NPCStats.giveAction(action);
        // }
        // else
        // {
        //     //Debug.Log("MASUK");
        // }

        
        for (int i = 0; i < score.Length; i++)
        {
            if (actionnum < score[i])
            {
                actionnum = score[i];
                actionname = i;
            }
        }
        if (actionname == 0f)
        {
            //Debug.Log(" Menyerang Player ");
            AttackPlayer();
            actionnum = -1f;
            actionname = 0;
        }
        else if (actionname == 1f || actionname == 2f || actionname == 3f)
        {
            NPCStats.giveAction(actionname);
            actionnum = -1f;
            actionname = 0;
        }
        
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

    void checkorb()
    {
        if (GameObject.FindGameObjectWithTag("Orb") != null)
        {
            isOrb = true;
        }
        else
        {
            isOrb = false;
        }
    }
    //ACTION FUNCTION

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            anim.SetInteger("State",0);
            // //Debug.Log("attack");
            // blocking = false;
            anim.SetTrigger("Attack1");
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
}
