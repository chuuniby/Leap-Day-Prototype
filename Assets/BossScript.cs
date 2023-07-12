using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public BossPhase currentPhase;

    public GameObject spikeGroup;
    public GameObject[] spikes;
    public GameObject warningSign;

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
                StopAllCoroutines();


                break;
        }
    }

    IEnumerator SpikeFade()
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

    IEnumerator SpikeAppear()
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

    IEnumerator Warning()
    {
        yield return new WaitForSeconds(waitingTimeForWarningSign);
        warningSign.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 255f);
    }

}
