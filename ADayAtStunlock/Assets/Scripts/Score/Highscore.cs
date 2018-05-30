using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Highscore : MonoBehaviour {

    static string filePath;
    static public List<Score> playerScoresList = new List<Score>();
    static int maxListSize;
    public static Score latestAddedScore;

    // Use this for initialization
    void Start()
    {
        InitFile(); //Creates Highscore.txt if it does not exist.
        maxListSize = 10;
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
        filePath = Application.persistentDataPath + "/Highscore.txt"; // "Assets /Resources/Score/Highscore.txt";

        Debug.Log("filepath: " + filePath);

        if (!File.Exists(filePath))
            CreateEmptyFile(filePath);
    }

    private void CreateEmptyFile(string filename)
    {
        File.Create(filename).Dispose();
    }

    public static void SaveHighscore()
    {
        StreamWriter writer = new StreamWriter(filePath, false);
 
        for (int i = 0; i < playerScoresList.Count; i++)
            writer.WriteLine(playerScoresList[i].Amount + " " + playerScoresList[i].Name + " " + playerScoresList[i].Time);

        writer.Close();
    }

    public static void AddHighscore(string name, int score, int time)
    {
        if(CheckIfAdded(name,score) == false)
        {
            playerScoresList.Add(new Score(score, name, time));
            latestAddedScore = playerScoresList[playerScoresList.Count - 1];
        }
    }

    public static void SortHighscore()
    {
        playerScoresList = playerScoresList.OrderBy(s => s.Amount).ToList();
        playerScoresList.Reverse();

        while(playerScoresList.Count > maxListSize)
            playerScoresList.RemoveAt(playerScoresList.Count - 1);
    }

    private void BuildListsOnStartup()
    {
        playerScoresList.Clear();
        string line;
        StreamReader reader = new StreamReader(filePath);

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
                playerScoresList.Add(new Score(playerScore, parts[1], int.TryParse(parts[2], out playerTime) ? playerTime : -1));
            else
            {
                if (int.TryParse(parts[1], out playerScore))
                    playerScoresList.Add(new Score(playerScore, parts[0], int.TryParse(parts[2], out playerTime) ? playerTime : -1));
                else
                    Debug.LogWarning("Highscore tried parsing a string as an int, but failed. String was: " + parts[0] + " or " + parts[1]);
            }
        }

        reader.Close();
    }

    public static bool CheckIfAdded(string name, int score)
    {
        foreach (var pScore in playerScoresList)
            if (pScore.Amount == score && pScore.Name == name)
                return(true);
            
        return false;
    }
}
