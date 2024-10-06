using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Static object that gives out colors to enemies to discern them and their patrol paths
in the editor. Could maybe use this in the future for tagging general objects for level editing
- Lars
*/

public static class GizmoColorManager
{
    //global counter
    private static int colorIndex = 0;

    //List of possible colors
    public static List<Color> colorList = new List<Color>
    {
        Color.yellow,
        Color.green,
        Color.red,
        Color.blue,
        Color.cyan,
        Color.magenta
        //TODO: maybe add more
    };

    //function to just get a color, gets called upon creation of an enemy
    public static Color GetNextColor()
    {
        Color colorToAssign = colorList[colorIndex % colorList.Count];
        colorIndex++;
        return colorToAssign;
    }
}
