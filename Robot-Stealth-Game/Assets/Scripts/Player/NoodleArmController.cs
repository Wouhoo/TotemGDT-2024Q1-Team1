using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleArmController : MonoBehaviour
{

    public GameObject player;
    public GameObject handPrefab;
    public GameObject armSegmentPrefab;
    public KeyCode noodleKey = KeyCode.Mouse0;

    public GameObject anchorSegment;

    //TODO: grab actual length of component
    public float maxSegmentLength = 1f;  // Length of an arm segment
    public int maxNumberOfSegments = 30;  // This times maxSegmentLength is the maximum total arm length
    public float armYOffset = 0.5f;

    //list of arm fragments, ordered from player towards hand!
    private List<GameObject> armSegments;
    private GameObject leadingArmSegment;

    private bool isDeployed = false;

    void Start()
    {
        //TODO: maybe not the best solution but this is the best option
        player = GameObject.FindWithTag("Player");

        Vector3 spawnPosition = transform.position;
        spawnPosition.y = armYOffset;
        anchorSegment = Instantiate(armSegmentPrefab, spawnPosition, Quaternion.identity);
        armSegments = new List<GameObject> { anchorSegment };
        leadingArmSegment = anchorSegment;
        ConnectFragments(player, anchorSegment);
    }


    private void AddArmSegment()
    {
        GameObject newSegment = Instantiate(armSegmentPrefab, anchorSegment.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeployed)
        {
            float distance;
            //get distance from 
            //Debug.Log(armFragments[0]);
            distance = Vector3.Distance(player.transform.position, armSegments[0].transform.position);

            // Check if a new fragment needs to be spawned
            if (distance > maxSegmentLength)
            {
                //Debug.Log("SPAWNING NEW ARM FRAGMENT: " + distance);
                // Spawn a new fragment
                //SpawnArmFragment();
            }
        }
    }

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
