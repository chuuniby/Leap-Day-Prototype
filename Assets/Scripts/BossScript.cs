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
    public GameObject _prefabEnemy;

    public BoxCollider2D[] leftMidRight;
    public List<GameObject> spawnPoints;

    public float timer;
    public float waitingTimeForWarningSign;

    public int phase2Count = 3;

    public Color tmpColor;
    public Color tmpWarningColor;

    public bool startAttack;
    public bool startPattern;
    public bool dead = false;
    public bool firstTimeFight = true;
    public bool finishPhase;
    public bool checkPhase1;
    public bool spawnDetectionScriptOn;
    public bool laserDoDamage;
    public ParticleSystem.MainModule mainModule;
    public enum BossPhase
    {
        Null,
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
        currentPhase = BossPhase.Null;
        tmpColor = new Color(255f,0f,0f,0f);
    }

    void Update()
    {
        switch (currentPhase)
        {
            case BossPhase.Intro:
                transform.GetComponent<SpriteRenderer>().enabled = true;
                transform.GetComponent<Animator>().enabled = true;  //Start the landing animation for the boss
                StartCoroutine(nameof(StartAttack));
                break;

            case BossPhase.Phase1:

                //Debug.Log("Hit update");

                //StartCoroutine(nameof(WherePlayer)); //Check player pos
                //    spawnPoints.Clear(); //Clear
                //    StartCoroutine(WaitxSeconds(2f)); //Give player x amount of time to kill enemy before spawning 
                //                                        //Maybe Im supposed to summon the boss down so that the player can fight?


                //if (finishPhase) currentPhase = BossPhase.Phase2;

                if(_prefabEnemy == null)
                {
                    spawnDetectionScriptOn = true;
                }

                if (_prefabEnemy == null && checkPhase1 == true)        //go to phase 2
                {
                    finishPhase = true;
                }

                if (finishPhase) currentPhase = BossPhase.Phase2;
                break;

            case BossPhase.Phase2:
                finishPhase = false;
                warningSign.SetActive(true);
                spikeGroup.SetActive(true);

                if (startPattern)
                {
                    StartCoroutine(SpikeFade());
                    startPattern = false;
                }
                if(phase2Count == 0)
                {
                    finishPhase = true;
                }
                if (finishPhase) currentPhase = BossPhase.Phase3;
                break;

            case BossPhase.Phase3:
                warningSign.SetActive(false);
                spikeGroup.SetActive(false);
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
                spike.GetComponent<PolygonCollider2D>().enabled = true; //CHANGE THIS TO TRUE FOR REAL GAME SO THE SPIKE ACTUALLY DO DAMAGE
            }
            phase2Count--;
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
        yield return new WaitForSeconds(2f);        //give player break before start phase 3
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

                if (Vector2.Distance(laser.transform.GetComponentInChildren<LaserInfo>().top.transform.position, laser.transform.GetComponentInChildren<LaserInfo>().mid.transform.position) < 0.01)
                {
                    laserDoDamage = true;
                }

                if (laserDoDamage)
                {
                    var collision = laser.transform.GetComponentInChildren<LaserInfo>().mid.GetComponent<ParticleSystem>().collision;
                    collision.enabled = true;
                    laser.transform.GetComponentInChildren<LaserInfo>().top.SetActive(false);
                    laser.transform.GetComponentInChildren<LaserInfo>().bot.SetActive(false);
                    mainModule = laser.transform.GetComponentInChildren<LaserInfo>().mid.GetComponent<ParticleSystem>().main;
                    mainModule.startSize = 1f;
                    //mainModule.startSpeed = 17f; //supposed to make the laser go faster after change size but it didnt probably due to stopping and playing the particle system
                    laser.transform.GetComponentInChildren<LaserInfo>().mid.GetComponent<ParticleSystem>().Stop();
                    laser.transform.GetComponentInChildren<LaserInfo>().mid.GetComponent<ParticleSystem>().Play();
                }
            }
        }
        yield return null;
        StartCoroutine(LaserAttack());
    }

    IEnumerator LaserAttack()
    {

        yield return null;
    }

    IEnumerator StartAttack()       //Intro
    {
        animator.SetTrigger("Idle");
        firstTimeFight = true;
        //play intro like put name tag there with cool image or smth
        yield return new WaitForSeconds(2f);
        startAttack = true;
        if (startAttack)    //Start phase 1
        {
            spawnDetectionScriptOn = true;
            currentPhase = BossPhase.Phase1;
        }
    }

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject spawnPoint in spawnPoints)
        {
            _prefabEnemy = Instantiate(normalEnemy, spawnPoint.transform.position, Quaternion.identity); //spawn enemy on player previous pos
        }
        checkPhase1 = true;     //win condition
    }
    IEnumerator WaitxSeconds(float x)
    {
        yield return new WaitForSeconds(x);
    }

    //private void OnParticleTrigger()
    //{
    //    int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    //}
}
