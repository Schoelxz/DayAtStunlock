using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class BananaEvent : MonoBehaviour
{
    public GameObject bananaPrefab;

    public Rect3D[] spawnAreas;

    public int bananaAmount = 3;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow/1.2f;
        foreach (var area in spawnAreas)
            Gizmos.DrawCube(area.positon, area.size);
    }

    public void StartBananaEvent()
    {

        foreach (var area in spawnAreas)
            for (int i = 0; i < bananaAmount; i++)
            {
                GameObject banana = Instantiate(bananaPrefab);
                banana.AddComponent<Banana>();
                banana.transform.position = area.RandomPointInBox();
            }
    }
}

public class Banana : MonoBehaviour
{

}

[System.Serializable]
public class Rect3D
{
    public Vector3 size;
    public Vector3 positon;
    public Rect3D(Vector3 pSize, Vector3 pPosition)
    {
        size = pSize;
        positon = pPosition;
    }

    public Vector3 RandomPointInBox()
    {
        return positon + new Vector3(
           (Random.value - 0.5f) * size.x,
           (Random.value - 0.5f) * size.y,
           (Random.value - 0.5f) * size.z
        );
    }
}