using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Highscore : MonoBehaviour {

    static string filePath;
    static public List<Score> scores = new List<Score>();
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
            File.CreateText(filePath).Dispose();
    }

    static public void SaveHighscore()
    {
        StreamWriter writer = new StreamWriter(filePath, false);
 
        for (int i = 0; i < scores.Count; i++)
        {
            writer.WriteLine(scores[i].Amount + " " + scores[i].Name + " " + scores[i].Time);
        }

        writer.Close();
    }

    static public void AddHighscore(string name, int score, int time)
    {
        if(CheckIfAdded(name,score) == false)
        {
            scores.Add(new Score(score, name, time));
            latestAddedScore = scores[scores.Count - 1];
        }
    }

    static public void SortHighscore()
    {
        scores = scores.OrderBy(s => s.Amount).ToList();
        scores.Reverse();

        while(scores.Count > maxListSize)
        {
            scores.RemoveAt(scores.Count - 1);
        }

    }

    void BuildListsOnStartup()
    {
        scores.Clear();
        string line;
        StreamReader reader = new StreamReader(filePath);

        while((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split();

            if (parts[0] == "")
                continue;
            int asdfgh;
            if (int.TryParse(parts[0], out asdfgh))
                scores.Add(new Score(int.Parse(parts[0]), parts[1], int.Parse(parts[2])));
            else
                Debug.LogWarning("Highscore tried parsing an string as int, but failed. String was: " + parts[0]);
        }

        reader.Close();
    }

    static public bool CheckIfAdded(string name, int score)
    {
        foreach (var item in scores)
        {
            if (item.Amount == score && item.Name == name)
            {
                return(true);
            }
        }

        return false;
    }
}
