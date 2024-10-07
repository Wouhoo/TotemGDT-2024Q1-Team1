using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class player_controller : MonoBehaviour
{


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                transform.position = hitInfo.point + new Vector3(0, 1, 0);
            }
        }
    }
}
