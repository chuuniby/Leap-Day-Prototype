using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public static BossScript instance;
    public BossPhase currentPhase;

    public Animator animator;

    public GameObject player;
    public GameObject spikeGroup;
    public GameObject[] spikes;
    public GameObject warningSign;
    public GameObject Sign;
    public GameObject[] lasers;
    public GameObject normalEnemy;
    public GameObject _prefabEnemy;
    public int enemyCount;

    public BoxCollider2D[] leftMidRight;
    public List<GameObject> spawnPoints;

    public float timer;
    public float waitingTimeForWarningSign;
    public Vector2 curPos;


    public int phase2Count = 3;

    public Color tmpColor;
    public Color tmpWarningColor;

    public bool BossDescend;
    public bool startAttack;
    public bool startPattern;
    public bool dead = false;
    public bool firstTimeFight = true;
    public bool finishPhase;
    public bool checkPhase1;
    public bool enemyIsDeadAndSpawnNewOne;
    public bool spawnDetectionScriptOn;
    public bool laserDoDamage;
    public ParticleSystem.MainModule mainModule;
    public float timerBoss;
    public float alpha;
    public bool startFade;
    public Color startColor;
    public Color endColor;
    public enum BossPhase
    {
        Null,
        Intro,
        Phase1,
        Phase2,
        //Phase3,
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
        tmpColor = new Color(255f, 0f, 0f, 0f);
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

                //if (_prefabEnemy == null)           //this always true so player go phase 2 lol
                //{
                //    spawnDetectionScriptOn = true;
                //}

                if (firstTimeFight == true)
                {
                    StartCoroutine(BossTease());
                }

                if (enemyCount >= 2)
                {
                    //animator.SetBool("BossDescend", true);
                    //if boss get hit
                    checkPhase1 = true;     //win condition
                }

                if (checkPhase1 == true)        //go to phase 2
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
                }
                if (phase2Count == 0)
                {
                    finishPhase = true;
                }
                //if (finishPhase) currentPhase = BossPhase.Phase3;     //Phase 3 no longer use
                if (finishPhase) currentPhase = BossPhase.Dead;
                break;

            //case BossPhase.Phase3:
            //    //warningSign.SetActive(false);
            //    //spikeGroup.SetActive(false);
            //    //finishPhase = false;
            //    //StartCoroutine(LaserAppear());
            //                                                            //phase 3 no longer use
            //    //if (finishPhase) currentPhase = BossPhase.Dead;
            //    //break;
            //    break;
            case BossPhase.Dead:
                warningSign.SetActive(false);
                spikeGroup.SetActive(false);
                finishPhase = false;
                dead = true;                //boss is dead what now lol
                transform.GetComponent<SpriteRenderer>().enabled = false;
                //Play dead animation
                //Go to next scene I guess
                break;
        }
    }

    IEnumerator SpikeFade() //Stop Attack Phase 2
    {
        startFade = true;
        StartCoroutine(FadeImage());
        if (spikeGroup.activeInHierarchy)
        {
            foreach (GameObject spike in spikes)
            {
                tmpColor.a = 0.3f;
                spike.GetComponent<SpriteRenderer>().color = tmpColor;
                spike.GetComponent<PolygonCollider2D>().enabled = false;
            }
            StartCoroutine(Warning());
            yield return new WaitForSeconds(3f);
            StartCoroutine(SpikeAppear());
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
            StartCoroutine(SpikeFade());
        }

    }

    IEnumerator Warning() //Warning Phase 2
    {
        yield return new WaitForSeconds(waitingTimeForWarningSign);                             //time wait = time wait in spike Appear - waitingTimeForWarningSign
        warningSign.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 255f);
        startPattern = false;
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
        foreach (GameObject laser in lasers)
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
        curPos = transform.position;
        //play intro like put name tag there with cool image or smth
        if (firstTimeFight != true)
        {
            startAttack = true;
        }
        else
        {
            currentPhase = BossPhase.Phase1;
            yield break;
        }
        yield return new WaitForSeconds(2f);
        if (startAttack)    //Start phase 1
        {
            spawnDetectionScriptOn = true;
            currentPhase = BossPhase.Phase1;
        }
    }

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnEnemyAnimation());
        yield return new WaitForSeconds(1f);
        StartCoroutine(OffSpawnEnemyAnimation());
        foreach (GameObject spawnPoint in spawnPoints)
        {
            _prefabEnemy = Instantiate(normalEnemy, spawnPoint.transform.position, Quaternion.identity); //spawn enemy on player previous pos
            _prefabEnemy.GetComponent<NormalEnemyScript>().enemyBoss = true;
        }
    }

    public IEnumerator SpawnEnemyAnimation()
    {
        foreach (GameObject spawnPoint in spawnPoints)
            spawnPoint.GetComponent<SpriteRenderer>().enabled = true;       //Get Portal Effect or Something
        yield return null;
    }

    IEnumerator OffSpawnEnemyAnimation()
    {
        foreach (GameObject spawnPoint in spawnPoints)
            spawnPoint.GetComponent<SpriteRenderer>().enabled = false;        //Get Portal Effect or Something
        yield return null;
    }

    IEnumerator WaitxSeconds(float x)
    {
        yield return new WaitForSeconds(x);
    }

    IEnumerator BossTease()
    {
        animator.enabled = false;
        //Start Taunting like come and get me or something
        if (Vector2.Distance(transform.position, player.transform.position) < 7f)
            transform.position = Vector2.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 7f), Time.deltaTime);
        else
            transform.position = Vector2.Lerp(transform.position, curPos, Time.deltaTime);

        timerBoss -= Time.deltaTime;
        if (timerBoss < 0)
            LevelSequencer.instance.unlockDockLevel2 = true;
        yield return null;
    }

    IEnumerator FadeImage()
    {
        // fade from opaque to transparent
        if (startFade)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                Sign.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                yield return null;
            }
            startFade = false;
        }
        // fade from transparent to opaque
        if(!startFade)
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                Sign.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                yield return null;
            }
            startFade = true;
        }
    }


}
