using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserScript : MonoBehaviour
{
    public GameObject boss;
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (boss.GetComponent<BossScript>().laserDoDamage)
            {
                other.transform.GetComponent<PlayerScript>().hp -= 1;
            }
        }
    }
}
