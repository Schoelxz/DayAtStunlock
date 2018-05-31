using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Highscore : MonoBehaviour {

    private static string s_filePath;
    private static int s_maxListSize;

    public static List<Score> s_playerScoresList = new List<Score>();
    public static Score s_latestAddedScore;

    // Use this for initialization
    private void Start()
    {
        InitFile(); //Creates Highscore.txt if it does not exist.
        s_maxListSize = 10;
        BuildListsOnStartup();
        SortHighscore();
        SaveHighscore();
    }

    public class Score
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Time { get; set; }

        public Score(int amount, string name, int time)
        {
            Amount = amount;
            Name = name;
            Time = time;
        }
    }

    private void InitFile()
    {
        s_filePath = Application.persistentDataPath + "/Highscore.txt"; // "Assets /Resources/Score/Highscore.txt";

        Debug.Log("filepath: " + s_filePath);

        if (!File.Exists(s_filePath))
            CreateEmptyFile(s_filePath);
    }

    private void CreateEmptyFile(string filename)
    {
        File.Create(filename).Dispose();
    }

    public static void SaveHighscore()
    {
        StreamWriter writer = new StreamWriter(s_filePath, false);
 
        for (int i = 0; i < s_playerScoresList.Count; i++)
            writer.WriteLine(s_playerScoresList[i].Amount + " " + s_playerScoresList[i].Name + " " + s_playerScoresList[i].Time);

        writer.Close();
    }

    public static void AddHighscore(string name, int score, int time)
    {
        if(CheckIfAdded(name,score) == false)
        {
            s_playerScoresList.Add(new Score(score, name, time));
            s_latestAddedScore = s_playerScoresList[s_playerScoresList.Count - 1];
        }
    }

    public static void SortHighscore()
    {
        s_playerScoresList = s_playerScoresList.OrderBy(s => s.Amount).ToList();
        s_playerScoresList.Reverse();

        while(s_playerScoresList.Count > s_maxListSize)
            s_playerScoresList.RemoveAt(s_playerScoresList.Count - 1);
    }

    private void BuildListsOnStartup()
    {
        s_playerScoresList.Clear();
        string line;
        StreamReader reader = new StreamReader(s_filePath);

        while((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split();
            if(parts.Length != 3)
            {
                Debug.LogWarning("Highscore.txt had not enough elements or too many elements, changed highscore with -1 in timeScore for those elements.");
                string[] temp = { parts[0], parts[1], "-1"};
                parts = temp;
            }

            if (parts[0] == "")
                continue;

            int playerScore;
            int playerTime;

            if (int.TryParse(parts[0], out playerScore))
                s_playerScoresList.Add(new Score(playerScore, parts[1], int.TryParse(parts[2], out playerTime) ? playerTime : -1));
            else
            {
                if (int.TryParse(parts[1], out playerScore))
                    s_playerScoresList.Add(new Score(playerScore, parts[0], int.TryParse(parts[2], out playerTime) ? playerTime : -1));
                else
                    Debug.LogWarning("Highscore tried parsing a string as an int, but failed. String was: " + parts[0] + " or " + parts[1]);
            }
        }

        reader.Close();
    }

    public static bool CheckIfAdded(string name, int score)
    {
        foreach (var pScore in s_playerScoresList)
            if (pScore.Amount == score && pScore.Name == name)
                return(true);
            
        return false;
    }
}
