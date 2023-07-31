using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if(SceneManager.GetActiveScene().name == "Docks")
            {
                LevelSequencer.instance.unlockTerraceHouseLevel = true;
            }
            if(SceneManager.GetActiveScene().name == "Docks 2")
            {
                LevelSequencer.instance.unlockTerraceHouseLevel2 = true;
            }
        }
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, transform.GetComponent<MovingSpikeBlock>().patrolPoints[1].transform.position) <= 0.2f)
        {
            if(LevelSequencer.instance.unlockTerraceHouseLevel == true)
            {
                SceneManager.LoadScene("Terrace House");
                LevelSequencer.instance.unlockTerraceHouseLevel = false;
            }
            if(LevelSequencer.instance.unlockTerraceHouseLevel2 == true)
            {
                SceneManager.LoadScene("Terrace House 2");
                LevelSequencer.instance.unlockTerraceHouseLevel2 = false;
            }
        }
    }
}
