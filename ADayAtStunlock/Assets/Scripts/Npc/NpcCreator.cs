using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class NPC : MonoBehaviour
{
    public string name;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
};

public class NpcCreator : MonoBehaviour
{
    List<GameObject> npcList = new List<GameObject>();
    string[] names = new string[45];
    public Vector3Int[] spawnLocations = new Vector3Int[2];

    [Range(0, 45)]
    [SerializeField]
    private float numOfNPCs;
    private float maxNPCs = 45; //45 could be a "number of workseats" variable instead, maybe from another script?

    float dt;

    private void OnGUI()
    {
        numOfNPCs = GUI.VerticalSlider(new Rect(25, 25, 100, 100), numOfNPCs, maxNPCs, 0);
        numOfNPCs = (int)numOfNPCs;
        GUI.Box(new Rect(35, 10, 25, 25), numOfNPCs.ToString());
    }

    // Use this for initialization
    void Start ()
    {
        // Set the names for the stunlockers in an array.
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = "Stunlocker " + (i+1).ToString();
        }	
	}
	
	// Update is called once per frame
	void Update ()
    {
        numOfNPCs = (int)numOfNPCs;
        dt += Time.deltaTime;
        if(dt >= 0.05)
        {
            dt = 0;
            return;
        }
        

		if(npcList.Count != numOfNPCs)
        {
            if(npcList.Count < numOfNPCs)
            {
                for (int i = npcList.Count; i < numOfNPCs; i++)
                {
                    npcList.Add(new GameObject(names[npcList.Count]));
                    npcList[npcList.Count - 1].AddComponent<NPC>().name = npcList[npcList.Count - 1].gameObject.name;
                    npcList[npcList.Count - 1].transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)];
                }
            }
            else if(npcList.Count > numOfNPCs)
            {
                while(npcList.Count > numOfNPCs)
                { 
                    Destroy(npcList[npcList.Count - 1]);
                    npcList.RemoveAt(npcList.Count - 1);
                }
            }
            
        }
	}
}
