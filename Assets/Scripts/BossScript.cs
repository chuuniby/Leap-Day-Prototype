using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public BossPhase currentPhase;

    public GameObject spikeGroup;
    public GameObject[] spikes;
    public GameObject warningSign;
    public GameObject[] lasers;

    public float timer;
    public float waitingTimeForWarningSign;

    public Color tmpColor;
    public Color tmpWarningColor;

    public bool startPattern;
    public enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    };
    void Start()
    {
        currentPhase = BossPhase.Phase1;
        tmpColor = new Color(255f,0f,0f,0f);
    }

    void Update()
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:

                break;

            case BossPhase.Phase2:
                spikeGroup.SetActive(true);

                if (startPattern)
                {
                    StartCoroutine(SpikeFade());
                    startPattern = false;
                }


                break;

                case BossPhase.Phase3:
                StartCoroutine(LaserAppear());

                break;
        }
    }

    IEnumerator SpikeFade() //Stop Attack Phase 2
    {
        if (spikeGroup.activeInHierarchy)
        {
            foreach (GameObject spike in spikes)
            {
                warningSign.GetComponent<SpriteRenderer>().color = new Color(255f, 235f, 0f, 255f);
                tmpColor.a = 0.3f;
                spike.GetComponent<SpriteRenderer>().color = tmpColor;
                spike.GetComponent<PolygonCollider2D>().enabled = false;
            }
            StartCoroutine(Warning());
            yield return new WaitForSeconds(3f);
            StartCoroutine (SpikeAppear());
        }
    }

    IEnumerator SpikeAppear() //Start Attack Phase 2
    {
        if (spikeGroup.activeInHierarchy)
        {
            foreach (GameObject spike in spikes)
            {

                tmpColor.a = 1f;
                spike.GetComponent<SpriteRenderer>().color = tmpColor;
                spike.GetComponent<PolygonCollider2D>().enabled = false; //CHANGE THIS TO TRUE FOR REAL GAME SO THE SPIKE ACTUALLY DO DAMAGE

            }
            yield return new WaitForSeconds(3f);
            StartCoroutine (SpikeFade());
        }

    }

    IEnumerator Warning() //Warning Phase 2
    {
        yield return new WaitForSeconds(waitingTimeForWarningSign);
        warningSign.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 255f);
    }

    IEnumerator LaserAppear() //Start Attack Phase 3
    {
        int x;
        x = 0;
        lasers[x].gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(LaserConverge());
    }
    IEnumerator LaserConverge()
    {
        foreach(GameObject laser in lasers)
        {
            if (laser.activeInHierarchy)
            {
                Vector3 topDir = laser.transform.GetComponentInChildren<LaserInfo>().mid.transform.position - laser.transform.GetComponentInChildren<LaserInfo>().top.transform.position;
                laser.transform.GetComponentInChildren<LaserInfo>().top.transform.position += topDir * Time.deltaTime;
                Vector3 botDir = laser.transform.GetComponentInChildren<LaserInfo>().mid.transform.position - laser.transform.GetComponentInChildren<LaserInfo>().bot.transform.position;
                laser.transform.GetComponentInChildren<LaserInfo>().bot.transform.position += botDir * Time.deltaTime;
            }
        }
        yield return null;
        StartCoroutine(LaserAttack());
    }

    IEnumerator LaserAttack()
    {
        //Laser Collide with Player
        yield return null;
    }
}
