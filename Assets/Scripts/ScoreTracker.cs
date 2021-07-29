using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreTracker : MonoBehaviour
{

    public static ScoreTracker instance;

    public PlayerData[] highScores;
    public string playerName;
    public const int highScoresToDisplay = 5;

    //Runs on Awake
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        highScores = new PlayerData[highScoresToDisplay];
        for (int i = 0; i < highScoresToDisplay; i++)
        {
            highScores[i] = new PlayerData();
        }

        DontDestroyOnLoad(gameObject);
        LoadScores();
    }

    //Compares the new score to the old high scores, updating highScoreList
    public void CheckHighScores(int score)
    {
        if(score > highScores[highScoresToDisplay - 1].score)
        {
            List<PlayerData> scoresToSort = new List<PlayerData>(highScores);
            scoresToSort.Add(new PlayerData(score, playerName));

            scoresToSort.Sort((x, y) => x.score.CompareTo(y.score));
            scoresToSort.Reverse();
            scoresToSort.RemoveAt(scoresToSort.Count - 1);
            for(int i = 0; i < highScores.Length; i++)
            {
                highScores[i] = scoresToSort[i];
            }
        }
    }

    //Returns the highscore name
    public string GetHighScoreName(int index)
    {
        return highScores[index].name;
    }

    //Returns the highscore score
    public int GetHighScoreScore(int index)
    {
        return highScores[index].score;
    }

    //Clears the high score list
    void ClearHighScores()
    {
        for(int i = 0; i < highScoresToDisplay; i++)
        {
            highScores[i] = new PlayerData(0);
        }
    }

    //Subclass for storing player name/score pairs
    public class PlayerData
    {
        public PlayerData(int pScore = 0, string pName = "Doom")
        {
            score = pScore;
            name = pName;
        }
        public int score;
        public string name;
    }

    [System.Serializable]
    public class SavablePlayerData
    {
        public int score;
        public string name;
    }

    //Json Wrapper
    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] array;
    }

    //Saves the current set of highScores
    public void SaveScores()
    {
        SavablePlayerData[] data = new SavablePlayerData[highScoresToDisplay];
        Wrapper<SavablePlayerData> wrapper = new Wrapper<SavablePlayerData>();

        for (int i = 0; i < highScoresToDisplay; i++)
        {
            data[i] = new SavablePlayerData();
            data[i].score = highScores[i].score;
            data[i].name = highScores[i].name;
        }

        wrapper.array = data;
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Wrapper<SavablePlayerData> wrapper = JsonUtility.FromJson<Wrapper<SavablePlayerData>>(json);

            for (int i = 0; i < highScoresToDisplay; i++)
            {
                highScores[i].score = wrapper.array[i].score;
                highScores[i].name = wrapper.array[i].name;
            }
        }
        else
        {
            ClearHighScores();
        }
    }


}
