using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleArmController : MonoBehaviour
{
    public Transform player;
    public GameObject armFragmentPrefab;

    //TODO: grab actual length of component
    private float fragmentLength;  // Distance between fragments

    //list of arm fragments, ordered from player towards hand!
    private List<GameObject> armFragments;

    //get the reference to the player's transform
    public NoodleArmController(Transform player)
    {
        this.player = player;
    }

    void Start()
    {
        //TODO: maybe not the best solution but this is the best option
        player = GameObject.FindWithTag("Player").transform;

        //get the length of the arm fragments
        if (armFragmentPrefab != null)
        {
            //TODO: sucks and does not work
            fragmentLength = armFragmentPrefab.GetComponent<Renderer>().bounds.size.y + 1.5f;
        }

        armFragments = new List<GameObject>();
        armFragments.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float distance;
        //get distance from 
        //Debug.Log(armFragments[0]);
        distance = Vector3.Distance(player.position, armFragments[0].transform.position);
        
        // Check if a new fragment needs to be spawned
        if (distance > fragmentLength)
        {
            Debug.Log("SPAWNING NEW ARM FRAGMENT: " + distance);
            // Spawn a new fragment
            SpawnArmFragment();
        }
    }

    void SpawnArmFragment()
    {
        // Calculate the spawn position along the line between player and hand
        Vector3 spawnPosition = player.position + (armFragments[0].transform.position - player.position).normalized * fragmentLength;
        
        // Instantiate the fragment
        GameObject newFragment = Instantiate(armFragmentPrefab, spawnPosition, Quaternion.identity);
        
        // Optional: Set parent to player or hand to organize hierarchy
        //newFragment.transform.SetParent(player);

        // Add the fragment to the list
        armFragments.Insert(0, newFragment);

        // Optionally, add physics joints (e.g., HingeJoint or ConfigurableJoint) to connect it to the previous fragment
        if (armFragments.Count > 1)
        {
            //ConnectFragments(armFragments[armFragments.Count - 2], newFragment);
        }
    }
}
