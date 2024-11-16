using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    [Header("Arm Options")]
    public float armLength = 15f; // Maximum reasonable arm length
    public float armMassDensity = 3f; // Mass of arm per per unit length
    public float armSegmentDensity = 15f; // Number of nodes per unit length
    public float armDrag = 0.5f; // Drag constant of the arm (per unit length)
    public float armNodeAngularDrag = 3f; // Drag constant of the arm (per unit length)
    public float armNodeSpringConstant = 10f; // Spring constant of the arm (use for each node)
    public float armNodeDampingConstant = 0.5f; // Damping constant of the arm (use for each node)
    private int armSegmentCount;

    [Header("Nodes")]
    public GameObject targetNode;
    public GameObject handNode;
    public GameObject armNodePrefab;
    public GameObject anchorNode;
    private Transform[] armNodes; // used to rener the arm

    [Header("Hand Options")]
    public float driveSpringForce = 999999f;
    public float driveSpringDampner = 0.5f;
    public float maxDriveForce = 500f;

    [Header("Other")]
    public KeyCode deployArmKey = KeyCode.Space;
    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;
    private bool isDeployed = false;

    private void Awake()
    {
        // Get script components
        meshFilter = GetComponent<MeshFilter>();
        lineRenderer = GetComponent<LineRenderer>();

        // Get all constant values
        armSegmentCount = (int)Math.Round(armSegmentDensity * armLength);
        float armNodeMass = armMassDensity * armLength / armSegmentCount;
        float armNodeDrag = armDrag * armLength / armSegmentCount;
        float armSegmentLength = armLength / armSegmentCount;

        // Set up the arm nodes array (with fixed length)
        armNodes = new Transform[armSegmentCount + 1]; // +1 for the body node
        lineRenderer.positionCount = armSegmentCount + 1;

        // Get node spawning position (anchor point)
        Vector3 spawnPosition = anchorNode.transform.position;

        // Setup anchor node
        Rigidbody prevRB = anchorNode.GetComponent<Rigidbody>();
        armNodes[0] = anchorNode.transform;

        // Add the rest of the arm
        for (int i = 1; i < armSegmentCount; i++) // start at 1 end at n-1 for the hand and anchor nodes (no security checking here :p)
        {
            // Instantiate the segment
            GameObject armNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            // Set up arm node
            ConnectJoint(armNode, prevRB);
            CalibrateJointDrive(armNode, armNodeSpringConstant, armNodeDampingConstant, 999999999f);
            CalibrateJointLimit(armNode, armSegmentLength);
            armNodes[i] = armNode.transform;
            prevRB = armNode.GetComponent<Rigidbody>();
            CalibrateRigidbody(prevRB, armNodeMass, armNodeDrag, armNodeAngularDrag);
        }

        // Set up hand node
        ConnectJoint(handNode, prevRB);
        CalibrateJointDrive(handNode, armNodeSpringConstant, armNodeDampingConstant, 999999999f);
        CalibrateJointLimit(handNode, armSegmentLength);
        armNodes[armSegmentCount] = handNode.transform;
        prevRB = handNode.GetComponent<Rigidbody>();
        CalibrateRigidbody(prevRB, armNodeMass, armNodeDrag, armNodeAngularDrag);

        // Set up target node
        CalibrateJointDrive(targetNode, driveSpringForce, driveSpringDampner, maxDriveForce);
    }

    private void Update()
    {
        // We update the visuals
        UpdateLineRenderer();

        // We now look if we should retract or deploy the arm
        if (Input.GetKeyDown(deployArmKey))
        {
            if (isDeployed)
            {
                targetNode.SetActive(false);
            }
            else
            {
                targetNode.SetActive(true);
            }
            isDeployed = !isDeployed;
        }
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i <= armSegmentCount; i++)
            lineRenderer.SetPosition(i, armNodes[i].transform.position);
        lineRenderer.BakeMesh(meshFilter.mesh, true);
    }

    private void ConnectJoint(GameObject node, Rigidbody connectedRigidbody)
    {
        ConfigurableJoint joint = node.GetComponent<ConfigurableJoint>();
        joint.connectedBody = connectedRigidbody;
    }

    private void CalibrateJointDrive(GameObject node, float spring, float dampner, float maxForce)
    {
        ConfigurableJoint joint = node.GetComponent<ConfigurableJoint>();
        JointDrive drive = joint.xDrive;
        drive.positionSpring = spring;
        drive.positionDamper = dampner;
        drive.maximumForce = maxForce;
        joint.xDrive = drive;
    }

    private void CalibrateJointLimit(GameObject node, float maxDist)
    {
        ConfigurableJoint joint = node.GetComponent<ConfigurableJoint>();
        SoftJointLimit limit = joint.linearLimit;
        limit.limit = maxDist;
        joint.linearLimit = limit;
    }

    private void CalibrateRigidbody(Rigidbody rigidbody, float mass, float drag, float angularDrag)
    {
        rigidbody.mass = mass;
        rigidbody.drag = drag;
        rigidbody.angularDrag = angularDrag;
    }
}