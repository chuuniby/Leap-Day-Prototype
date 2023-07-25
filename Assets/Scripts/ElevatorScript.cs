using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public GameObject fakeBC;
    private void Awake()
    {
        transform.GetComponent<MovingSpikeBlock>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            fakeBC.SetActive(true);
            transform.GetComponent<MovingSpikeBlock>().enabled = true;
        }
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, transform.GetComponent<MovingSpikeBlock>().patrolPoints[1].transform.position) <= 0.2f)
        {
            LevelSequencer.instance.unlockTerraceHouseLevel = true;
            LevelSequencer.instance.ChangeLevel();
        }
    }
}
