using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BananaEvent : MonoBehaviour
{
    public static BananaEvent myInstance;

    public GameObject bananaPrefab;

    public Rect3D[] spawnAreas;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow/1.2f;
        foreach (var area in spawnAreas)
            Gizmos.DrawCube(area.positon, area.size);
    }

    private void Awake()
    {
        myInstance = this;
    }

    public void StartBananaEvent()
    {
        StartCoroutine(SpawnBananas());
    }

    private IEnumerator SpawnBananas()
    {
        //Spawn bananas
        foreach (var area in spawnAreas)
            for (int i = 0; i < (area.size.x + area.size.y + area.size.z)/2; i++)
            {
                yield return new WaitForEndOfFrame();
                GameObject banana = Instantiate(bananaPrefab);
                banana.AddComponent<Banana>();
                banana.transform.position = area.RandomPointInBox;
                banana.transform.Rotate(new Vector3(0, 0, Random.Range(0, 1000)));
            }
    }
}

public class Banana : MonoBehaviour
{
    float lifeTime = 25;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<DAS.PlayerMovement>().OnBananaHit();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(gameObject);
    }
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

    /// <summary>
    /// ...except y coords.
    /// </summary>
    public Vector3 RandomPointInBox
    {
        get
        {
            return positon + new Vector3(
               (Random.value - 0.5f) * size.x,
               0,
               (Random.value - 0.5f) * size.z
            );
        }
    }
}