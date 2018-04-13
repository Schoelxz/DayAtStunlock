using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Highscore : MonoBehaviour {

    static string filePath;
    static List<Score> scores = new List<Score>();

    // Use this for initialization
    void Start() {
        filePath = "Assets/Resources/Score/Highscore.txt";
        BuildListsOnStartup();
        SortHighscore();
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

    // Update is called once per frame
    void Update() {

        if(Input.GetKeyDown(KeyCode.W))
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            
            for (int i = 0; i < scores.Count; i++)
            {
                writer.WriteLine(scores[i]);
            }
            writer.Close();
        }
    }

    static public void SaveHighscore()
    {
        StreamWriter writer = new StreamWriter(filePath);
 
        for (int i = 0; i < scores.Count; i++)
        {
            writer.WriteLine(scores[i].Amount + " " + scores[i].Name);
        }

        writer.Close();
    }

    static public void AddHighscore(string name, int score)
    {
        scores.Add(new Score(score, name));
    }

    static public void SortHighscore()
    {
        scores = scores.OrderBy(s => s.Amount).ToList();
        scores.Reverse();
    }

    void BuildListsOnStartup()
    {
        string line;
        StreamReader reader = new StreamReader(filePath);

        while((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split();
            print(parts.Length);
            scores.Add(new Score(int.Parse(parts[0]), parts[1]));
        }
    }
}
