using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleArmController : MonoBehaviour
{
    
    public GameObject player;
    public GameObject handPrefab;
    public GameObject armFragmentPrefab;
    public KeyCode noodleKey = KeyCode.Mouse0;

    public GameObject noodleHand;

    //TODO: grab actual length of component
    private float fragmentLength;  // Distance between fragments

    //list of arm fragments, ordered from player towards hand!
    private List<GameObject> armFragments;

    private bool isDeployed = false;

    void Start()
    {
        //TODO: maybe not the best solution but this is the best option
        player = GameObject.FindWithTag("Player");

        //get the length of the arm fragments
        if (armFragmentPrefab != null)
        {
            //TODO: sucks and does not work
            fragmentLength = armFragmentPrefab.GetComponent<Renderer>().bounds.size.y + 1.5f;
        }
    }


    private void DeployNoodle()
    {
        Debug.Log("noodle deployed!");
        Vector3 spawnPosition = transform.position + transform.forward * 0.2f; // 1.0f can be adjusted
        noodleHand = Instantiate(handPrefab, spawnPosition, Quaternion.identity);

        armFragments = new List<GameObject> {noodleHand};

        //attach the hand to the player first
        ConnectFragments(player, noodleHand);
    }

    private void RetractNoodle()
    {
        Destroy(noodleHand);
        Debug.Log("noodle removed!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(noodleKey))
        {
            //conjureth the noodle boi
            if(!isDeployed)
            {
                isDeployed = true;
                DeployNoodle();
            }
            else //get rid of it
            {
                RetractNoodle();
                isDeployed = false;
            }

        }

        if(isDeployed)
        {
            float distance;
            //get distance from 
            //Debug.Log(armFragments[0]);
            distance = Vector3.Distance(player.transform.position, armFragments[0].transform.position);
            
            // Check if a new fragment needs to be spawned
            if (distance > fragmentLength)
            {
                //Debug.Log("SPAWNING NEW ARM FRAGMENT: " + distance);
                // Spawn a new fragment
                //SpawnArmFragment();
            }
        }
    }

    // void SpawnArmFragment()
    // {
    //     // Calculate the spawn position along the line between player and hand
    //     Vector3 spawnPosition = player.transform.position + (armFragments[0].transform.position - player.transform.position).normalized * fragmentLength;
        
    //     // Instantiate the fragment
    //     GameObject newFragment = Instantiate(armFragmentPrefab, spawnPosition, Quaternion.identity);
        
    //     // Optional: Set parent to player or hand to organize hierarchy
    //     //newFragment.transform.SetParent(player);

    //     // Add the fragment to the list
    //     armFragments.Insert(0, newFragment);

    //     // Optionally, add physics joints (e.g., HingeJoint or ConfigurableJoint) to connect it to the previous fragment
    //     if (armFragments.Count > 1)
    //     {
    //         //ConnectFragments(armFragments[armFragments.Count - 2], newFragment);
    //     }
    // }

    //connects the two objects with a hinge joint
    void ConnectFragments(GameObject previousFragment, GameObject newFragment)
    {
        //HingeJoint hinge = newFragment.AddComponent<HingeJoint>();
        HingeJoint hinge = newFragment.GetComponent<HingeJoint>();
        hinge.connectedBody = previousFragment.GetComponent<Rigidbody>();
        //hinge.anchor = Vector3.zero;
        hinge.autoConfigureConnectedAnchor = true;
    }
}
