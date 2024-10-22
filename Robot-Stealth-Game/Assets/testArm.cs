using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class testArm : MonoBehaviour
{
    public float armLength = 10f;
    public int armSegmentLength;

    public GameObject armNodePrefab;
    public GameObject targetNodePrefab;
    public KeyCode noodleKey = KeyCode.Space;


    private GameObject targetNode;
    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;
    public List<GameObject> armNodes = new List<GameObject>();


    public float driveForce = 1f;
    public float dynamicDriveForce = 1f;
    public bool isDeployed = false;
    public bool isRetracting = false;
    float driveForceIncrease = 0.1f;

    public float targetYOffset;

    public float maxArmSegments;

    private ConfigurableJoint anchorJoint;

    private void Awake()
    {
        targetYOffset = transform.position.y;
        meshFilter = GetComponent<MeshFilter>();
        anchorJoint = GetComponent<ConfigurableJoint>();
        SoftJointLimitSpring limit = anchorJoint.linearLimitSpring;
        dynamicDriveForce = limit.spring;

        maxArmSegments = armLength / armSegmentLength;

        // Setup the final hand node
        targetNode = Instantiate(targetNodePrefab, transform.position, Quaternion.identity, transform);
        targetNode.SetActive(false);

        // Instantiate the first segment
        GameObject armNode = Instantiate(armNodePrefab, transform.position + transform.forward * armSegmentLength, Quaternion.identity, transform);
        // Set the connected body
        ConfigurableJoint joint = armNode.GetComponent<ConfigurableJoint>();
        joint.connectedBody = targetNode.GetComponent<Rigidbody>();
        // Add to list of nodes
        armNodes.Add(armNode);

        // setup line renderer
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLineRenderer();
    }

    private void Update()
    {
        UpdateLineRenderer();
        if (Input.GetKeyDown(noodleKey))
        {
            if (isDeployed)
            {
                targetNode.SetActive(false);
                isRetracting = true;
            }
            else
            {
                targetNode.SetActive(true);
                dynamicDriveForce = driveForce; // reset the spring constant
                isRetracting = false;
            }
            isDeployed = !isDeployed;
        }
        if (isDeployed)
        {
            Vector3 targetPosition = MouseWorldPosition.GetMouseWorldPosition();
            targetPosition.y = targetYOffset;
            targetNode.transform.position = targetPosition;
        }
    }



    private void FixedUpdate()
    {
        if (isDeployed)
        {
            Vector3 anchorToNearestNode = armNodes[0].transform.localPosition;
            if (anchorToNearestNode.magnitude > 2 * armSegmentLength && armNodes.Count < maxArmSegments)
                AddArmSegment(anchorToNearestNode.normalized * armSegmentLength);
            else if (anchorToNearestNode.magnitude < armSegmentLength / 2 && armNodes.Count > 1)
                RemoveArmSegment();
        }
        if (isRetracting)
        {
            if (armNodes.Count > 1)
            {
                dynamicDriveForce += driveForceIncrease * Time.deltaTime;
            }
            else
            {
                dynamicDriveForce = 0f;
                isRetracting = false;
            }

        }
    }

    private void AddArmSegment(Vector3 atRelativePosition)
    {
        ConfigurableJoint joint;

        // Instantiate the segment
        GameObject newArmNode = Instantiate(armNodePrefab, atRelativePosition + transform.position, Quaternion.identity, transform);
        // Set the connected body
        joint = newArmNode.GetComponent<ConfigurableJoint>();
        joint.connectedBody = armNodes[0].GetComponent<Rigidbody>();
        // Add to list of nodes
        armNodes.Insert(0, newArmNode);

        // reconnect nearest node to body
        anchorJoint.connectedBody = armNodes[0].GetComponent<Rigidbody>();
    }

    private void RemoveArmSegment()
    {
        // remove the node nearest to the body
        GameObject armNode = armNodes[0];
        armNodes.Remove(armNode);
        Destroy(armNode);

        // reconnect nearest node to body
        anchorJoint.connectedBody = armNodes[0].GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        targetYOffset = 0f;
    }

    void OnMouseUp()
    {
        targetYOffset = transform.position.y;
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = armNodes.Count;
        for (int i = 0; i < lineRenderer.positionCount; i++)
            lineRenderer.SetPosition(i, armNodes[i].transform.position);
        lineRenderer.BakeMesh(meshFilter.mesh, true);
    }
}