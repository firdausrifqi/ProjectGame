using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHealth1 : MonoBehaviour
{
    public int TAGS = 0;
    OrbCountDown1[] OCD1;
    OrbCountDown1 OCDs1;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject[] OrbCD1 = GameObject.FindGameObjectsWithTag("CountDown1");
        OCD1 = new OrbCountDown1[OrbCD1.Length];

        for (int i = 0; i < OrbCD1.Length; ++i)
        {
            OCD1[i] = OrbCD1[i].GetComponent<OrbCountDown1>();
            OCDs1 = OCD1[i];
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerAttacks>().GiveHealth(40);
            OCDs1.TimeStart();
            Destroy(gameObject);
        }
        if (other.tag == "Enemy")
        {
            //Debug.Log("MENYENTUH ORB");
            other.GetComponent<EnemyBossFSM>().GiveHealth(40);
            OCDs1.TimeStart();
            Destroy(gameObject);
        }
        if (other.tag == "PlayerW")
        {
            other.GetComponent<WarriorAttack>().GiveHealth(40);
            OCDs1.TimeStart();
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
