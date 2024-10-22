using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;

    [Header("Nodes")]
    public GameObject targetNodePrefab;
    public GameObject handNodePrefab;
    public GameObject armNodePrefab;
    public GameObject anchorNode;
    private SpringJoint handNodeJoint;
    private Rigidbody targetNodeRB;
    private Transform[] armNodes; // used to rener the arm

    [Header("Adjustable Settings")]
    public float armSpring;
    public float armDampner;
    public float handSpring;
    public float handDampner;
    public float nodeDrag;

    [Header("Initial Conditions & Other")]
    public bool isDeployed = true;
    public int armNodeCount = 15; // MUST BE MORE THAN 2!!! (im not building robust code here, ensure this yourself)
    public KeyCode deployArmKey = KeyCode.Space;

    private void Awake()
    {
        // Get script components
        meshFilter = GetComponent<MeshFilter>();
        lineRenderer = GetComponent<LineRenderer>();

        // Set up the arm nodes array (with fixed length)
        armNodes = new Transform[armNodeCount];
        lineRenderer.positionCount = armNodeCount;

        // Precalculate node spawning position
        Vector3 spawnPosition = anchorNode.transform.position;

        // Setup target node & get its rigid body (for later update)
        GameObject targetNode = Instantiate(targetNodePrefab, spawnPosition, Quaternion.identity, transform);
        targetNodeRB = targetNode.GetComponent<Rigidbody>();

        // Setup hand node & get spring joint (for later update)
        GameObject handNode = Instantiate(handNodePrefab, spawnPosition, Quaternion.identity, transform);
        SetupSpringJoint(handNode, targetNodeRB, handSpring, handDampner, 0);
        handNodeJoint = handNode.GetComponent<SpringJoint>();
        // Set previous rigid body
        Rigidbody prevRB = handNode.GetComponent<Rigidbody>();
        prevRB.drag = nodeDrag; // set drag

        // Add the rest of the arm
        for (int i = 1; i < armNodeCount - 1; i++) // start at 1 for the hand arm node (no security checking here :p)
        {
            // Instantiate the segment
            GameObject armNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            SetupSpringJoint(armNode, prevRB, armSpring, armDampner, i);
            // Update previous rigid body
            prevRB = armNode.GetComponent<Rigidbody>();
            prevRB.drag = nodeDrag; // set drag
        }

        // Set up body joint
        SetupSpringJoint(anchorNode, prevRB, armSpring, armDampner, armNodeCount - 1);
    }

    private void Update()
    {
        // We update the visuals
        UpdateLineRenderer();

        // We now look if we should retract or deploy the arm
        if (Input.GetKeyDown(deployArmKey))
        {
            if (isDeployed)
                handNodeJoint.spring = 0f;
            else
                handNodeJoint.spring = handSpring;
            isDeployed = !isDeployed;
        }
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < armNodeCount; i++)
            lineRenderer.SetPosition(i, armNodes[i].transform.position - transform.position);
        lineRenderer.BakeMesh(meshFilter.mesh, true);
    }

    private void SetupSpringJoint(GameObject node, Rigidbody rigidbody, float spring, float dampner, int index)
    {
        SpringJoint joint = node.GetComponent<SpringJoint>();
        // Set the connected body
        joint.connectedBody = rigidbody;
        // Set the parameters
        joint.spring = spring;
        joint.damper = dampner;
        // Add to array of nodes
        armNodes[index] = joint.transform;
    }
}