using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleArmController : MonoBehaviour
{

    public GameObject noodleArmHandPrefab; // Reference to the prefab in the Inspector
    private bool isActive = false;
    private GameObject noodleArmHand;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //conjureth the noodle boi
            if(!isActive)
            {
                Vector3 spawnPosition = transform.position + transform.forward * 1.0f; // 1.0f can be adjusted
                noodleArmHand = Instantiate(noodleArmHandPrefab, spawnPosition, Quaternion.identity);
                isActive = true;
            }
            else //get rid of it
            {
                Destroy(noodleArmHand);
                isActive = false;
            }

        }
    }
}
