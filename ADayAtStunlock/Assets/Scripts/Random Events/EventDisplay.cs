using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
    public bool showGUI = true;

    private static Rect m_methodNamesPosition = new Rect(200, 50, 300, 50);

    private static string currentMethodPlayed = "Current Event: \n";

    private static int eventNamesLength = 0;

    private static string AllMethodNames
    {
        get
        {
            string result = "All events inside RandomEvents: ";
            int amountOfEvents = 0;

            foreach (var events in RandomEventTrigger.s_allEvents)
            {
                amountOfEvents++;
                result += "\n" + events.Method.Name;
            }
            m_methodNamesPosition = new Rect(Screen.width - 300, 100, 300, (21* amountOfEvents) + 2);
            return result;
        }
    }

    /// <summary>
    /// List of names of events that has been played, ordered from first played event to last played event.
    /// </summary>
    private static List<string> eventHistory = new List<string>();

    private void Start()
    {
        eventHistory.Clear();
    }

    private void OnGUI()
    {
        if (!DAS.DBUG.CheatsToCheat.CheatsEnabled)
            return;

        if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 50), "Toggle GUI"))
        {
            showGUI = !showGUI;
        }

        if (!showGUI)
            return;

        GUI.Box(m_methodNamesPosition, AllMethodNames);

        GUI.Box(new Rect(Screen.width - 300, 50, 300, 42), currentMethodPlayed);

        int amountOfEvents = 0;
        foreach (var item in RandomEventTrigger.s_allEvents)
        {
            eventNamesLength = (50 * amountOfEvents) + 50;
            if (GUI.Button(new Rect(Screen.width - 360, Screen.height - (25 + 25*amountOfEvents), 240, 25), item.Method.Name))
            {
                FunctionTriggered(item);
            }
            amountOfEvents++;
        }

        GUI.Box(new Rect(Screen.width - 540, 25, 240, 25), "Event history:");
        for (int i = 0; i < eventHistory.Count; i++)
        {
            GUI.Box(new Rect(Screen.width - 540, 50 + (25 * i), 240, 25), eventHistory[i]);
        }

        GUI.Box(new Rect(Screen.width - 120, eventNamesLength + 50, 120, 38), "Current Difficulty: \n" + DifficultyManager.currentDifficulty.ToString());
        GUI.Box(new Rect(Screen.width - 120, eventNamesLength + 100, 120, 75), "Next difficulty in: \n" + DifficultyManager.s_myInstance.timeTilNextDifficulty + "\n at timestamp: \n" + DifficultyManager.s_myInstance.timeAtNextDifficulty);
    }

    public static System.Action FunctionTriggered(System.Action function)
    {
        //Calls the function (so it actually happens)
        function();

        eventHistory.Add(function.Method.Name + " at: " + DAS.TimeSystem.TimePassedSeconds.ToString("0.00"));

        // Debug.Log(function.Method.Name);

        currentMethodPlayed = "Current Event: \n" + function.Method.Name;

        return function;
    }
}