using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Highscore : MonoBehaviour {

    static string filePath;
    static string[] scores = new string[] { "hey", "hey", "hey" };

    // Use this for initialization
    void Start() {
        filePath = "Assets/Resources/Score/Highscore.txt";
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetKeyDown(KeyCode.W))
        {
            StreamWriter writer = new StreamWriter(filePath);

            foreach (var item in scores)
            {
                writer.WriteLine(item);
            }
            writer.Close();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            StreamReader reader = new StreamReader(filePath);
            Debug.Log(reader.ReadToEnd());
            reader.Close();
        }
    }

    static public void SaveHighscore()
    {
        for(int i = 0; i < MoneyManager.highscoreList.Count; i++)
        {
            scores[i] = MoneyManager.highscoreList[i];
        }
       
        File.WriteAllLines(filePath, scores);
    }
}
