using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerMovement : MonoBehaviour
{
    public float friction = 5f;
    public float maxSpeed = 9999f;
    private GameObject player;
    private Movement_Controller playerController;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<Movement_Controller>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player)
            playerController.SetMovement(friction, maxSpeed);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == player)
            playerController.ResetMaxSpeed();
    }
}
