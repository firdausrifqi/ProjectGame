using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    float Speed = 2.5f;
    Boss boss;
    [SerializeField] float attackRange = 0.5f;
    BossAction bossAction;
    float nextAttackTime = 0f;
    public float attackRate = 3f;

    bool chaseplayer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (GameObject.FindGameObjectWithTag("PlayerW") != null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerW").transform;
        }

        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        bossAction = animator.GetComponent<BossAction>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (bossAction.currentHealth > 90)
        {
            chaseplayer = true;
        }
        else
        {
            if (bossAction.findOrb)
            {
                if (bossAction.Orb == null)
                {
                    chaseplayer = true;
                    animator.SetBool("Idle", false);
                }
                else
                {
                    chaseplayer = false;
                    boss.LookAtOrb(bossAction.Orb);
                    Vector2 target = new Vector2(bossAction.Orb.position.x, rb.position.y);
                    // Vector2 newpos = Vector2.MoveTowards(rb.position, target, Speed * Time.fixedDeltaTime);
                    // rb.MovePosition(newpos);
                    rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);
                    if (Vector2.Distance(rb.position, bossAction.Orb.position) < 5.0)
                    {
                         rb.position = this.rb.position;
                         animator.SetTrigger("idling");
                         animator.SetBool("Idle", true);
                    }
                }

            }
            else chaseplayer = true;
        }


        if (chaseplayer && bossAction.doChase)
        {
            if (bossAction.playerisDead)
            {
                rb.position = this.rb.position;
            }
            else
            {
                boss.LookAtPlayer(player);
                Vector2 target = new Vector2(player.position.x, rb.position.y);
                // Vector2 newpos = Vector2.MoveTowards(rb.position, target, Speed * Time.fixedDeltaTime);
                // rb.MovePosition(newpos);
                rb.position = Vector2.MoveTowards(rb.position, target, Speed * Time.deltaTime);

            }


        }
        // else if (chaseplayer)
        // {
        //     if (Time.time >= nextAttackTime)
        //         {

        //             if (Vector2.Distance(player.position, rb.position) <= attackRange && bossAction.GetHurt == false)
        //             {
        //                 animator.SetTrigger("Attack1");
        //                 nextAttackTime = Time.time + 2f / attackRate;
        //                 //Debug.Log(nextAttackTime);
        //             }

        //         }
        // }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }






}
