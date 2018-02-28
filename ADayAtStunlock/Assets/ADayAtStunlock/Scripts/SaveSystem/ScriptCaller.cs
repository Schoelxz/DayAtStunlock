using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;
using System.ComponentModel.Design;

public class ScriptCaller : MonoBehaviour
{
    static int indexChooser = 0;

    private void Awake()
    {
        // Load a game set of values
        Game.current = new Game();
        SaveLoad.Load();

        if(SaveLoad.savedGames.Count != 0)
        {
            Game.current = ObjectCopier.Clone<Game>(SaveLoad.savedGames[0]);
        }
        DAS.TimeSystem.TimePassedSeconds = Game.current.timeSystemData.timePassedSeconds;
        DAS.TimeSystem.TimeMultiplier = Game.current.timeSystemData.timeMultiplier;

        Debug.Log(Application.persistentDataPath);
        
    }
    float waow = 0;

	void Update ()
    {
        waow += Time.deltaTime;
        if (waow > 0.004f)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveLoad.Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SaveLoad.Load();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                indexChooser++;
                if (indexChooser >= SaveLoad.savedGames.Count)
                    indexChooser = 0;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                indexChooser--;
                if (indexChooser < 0)
                    indexChooser = SaveLoad.savedGames.Count - 1;
            }

            waow = 0;
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(250f, 32f, 30, 30), indexChooser.ToString());

        for (int i = 0; i < SaveLoad.savedGames.Count; i++)
        {

            GUI.Button(new Rect(20f, i * 32f, 200, 30), i + " " + SaveLoad.savedGames[i].timeSystemData.timePassedSeconds);
        }

        if(SaveLoad.savedGames.Count != 0)
            GUI.Button(new Rect(300f, 62f, 200, 60), "" + SaveLoad.savedGames.IndexOf(Game.current) + " " + Game.current.timeSystemData.timePassedSeconds);
    }

    public static void ChangeGameCurrent()
    {
        Game.current = ObjectCopier.Clone<Game>(SaveLoad.savedGames[indexChooser]);
        DAS.TimeSystem.TimePassedSeconds = Game.current.timeSystemData.timePassedSeconds;
        DAS.TimeSystem.TimeMultiplier = Game.current.timeSystemData.timeMultiplier;
    }
}
