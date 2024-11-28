using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerGetsHit : MonoBehaviour
{
    private EndLevel endLevel;

    private void Start()
    {
        endLevel = GameObject.Find("GameManager").GetComponent<EndLevel>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When player gets caught by an enemy: do something
        // For now this is instant game over, but we may want to add health later.
        if (collision.gameObject.tag == "Enemy")
            endLevel.CompleteLevelDefeat();
    }
}
