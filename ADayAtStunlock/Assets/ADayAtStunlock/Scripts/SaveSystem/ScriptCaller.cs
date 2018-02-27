using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;
using System.ComponentModel.Design;

public class ScriptCaller : MonoBehaviour
{
    private void Awake()
    {
        // Load a game set of values
        Game.current = new Game();
        SaveLoad.Load();

        if(SaveLoad.savedGames.Count != 0)
        {
            Game.current = SaveLoad.savedGames[0];
        }
        DAS.TimeSystem.TimePassedSeconds = Game.current.timeSystemData.timePassedSeconds;
        DAS.TimeSystem.TimeMultiplier = Game.current.timeSystemData.timeMultiplier;
        
    }

    void Start ()
    {

    }

	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveLoad.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        { 
            SaveLoad.Load();
        }
    }

    private void OnGUI()
    {
        for (int i = 0; i < SaveLoad.savedGames.Count; i++)
        {

            GUI.Button(new Rect(20f, i * 32f, 200, 30), i + " " + SaveLoad.savedGames[i].timeSystemData.timePassedSeconds);
        }

        if(SaveLoad.savedGames.Count != 0)
            GUI.Button(new Rect(300f, 62f, 200, 60), "" + SaveLoad.savedGames.IndexOf(Game.current) + " " + Game.current.timeSystemData.timePassedSeconds);
    }
}
