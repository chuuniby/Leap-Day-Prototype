using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class BossScript : MonoBehaviour
{
    public static BossScript instance;
    public BossPhase currentPhase;

    public Animator animator;

    public GameObject spikeGroup;
    public GameObject[] spikes;
    public GameObject warningSign;
    public GameObject[] lasers;
    public GameObject normalEnemy;

    public BoxCollider2D[] leftMidRight;
    public List<Transform> spawnPoints;

    public float timer;
    public float waitingTimeForWarningSign;

    public Color tmpColor;
    public Color tmpWarningColor;

    public bool startAttack;
    public bool startPattern;
    public bool dead = false;
    public bool firstTimeFight = true;
    public bool finishPhase;



    public enum BossPhase
    {
        Intro,
        Phase1,
        Phase2,
        Phase3,
        Dead
    };
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();

    }
    void Start()
    {
        currentPhase = BossPhase.Phase1;
        tmpColor = new Color(255f,0f,0f,0f);
    }

    void Update()
    {
        switch (currentPhase)
        {
            case BossPhase.Intro:
                transform.GetComponent<SpriteRenderer>().enabled = true;
                transform.GetComponent<Animator>().enabled = true;
                StartCoroutine(nameof(StartAttack));
                if (startAttack)
                {
                    currentPhase = BossPhase.Phase1;
                }
                break;

            case BossPhase.Phase1:
                    StartCoroutine(nameof(WherePlayer)); //Check player pos
                    spawnPoints.Clear(); //Clear
                    StartCoroutine(WaitxSeconds(2f)); //Give player x amount of time to kill enemy before spawning 
                                                        //Maybe Im supposed to summon the boss down so that the player can fight?

                
                if (finishPhase) currentPhase = BossPhase.Phase2;
                break;

            case BossPhase.Phase2:
                finishPhase = false;
                spikeGroup.SetActive(true);

                if (startPattern)
                {
                    StartCoroutine(SpikeFade());
                    startPattern = false;
                }

                if (finishPhase) currentPhase = BossPhase.Phase3;
                break;

            case BossPhase.Phase3:
                finishPhase = false;
                StartCoroutine(LaserAppear());

                if (finishPhase) currentPhase = BossPhase.Dead;
                break;
            case BossPhase.Dead:
                dead = true;
                transform.GetComponent<SpriteRenderer>().enabled = false;
                //Play dead animation
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
        lasers[0].SetActive(true);
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

    IEnumerator StartAttack()
    {
        animator.SetTrigger("Idle");
        firstTimeFight = true;
        //play intro or smth
        yield return new WaitForSeconds(2f);
        startAttack = true;
    }

    IEnumerator WherePlayer()
    {
        foreach (BoxCollider2D col in leftMidRight)
        {
            col.enabled = true; 
            yield return null;
            col.enabled = false;
            
        }
        StartCoroutine(WaitxSeconds(0.7f)); // Spawn on player pos on delay so that player is away from it
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(normalEnemy, spawnPoint.position, Quaternion.identity); //spawn enemy on player previous pos
        }
    }
    IEnumerator WaitxSeconds(float x)
    {
        yield return new WaitForSeconds(x);
    }
}
