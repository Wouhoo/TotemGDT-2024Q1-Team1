using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> entrances;
    GameObject player;

    // Spawn the player in the correct spot based on the entrance selected in the main menu
    void Awake()
    {
        player = GameObject.Find("Player");
        Vector3 entrancePosition = entrances[VolumeManager.Instance.levelNr-1].transform.position;
        player.transform.position = new Vector3(entrancePosition.x, 1, entrancePosition.z);
    }
}
