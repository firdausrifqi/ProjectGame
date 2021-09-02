using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHealth : MonoBehaviour
{
    public int TAGS = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerAttacks>().GiveHealth(40);
            Destroy(gameObject);
        }
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyBoss>().GiveHealth(40);
            Destroy(gameObject);
        }
        if (other.tag == "PlayerW")
        {
            other.GetComponent<WarriorAttack>().GiveHealth(40);
            Destroy(gameObject);
        }
        if (other.tag == "Ground")
        {
            TAGS = 1;
        }
        if (other.tag == "Ground2")
        {
            TAGS = 2;
        }
    }
}
