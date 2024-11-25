using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryStatText : MonoBehaviour
{
    private TextMeshProUGUI statText;

    // Display stats for the player's run
    void Start()
    {
        statText = GetComponent<TextMeshProUGUI>();
        int entranceNr = VolumeManager.Instance.levelNr;
        int exitNr = VolumeManager.Instance.exitNr;
        int levelMinutes = (int) VolumeManager.Instance.levelTime / 60;
        int levelSeconds = (int) VolumeManager.Instance.levelTime % 60;
        statText.text = string.Format("Entrance: {0} \n Exit: {1} \n Time: {2:00}:{3:00}", entranceNr, exitNr, levelMinutes, levelSeconds);
    }
}
