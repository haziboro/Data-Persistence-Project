using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private string playerName = "Default";
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        if (ScoreTracker.instance != null)
        {
            playerName = ScoreTracker.instance.playerName;
            DisplayBestScore();
        }
        SetScoreText();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    //Display the best score ever acheieved on the top of the game during play
    void DisplayBestScore()
    {
        string bestScoreName = ScoreTracker.instance.GetHighScoreName(0);
        int bestScoreScore = ScoreTracker.instance.GetHighScoreScore(0);
        BestScoreText.text = "Best Score: " + bestScoreName + ":" + bestScoreScore;
    }

    void UpdateHighScoreList()
    {
        if (ScoreTracker.instance != null)
        {
            ScoreTracker.instance.CheckHighScores(m_Points);
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        SetScoreText();
    }

    void SetScoreText()
    {
        ScoreText.text = playerName + "'s " + $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateHighScoreList();
    }
}
