using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();

    public static void Save()
    {
        SaveTimeSystemData(Game.current);



        savedGames.Add(Game.current);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        Debug.Log(Application.persistentDataPath);
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }
    
    private static void SaveTimeSystemData(Game game)
    {
        game.timeSystemData.timeMultiplier = DAS.TimeSystem.TimeMultiplier;
        game.timeSystemData.timePassedSeconds = DAS.TimeSystem.TimePassedSeconds;
    }

}
