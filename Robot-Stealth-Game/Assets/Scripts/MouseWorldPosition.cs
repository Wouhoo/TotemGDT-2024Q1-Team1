using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseWorldPosition
{
    // This method returns the mouse world position by raycasting to the terrain or collider
    public static Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Declare a RaycastHit to store hit information
        RaycastHit hit;

        // Cast the ray onto the terrain (or any collider with physics) and return the hit point if it hits something
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point; // Return the point where the ray hits the terrain or collider
        }

        // If nothing is hit, return a default value (could adjust based on your needs)
        return Vector3.zero;
    }
}
