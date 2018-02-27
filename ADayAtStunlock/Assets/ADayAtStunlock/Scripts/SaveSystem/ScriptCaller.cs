using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptCaller : MonoBehaviour {

    private DAS.TimeSystem temp;
    private Game testGame;

    private void Awake()
    {
        Game.current = new Game();
        SaveLoad.Load();//
        if(SaveLoad.savedGames.Count != 0)
        {
            Game.current = SaveLoad.savedGames[0];
        }
        DAS.TimeSystem.TimePassedSeconds = Game.current.timeSystemData.timePassedSeconds;
        DAS.TimeSystem.TimeMultiplier = Game.current.timeSystemData.timeMultiplier;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
        if(Input.GetKeyDown(KeyCode.S))
        {

            //temp = new DAS.TimeSystem.;
            SaveLoad.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //DAS.TimeSystem.timeInstance = testGame.timeSystem;
            
            SaveLoad.Load();
        }
    }
}
