using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHealth : MonoBehaviour
{
    public int TAGS = 0;
    OrbCountDown[] OCD;
    OrbCountDown OCDs;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject[] OrbCD = GameObject.FindGameObjectsWithTag("CountDown");
        OCD = new OrbCountDown[OrbCD.Length];

        for ( int i = 0; i < OrbCD.Length; ++i )
        {
            OCD[i] = OrbCD[i].GetComponent<OrbCountDown>();
            OCDs = OCD[i];
        }
          
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerAttacks>().GiveHealth(40);
            OCDs.TimeStart();
            Destroy(gameObject);
        }
        if (other.tag == "Enemy")
        {
            //Debug.Log("MENYENTUH ORB");
            other.GetComponent<EnemyBossFSM>().GiveHealth(40);
            OCDs.TimeStart();
            Destroy(gameObject);
        }
        if (other.tag == "EnemyMCTS")
        {
            //Debug.Log("MENYENTUH ORB");
            other.GetComponent<EnemyBoss>().GiveHealth(40);
            OCDs.TimeStart();
            Destroy(gameObject);
        }
        if (other.tag == "PlayerW")
        {
            other.GetComponent<WarriorAttack>().GiveHealth(40);
            OCDs.TimeStart();
            Destroy(gameObject);
        }
        if (other.tag == "Ground")
        {
            TAGS = 1;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (other.tag == "Ground2")
        {
            TAGS = 2;
           rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
