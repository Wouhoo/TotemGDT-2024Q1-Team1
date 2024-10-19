using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public int armSegments;
    public float armYOffset;
    public GameObject armNodePrefab;
    public GameObject handNodePrefab;

    public LineRenderer lineRenderer;
    public LayerMask collMask;

    private List<Transform> armNodes = new List<Transform>();

    private void Awake()
    {
        // The player is the first arm node
        GameObject player = GameObject.FindWithTag("Player");
        armNodes.Add(player.transform);
        Rigidbody prevRB = player.GetComponent<Rigidbody>();

        SpringJoint springJoint;

        Vector3 spawnPosition = player.transform.position;
        spawnPosition.y = armYOffset;
        for (var i = 0; i < armSegments; i++)
        {
            // Instantiate the segment
            GameObject armNode = Instantiate(armNodePrefab, spawnPosition, Quaternion.identity, transform);
            springJoint = armNode.GetComponent<SpringJoint>();

            // Set the connected body
            springJoint.connectedBody = prevRB;
            prevRB = springJoint.GetComponent<Rigidbody>();

            armNodes.Add(armNode.transform);
        }

        GameObject handNode = Instantiate(handNodePrefab, spawnPosition, Quaternion.identity, transform);
        springJoint = handNode.GetComponent<SpringJoint>();

        // Set the connected body
        springJoint.connectedBody = prevRB;

        armNodes.Add(handNode.transform);

        lineRenderer = GetComponent<LineRenderer>();
        UpdateLineRenderer();
    }

    private void Update()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = armNodes.Count;
        for (int i = 0; i < lineRenderer.positionCount; i++)
            lineRenderer.SetPosition(i, armNodes[i].position);
    }
}