using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    // General class for finishing the level, attached to GameManager object

    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] TextMeshProUGUI gameOverText;
    private GameObject player;
    private IEnumerator coroutine;
    private bool levelFinished = false;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void CompleteLevelVictory()
    {
        // Finish the level victoriously (when player reaches end goal)
        if (!levelFinished)
        {
            coroutine = CompleteLevel(victoryText);
            StartCoroutine(coroutine);
        }
    }

    public void CompleteLevelDefeat()
    {
        // Finish the level in defeat (when player gets caught)
        if (!levelFinished)
        {
            coroutine = CompleteLevel(gameOverText);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator CompleteLevel(TextMeshProUGUI textToDisplay)
    {
        // Do everything that needs to be done to finish the level
        // For now, this is just popping up the victory/game over text, disabling player control, and quitting the game.
        levelFinished = true;
        textToDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        player.GetComponent<Movement_Controller>().enabled = false;
        yield return new WaitForSeconds(1f);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
