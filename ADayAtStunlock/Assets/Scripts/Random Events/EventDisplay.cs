using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
    private static Rect m_methodNamesPosition = new Rect(200, 50, 300, 50);

    static string methodName = "Current Event: \n";

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
            m_methodNamesPosition = new Rect(Screen.width - 300, 50, 300, 21* amountOfEvents);
            return result;
        }
    }

    private void OnGUI()
    {
        GUI.Box(m_methodNamesPosition, AllMethodNames);

        GUI.Box(new Rect(Screen.width - 300, 0, 300, 42), methodName);

        int amountOfEvents = 0;
        foreach (var item in RandomEventTrigger.randomEvents)
        {
            if (GUI.Button(new Rect(Screen.width - 450, 50* amountOfEvents, 150, 50), item.Method.Name))
            {
                item();
            }
            amountOfEvents++;
        }
        
    }

    public static System.Action FunctionTriggered(System.Action function)
    {
        //Calls the function (so it actually happens)
        function();

        Debug.Log(function.Method.Name);

        methodName = "Current Event: \n" + function.Method.Name;

        return function;
    }
}
