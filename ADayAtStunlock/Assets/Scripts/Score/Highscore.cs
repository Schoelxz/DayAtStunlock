using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Highscore : MonoBehaviour {

    static string filePath;
    static List<Score> scores = new List<Score>();
    static int maxListSize;

    // Use this for initialization
    void Start() {

        maxListSize = 15;
        filePath = "Assets/Resources/Score/Highscore.txt";
        BuildListsOnStartup();
        SortHighscore();
        SaveHighscore();
    }

    public class Score
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public Score(int amount, string name)
        {
            Amount = amount;
            Name = name;
        }
    }

    

    static public void SaveHighscore()
    {
        StreamWriter writer = new StreamWriter(filePath, false);
 
        for (int i = 0; i < scores.Count; i++)
        {
            writer.WriteLine(scores[i].Amount + " " + scores[i].Name);
        }

        writer.Close();
    }

    static public void AddHighscore(string name, int score)
    {
        if(CheckIfAdded(name,score) == false)
        {
            scores.Add(new Score(score, name));
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
            print(parts.Length);
            scores.Add(new Score(int.Parse(parts[0]), parts[1]));
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
