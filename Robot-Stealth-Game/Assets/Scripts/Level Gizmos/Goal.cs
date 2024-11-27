using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] int exitNr;
    private EndLevel endLevel;
    private float startTime; // Keep track of when the player entered the level

    private void Start()
    {
        endLevel = GameObject.Find("GameManager").GetComponent<EndLevel>();
        startTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player touches the goal: finish the level & save level time in the (abused) volume manager
        if(other.gameObject.name == "PlayerBody")
        {
            endLevel.CompleteLevelVictory();
            VolumeManager.Instance.exitNr = exitNr;
            VolumeManager.Instance.levelTime = Time.time - startTime;
        }
    }
}
