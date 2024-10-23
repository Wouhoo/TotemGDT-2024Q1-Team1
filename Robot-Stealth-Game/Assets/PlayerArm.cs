using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    public int armNodeCount = 15; // MUST BE MORE THAN 1!!! (im not building robust code here, ensure this yourself)
    public KeyCode deployArmKey = KeyCode.Space;
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

    [Header("Adjustable Settings (Arm)")]
    public float totalArmMass = 1f;
    public float armNodeDrag = 1f;
    public float armSpring = 1f;
    public float armDampner = 1f;

    [Header("Adjustable Settings (Hand)")]
    public float handMass = 1f;
    public float handDrag = 1f;
    public float handSpring = 1f;
    public float handDampner = 1f;

    private bool isDeployed = false;

    private void Awake()
    {
        // Get script components
        meshFilter = GetComponent<MeshFilter>();
        lineRenderer = GetComponent<LineRenderer>();

        // Set up the arm nodes array (with fixed length)
        armNodes = new Transform[armNodeCount + 1]; // +1 for the body node
        lineRenderer.positionCount = armNodeCount + 1;

        // Precalculate node spawning position
        Vector3 spawnPosition = anchorNode.transform.position;
        float nodeMass = totalArmMass / (armNodeCount - 1); // minus one for the hand node

        // Setup target node & get its rigid body (for later update)
        GameObject targetNode = Instantiate(targetNodePrefab, spawnPosition, Quaternion.identity, transform);
        targetNodeRB = targetNode.GetComponent<Rigidbody>();

        // Setup hand node & get spring joint (for later update)
        GameObject handNode = Instantiate(handNodePrefab, spawnPosition, Quaternion.identity, transform);
        SetupSpringJoint(handNode, targetNodeRB, 0f, handDampner, 0);
        handNodeJoint = handNode.GetComponent<SpringJoint>();
        // Set previous rigid body
        Rigidbody prevRB = handNode.GetComponent<Rigidbody>();
        prevRB.mass = handMass; // set mass
        prevRB.drag = handDrag; // set drag

        // Add the rest of the arm
        for (int i = 1; i < armNodeCount; i++) // start at 1 for the hand arm node (no security checking here :p)
        {
            // Instantiate the segment
            GameObject armNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            SetupSpringJoint(armNode, prevRB, armSpring, armDampner, i);
            // Update previous rigid body
            prevRB = armNode.GetComponent<Rigidbody>();
            prevRB.mass = nodeMass; // set mass
            prevRB.drag = armNodeDrag; // set drag
        }

        // Set up body joint
        SetupSpringJoint(anchorNode, prevRB, armSpring, armDampner, armNodeCount);
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
        for (int i = 0; i <= armNodeCount; i++)
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