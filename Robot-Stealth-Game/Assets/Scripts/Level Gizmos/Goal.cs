using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] EndLevel endLevel;

    private void Start()
    {
        endLevel = GameObject.Find("GameManager").GetComponent<EndLevel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player touches the goal: finish the level
        if(other.gameObject.name == "Player")
        {
            endLevel.CompleteLevelVictory();
        }
    }
}
