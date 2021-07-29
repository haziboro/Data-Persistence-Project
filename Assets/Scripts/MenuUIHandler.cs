using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] InputField playerName;

    private string textToDisplay;

    private void Start()
    {
        playerName.text = ScoreTracker.instance.playerName;
        UpdateHighScore();
    }

    //Updates the high score display on the menu
    void UpdateHighScore()
    {
        for (int i = 0; i < ScoreTracker.highScoresToDisplay; i++)
        {
            textToDisplay += (i+1) + ". " + ScoreTracker.instance.GetHighScoreName(i)
                + ": " + ScoreTracker.instance.GetHighScoreScore(i) + "\n";
        }
        highScoreText.SetText(textToDisplay);
    }

    //Starts a new game
    public void StartNewGame()
    {
        ScoreTracker.instance.playerName = playerName.text;
        SceneManager.LoadScene(1);
    }

    //Quits the application
    public void ExitGame()
    {
        ScoreTracker.instance.SaveScores();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
