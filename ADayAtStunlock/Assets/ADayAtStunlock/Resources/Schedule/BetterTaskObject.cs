using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Schedule", menuName = "Schedule/Better Task", order = 1)]
public class BetterTaskObject : ScriptableObject
{
    public Vector2[] TaskTime;
    public string[] TaskName;
}
