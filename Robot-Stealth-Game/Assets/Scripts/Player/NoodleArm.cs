using System.Collections.Generic;
using UnityEngine;

public class NoodleArm : MonoBehaviour
{
    public float reach = 10f;
    public int armSegments;
    public float armYOffset;
    public GameObject anchor;
    public GameObject armNodePrefab;
    public GameObject targetNodePrefab;
    public KeyCode noodleKey = KeyCode.Space;

    private Rigidbody leadingBody;
    private ConfigurableJoint targetJoint;
    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;
    private List<Transform> armNodes = new List<Transform>();

    private bool armDeployed = false;

    private void Awake()
    {

        meshFilter = GetComponent<MeshFilter>();

        // The player is the first arm node
        armNodes.Add(anchor.transform);
        Rigidbody prevRB = anchor.GetComponent<Rigidbody>();

        ConfigurableJoint leadingJoint;
        SoftJointLimit limit;

        Vector3 spawnPosition = anchor.transform.position;
        spawnPosition.y = armYOffset;
        for (var i = 0; i < armSegments; i++)
        {
            // Instantiate the segment
            GameObject armNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            leadingJoint = armNode.GetComponent<ConfigurableJoint>();

            // Set the connected body
            leadingJoint.connectedBody = prevRB;
            // Limit the maximum reach
            limit = leadingJoint.linearLimit;
            limit.limit = reach / (armSegments + 1);
            leadingJoint.linearLimit = limit;

            prevRB = armNode.GetComponent<Rigidbody>();

            armNodes.Add(armNode.transform);
        }

        // Setup the final hand node
        GameObject leadingNode = Instantiate(targetNodePrefab, spawnPosition, Quaternion.identity, transform);
        targetJoint = leadingNode.GetComponent<ConfigurableJoint>();
        leadingBody = prevRB;

        // setup line renderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = armNodes.Count;
        UpdateLineRenderer();
    }

    private void Update()
    {
        UpdateLineRenderer();
        if (Input.GetKeyDown(noodleKey))
        {
            if (armDeployed)
                targetJoint.connectedBody = null;
            else
                targetJoint.connectedBody = leadingBody;
            armDeployed = !armDeployed;
        }
        if (armDeployed)
        {
            Vector3 targetPosition = MouseWorldPosition.GetMouseWorldPosition();
            targetPosition.y = armYOffset;
            targetJoint.transform.position = targetPosition;
        }
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
            lineRenderer.SetPosition(i, armNodes[i].position);
        lineRenderer.BakeMesh(meshFilter.mesh, true);
    }
}