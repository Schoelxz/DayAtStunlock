using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Path", menuName = "Waypoint/Path", order = 1)]
public class PathSO : ScriptableObject
{
    public List<string> pathWay;
}
