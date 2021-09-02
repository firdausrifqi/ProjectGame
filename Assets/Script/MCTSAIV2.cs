using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTSAIV2 : MonoBehaviour
{
    [Header("Logic Parameter")]
    public float[] parameter1;
    public float[] score;
    public float[] attackscore;
    public float[] dodge;
    float action;
    float actionnum = -1f;
    float actionname;

    [Header("Base")]
    EnemyBoss NPCStats;
    Rigidbody2D rb;
    Animator anim;
    Boss boss;
    float bundir = 0f;
    
    [Header("Attack")]
    [SerializeField] float nextAttackTime = 0f;
    public float attackRate = 2f;

    [Header("Block")]
    [SerializeField] float nextBlockTime = 0f;
    [SerializeField] bool blocking = false;

    [Header("NPC STATS")]
    [SerializeField] public int currentHealth;
    [SerializeField] int currentStamina;

    [Header("Target Status")]
    Transform Target;
    [SerializeField] PlayerAttacks PlayerStatus;
    int playerHealth;
    bool PlayerAttacking;

    [Header("ORB")]
    //bool findOrb;
    [SerializeField] bool isOrb;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        NPCStats = GetComponent<EnemyBoss>();
        checktarget();
        parameter1 = new float[6];
        score = new float[3];
        attackscore = new float[2];
        dodge = new float[2];
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
        currentStamina = NPCStats.GetStaminaStats();

        //LOGIC SCORE
        //1 CEK STATUS DARAH PLAYER DAN NPC
        if(currentHealth >= playerHealth)
        {
            parameter1[0] = 1f;
        }
        else if (currentHealth >= 50)
        {
            parameter1[0] = 1f;
        }
        else parameter1[0] = 0f;
        //Debug.Log("Darah yang dimiliki oleh NPC >= Player?: " + parameter1[0]);
        //2 CEK PLAYER APAKAH ATTACK ATAU TIDAK
        if(PlayerAttacking && currentHealth < 50)
        {
            parameter1[1] = 1f;
        }
        else if ( PlayerAttacking )
        {
            parameter1[1] = 0;
        }
        else parameter1[1] = 0f;
        //Debug.Log("Apakah Player melakukan serangan?: " + parameter1[1]);
        //3 CEK JARAK ANTARA PLAYER DAN NPC
        if(Vector2.Distance(rb.position,Target.position) >= 1f)
        {
            parameter1[2] = 1f;
        }
        else parameter1[2] = 0f;
        //Debug.Log("apakah jarak antara NPC dengan Player itu jauh?: " + parameter1[2]);
        //4 CEK ORB ADA ATAU TIDAK
        if(isOrb)
        {
            parameter1[3] = 1f;
        }
        else parameter1[3] = 0f;
        //Debug.Log("Apakah ada ORB?: " + parameter1[3]);
        //cek darah
        if(currentHealth <= 50 && isOrb)
        {
            parameter1[4] = 1;
        }
        else if (currentStamina*10 < currentHealth && currentStamina == 0)
        {
            parameter1[4] = 1;
        }
        else parameter1 [4] = 0;
        //cek stamina
        if(currentStamina <= 3)
        {
            parameter1[5] = 1;
        }
        else parameter1[5] = 0;
        //Debug.Log("Parameter 4= " + parameter1[3]);
        // kondisi untuk menyerang/block player
        score[0] = (1f * parameter1[0]) + (0.5f * parameter1[1]) + (0.5f * parameter1[2]) + (0.5f * parameter1[3]) + (0.5f * parameter1[4]) + (0.5f * parameter1[5]); 
        //Debug.Log("Nilai NPC untuk menyerang: " + score[0]);
        // kondisi untuk menjauh dari player
        score[1] = (0.5f * parameter1[0]) + (1f * parameter1[1]) + (0.5f * parameter1[2]) + (1f * parameter1[3]) + (1f * parameter1[4]) + (1f * parameter1[5]); 
        //Debug.Log("Nilai NPC untuk menjauh: " + score[1]);
        //Kondisi untuk mendekati player
        score[2] = (1f * parameter1[0]) + (0.5f * parameter1[1]) + (1f * parameter1[2]) + (0.5f * parameter1[3]) + (0.5f * parameter1[4]) + (0.5f * parameter1[5]); 
        //Debug.Log("Nilai NPC untuk mendekat: " + score[2]);
        //kondisi ngambil power
        // score[3] = (0.5f * parameter1[0]) + (0.5f * parameter1[1]) + (1f * parameter1[2]) + (1f * parameter1[3]); 
        // //Debug.Log("Score 4 = " + score[3]);
   
        for (int i = 0; i < score.Length; i++)
        {
            //Debug.Log("score "+i+" = "+score[i]);
            if (actionnum < score[i])
            {
                actionnum = score[i];
                actionname = i;
            }
        }
        //Debug.Log("Nilai Tertinggi: " + actionnum);
        if (actionname == 0f)
        {
            //Debug.Log(" NPC Menyerang Player ");
            PlayerClose();
            resetvar();
        }
        else if (actionname == 1f)
        {
            //Debug.Log(" NPC Menjauhi Player ");
            moveaway(actionname);
            resetvar();
        }
        else if (actionname == 2f)
        {
            //Debug.Log("NPC Mendekati Player ");
            NPCStats.giveAction(actionname);
            resetvar();
        }
        
    }
    //tutup update void

    void PlayerClose()
    {
        currentStamina = NPCStats.GetStaminaStats();
        //
        if(currentHealth >= playerHealth)
        {
            parameter1[0] = 1f;
        }
        else parameter1[0] = 0f;
        //
        if(currentStamina >= 2 )
        {
            parameter1[1] = 1f;
        }
        else parameter1[1] = 0f;
        //
        if(PlayerAttacking)
        {
            parameter1[2] = 1f;
        }
        else parameter1[2] = 0f;

        //untuk attack
        attackscore[0] = (1f * parameter1[0]) + (1f * parameter1[1]) + (0.25f * parameter1[2]);
        //untuk block
        attackscore[1] = (0.5f * parameter1[0]) + (1f * parameter1[1]) + (1f * parameter1[2]);

        for (int i = 0; i < attackscore.Length; i++)
        {
            if (actionnum < attackscore[i])
            {
                actionnum = attackscore[i];
                actionname = i;
            }
        }
        if (actionname == 0f && currentStamina != 0)
        {
            //Debug.Log(" Menyerang Player ");
            AttackPlayer();
            resetvar();
        }
        else if (actionname == 1f && currentStamina != 0)
        {
            //Debug.Log(" Block Player ");
            BlockPlayer();
            resetvar();
        }
    }

    //ACTION FUNCTION

    void AttackPlayer()
    {
        NPCStats.isiStamina(false);
        if (Time.time >= nextAttackTime)
        {
            anim.SetInteger("State",0);
            // //Debug.Log("attack");
            blocking = false;
            anim.SetTrigger("Attack1");
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void BlockPlayer()
    {
        if (Time.time >= nextBlockTime)
        {
            blocking = true;
            anim.SetInteger("State",2);
            nextBlockTime = Time.time + 2f;
        }
    }

    void moveaway(float actionnum)
    {
        //Debug.Log("Move");
        NPCStats.giveAction(actionnum);
        //logic
        if(currentStamina <= 3)
        {
            parameter1[0] = 1;
        }
        else parameter1[0] = 0;

        if(!PlayerAttacking)
        {
            parameter1[1] = 1;
        }
        else parameter1[1] = 0;

        if(currentHealth < currentStamina)
        {
            parameter1[2] = 1;
        }
        else parameter1[2] = 0;

        if(isOrb)
        {
            parameter1[3] = 1;
        }
        else parameter1[3] = 0;
        if (currentStamina <= 0)
        {
            parameter1[4] = 1;
        }
        else parameter1[4] = 0;
        //tutup logic, cari keputusan
        //Isi Stamina
        dodge[0] = (1f*parameter1[0])+(1f*parameter1[1])+(0.5f*parameter1[2])+(0.5f*parameter1[3])+parameter1[4];
        //Isi Darah
        dodge[1] = (0.5f*parameter1[0])+(1f*parameter1[1])+(1f*parameter1[2])+(1f*parameter1[3])+parameter1[4];
        //Debug.Log("Dodge 1 = " + dodge[0]);
        //Debug.Log("Dodge 2 = " + dodge[1]);

            //Debug.Log("PENGECEKAN");
            bundir = 0f;
            for (int i = 0; i < dodge.Length; i++)
            {
                //Debug.Log("score "+i+" = "+dodge[i]);
                if (actionnum < dodge[i])
                {
                    actionnum = dodge[i];
                    actionname = i;
                }
            }
            if (actionname == 0f)
            {
                //Debug.Log(" Isi Stamina ");
                NPCStats.isiStamina(true);
                resetvar();
            }
            else if (actionname == 1f)
            {
                //Debug.Log(" Isi Darah ");
                NPCStats.giveAction(3f);
                resetvar();
            }
        
    }

    void resetvar()
    {
        actionnum = -1f;
        actionname = 0;
    }

    void checktarget()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerStatus = Target.GetComponent<PlayerAttacks>();
        }
        if (GameObject.FindGameObjectWithTag("PlayerW") != null)
        {
            Target = GameObject.FindGameObjectWithTag("PlayerW").transform;
            //PlayerStatus = Target.GetComponent<PlayerAttacks>();
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

    //Kirim variable
    public bool GetStats()
    {
        return blocking;
    }
    
    //Nerima Variable
    public void SetStats(bool block)
    {
        blocking = block;
    }
    
}
