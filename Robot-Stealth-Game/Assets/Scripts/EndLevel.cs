using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    // General class for finishing the level, attached to GameManager object

    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] SceneAsset nextLevel;
    private GameObject player;
    private IEnumerator coroutine;
    private bool levelFinished = false;

    private void Start()
    {
        player = GameObject.Find("PlayerBody");
    }

    public void CompleteLevelVictory()
    {
        // Finish the level victoriously (when player reaches end goal)
        if (!levelFinished)
        {
            coroutine = CompleteLevel(true);
            StartCoroutine(coroutine);
        }
    }

    public void CompleteLevelDefeat()
    {
        // Finish the level in defeat (when player gets caught)
        if (!levelFinished)
        {
            coroutine = CompleteLevel(false);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator CompleteLevel(bool victorious)
    {
        // Do everything that needs to be done to finish the level
        levelFinished = true;

        // Display correct text
        if(victorious)
            victoryText.gameObject.SetActive(true);
        else
            gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // Disable player control (note: arm remains enabled for a second, but I'm keeping that cause it's funny)
        player.GetComponent<Movement_Controller>().enabled = false;
        yield return new WaitForSeconds(1f);

        // Reload scene (defeat) or load next level (victory)
        if(victorious)
            SceneManager.LoadScene(nextLevel.name);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);



/* Debug version - just quit playmode or quit the app
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
*/
    }
}
