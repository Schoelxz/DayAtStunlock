using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
    private static Rect m_methodNamesPosition = new Rect(200, 50, 300, 50);

    static string currentMethodPlayed = "Current Event: \n";

    static int eventNamesLength = 0;

    static string AllMethodNames
    {
        get
        {
            string result = "All events inside RandomEvents: ";
            int amountOfEvents = 0;

            foreach (var events in RandomEventTrigger.randomEvents)
            {
                amountOfEvents++;
                result += "\n" + events.Method.Name;
            }
            m_methodNamesPosition = new Rect(Screen.width - 300, 50, 300, (21* amountOfEvents) + 2);
            return result;
        }
    }

    private void OnGUI()
    {
        GUI.Box(m_methodNamesPosition, AllMethodNames);

        GUI.Box(new Rect(Screen.width - 300, 0, 300, 42), currentMethodPlayed);

        int amountOfEvents = 0;
        foreach (var item in RandomEventTrigger.randomEvents)
        {
            eventNamesLength = 50 * amountOfEvents;
            if (GUI.Button(new Rect(Screen.width - 450, eventNamesLength, 150, 50), item.Method.Name))
            {
                FunctionTriggered(item);
            }
            amountOfEvents++;
        }

        GUI.Box(new Rect(Screen.width - 120, eventNamesLength + 50, 120, 38), "Current Difficulty: \n" + DifficultyManager.currentDifficulty.ToString());
        GUI.Box(new Rect(Screen.width - 120, eventNamesLength + 100, 120, 75), "Next difficulty in: \n" + DifficultyManager.s_myInstance.timeTilNextDifficulty + "\n at timestamp: \n" + DifficultyManager.s_myInstance.timeAtNextDifficulty);
        
    }

    public static System.Action FunctionTriggered(System.Action function)
    {
        //Calls the function (so it actually happens)
        function();

        Debug.Log(function.Method.Name);

        currentMethodPlayed = "Current Event: \n" + function.Method.Name;

        return function;
    }
}
