using System.Collections.Generic;
using UnityEngine;

public class NoodleArm : MonoBehaviour
{
    public float reach = 10f;
    public int armSegments;
    public float armYOffset;
    public GameObject armNodePrefab;
    public GameObject handNodePrefab;


    public LayerMask collMask;

    private LineRenderer lineRenderer;
    private List<Transform> armNodes = new List<Transform>();

    private void Awake()
    {
        // The player is the first arm node
        GameObject player = GameObject.FindWithTag("Player");
        armNodes.Add(player.transform);
        Rigidbody prevRB = player.GetComponent<Rigidbody>();

        ConfigurableJoint joint;

        Vector3 spawnPosition = player.transform.position;
        spawnPosition.y = armYOffset;
        for (var i = 0; i < armSegments; i++)
        {
            // Instantiate the segment
            GameObject armNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            joint = armNode.GetComponent<ConfigurableJoint>();

            // Set the connected body
            joint.connectedBody = prevRB;
            // Limit the maximum reach
            SoftJointLimit limit = joint.linearLimit;
            limit.limit = reach / (armSegments + 1);
            joint.linearLimit = limit;

            prevRB = armNode.GetComponent<Rigidbody>();

            armNodes.Add(armNode.transform);
        }

        // Setup the final hand node
        GameObject handNode = Instantiate(handNodePrefab, spawnPosition, Quaternion.identity, transform);
        joint = handNode.GetComponent<ConfigurableJoint>();
        joint.connectedBody = prevRB;
        armNodes.Add(handNode.transform);

        // setup line renderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = armNodes.Count;
        UpdateLineRenderer();
    }

    private void Update()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
            lineRenderer.SetPosition(i, armNodes[i].position);
    }
}