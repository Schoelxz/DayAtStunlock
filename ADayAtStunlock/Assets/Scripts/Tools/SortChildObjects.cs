﻿#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SortChildObjects : EditorWindow
{
    [MenuItem("GameObject/Sort By Name", false, -1)]
    public static void SortGameObjectsByName(MenuCommand menuCommand)
    {
        if (menuCommand.context == null || menuCommand.context.GetType() != typeof(GameObject))
        {
            EditorUtility.DisplayDialog("Error", "You must select an item to sort in the frame", "Okay");
            return;
        }

        GameObject parentObject = (GameObject)menuCommand.context;

        if (parentObject.GetComponentInChildren<Image>())
        {
            EditorUtility.DisplayDialog("Error", "You are trying to sort a GUI element. This will screw up EVERYTHING, do not do", "Okay");
            return;
        }

        // Build a list of all the Transforms in this player's hierarchy
        Transform[] objectTransforms = new Transform[parentObject.transform.childCount];
        for (int i = 0; i < objectTransforms.Length; i++)
            objectTransforms[i] = parentObject.transform.GetChild(i);

        int sortTime = System.Environment.TickCount;

        bool sorted = false;
        // Perform a bubble sort on the objects
        while (sorted == false)
        {
            sorted = true;
            for (int i = 0; i < objectTransforms.Length - 1; i++)
            {
                // Compare the two strings to see which is sooner
                int comparison = objectTransforms[i].name.CompareTo(objectTransforms[i + 1].name);

                if (comparison > 0) // 1 means that the current value is larger than the last value
                {
                    objectTransforms[i].transform.SetSiblingIndex(objectTransforms[i + 1].GetSiblingIndex());
                    sorted = false;
                }
            }

            // resort the list to get the new layout
            for (int i = 0; i < objectTransforms.Length; i++)
                objectTransforms[i] = parentObject.transform.GetChild(i);
        }

        Debug.Log("Sort took " + (System.Environment.TickCount - sortTime) + " milliseconds");

    }
}

#endif