using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    public float armSegmentLength = 1f;
    public int armSegments = 15; // MUST BE MORE THAN 1!!! (im not building robust code here, ensure this yourself)
    public KeyCode noodleKey = KeyCode.Space;

    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;

    public GameObject armNodePrefab;
    public GameObject anchorNode;
    private ConfigurableJoint handNodeJoint;
    private Rigidbody targetNodeRB;
    private Transform[] armNodes; // used to rener the arm

    public bool isDeployed = true;
    public float handDriveForce = 10f;



    private void Awake()
    {
        // Get script components
        meshFilter = GetComponent<MeshFilter>();
        lineRenderer = GetComponent<LineRenderer>();
        ConfigurableJoint anchorJoint = anchorNode.GetComponent<ConfigurableJoint>();

        // Get the nodes
        GameObject handNode = GameObject.Find("HandNode");
        handNodeJoint = handNode.GetComponent<ConfigurableJoint>();
        GameObject targetNode = GameObject.Find("TargetNode");
        targetNodeRB = targetNode.GetComponent<Rigidbody>();

        Vector3 spawnPosition = anchorNode.transform.position;
        handNode.transform.position = spawnPosition;
        targetNode.transform.position = spawnPosition;

        // Connect hand to target
        handNodeJoint.connectedBody = targetNodeRB;
        // And set up the arm nodes array (with fixed length)
        armNodes = new Transform[armSegments];
        armNodes[0] = handNode.transform;
        lineRenderer.positionCount = armSegments;

        Rigidbody prevRB = handNode.GetComponent<Rigidbody>();
        SoftJointLimit limit;
        // Add the rest of the arm
        for (int i = 1; i < armSegments - 1; i++) // start at 1 for the hand arm node (no security checking here :p)
        {
            // Instantiate the segment
            GameObject newArmNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            ConfigurableJoint newJoint = newArmNode.GetComponent<ConfigurableJoint>();
            limit = newJoint.linearLimit;
            // Set the connected body & joint limits
            newJoint.connectedBody = prevRB;
            limit.limit = armSegmentLength;
            newJoint.linearLimit = limit; //set the joint's limit to our edited version.
            // Add to array of nodes
            armNodes[i] = newArmNode.transform;
            // Update previous rigid body
            prevRB = newArmNode.GetComponent<Rigidbody>();
        }
        // Connect body node to arm
        anchorJoint.connectedBody = prevRB;
        armNodes[armSegments - 1] = anchorJoint.transform;
        // Ensure limits
        limit = anchorJoint.linearLimit;
        limit.limit = armSegmentLength;
        anchorJoint.linearLimit = limit; //set the joint's limit to our edited version.
    }

    private void Update()
    {
        // We update the visuals
        UpdateLineRenderer();

        // We now look if we should retract or deploy the arm
        if (Input.GetKeyDown(noodleKey))
        {
            if (isDeployed)
            {
                JointDrive xDrive = handNodeJoint.xDrive;
                xDrive.positionSpring = 0f;
                handNodeJoint.xDrive = xDrive;
                JointDrive zDrive = handNodeJoint.zDrive;
                zDrive.positionSpring = 0f;
                handNodeJoint.zDrive = zDrive;
            }
            else
            {
                JointDrive xDrive = handNodeJoint.xDrive;
                xDrive.positionSpring = handDriveForce;
                handNodeJoint.xDrive = xDrive;
                JointDrive zDrive = handNodeJoint.zDrive;
                zDrive.positionSpring = handDriveForce;
                handNodeJoint.zDrive = zDrive;
            }
            isDeployed = !isDeployed;
        }
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < armSegments; i++)
            lineRenderer.SetPosition(i, armNodes[i].transform.position - transform.position);
        lineRenderer.BakeMesh(meshFilter.mesh, true);
    }

    /*
        private void AddArmSegment(Vector3 atRelativePosition)
        {
            // Instantiate the segment
            GameObject newArmNode = Instantiate(armNodePrefab, atRelativePosition + transform.position, Quaternion.identity, transform);
            ConfigurableJoint newJoint = newArmNode.GetComponent<ConfigurableJoint>();
            // Set the connected body
            newJoint.connectedBody = armNodes[0].GetComponent<Rigidbody>();
            // Add to list of nodes
            armNodes.Insert(0, newArmNode);

            // Connect body node to new arm node
            anchorJoint.connectedBody = newArmNode.GetComponent<Rigidbody>();
        }

        private void RemoveArmSegment()
        {
            // Remove the node nearest to the body
            GameObject oldArmNode = armNodes[0];
            armNodes.Remove(oldArmNode);
            Destroy(oldArmNode);

            // Connect body node to new arm node
            anchorJoint.connectedBody = armNodes[0].GetComponent<Rigidbody>();
        }
        */
}