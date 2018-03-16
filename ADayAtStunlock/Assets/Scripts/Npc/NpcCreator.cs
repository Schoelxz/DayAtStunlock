using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class NPC : MonoBehaviour
{
    public new string name;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
};

public class NpcCreator : MonoBehaviour
{
    private static int NPCMAX = 42;
    [SerializeField]
    private GameObject NPCPrefab;

    List<GameObject> npcList = new List<GameObject>();
    string[] names = new string[45];
    public Transform[] spawnLocations = new Transform[2];
    public bool toggleGUI = true;

    [Range(0, 42)]
    [SerializeField]
    private float numOfNPCs;
    private float maxNPCs = NPCMAX;
    
    // Delta Timers
    float dt;
    float dt2;

    private void OnGUI()
    {
        if (!toggleGUI)
            return;
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
    void Update()
    {
        numOfNPCs = (int)numOfNPCs;

        UpdateGate(2);

        //+++ Reduce update calls
        dt += Time.deltaTime;
        if (dt >= 0.05f)
        { dt = 0; }
        else
            return;
        //---

        NpcAmountController();
	}

    /// <summary>
    /// A seperate update from update..
    /// </summary>
    /// <param name="seconds">How often the update will run its' program (e.g. value 3 would call everything every third second).</param>
    void UpdateGate(float seconds)
    {
        //+++ Reduce update calls
        dt2 += Time.deltaTime;
        if (dt2 >= seconds)
        { dt2 = 0; }
        else
            return;
        //---

        if (npcList.Count < NPCMAX)
        {
            numOfNPCs++;
            AddNewNPC();
        }
    }

    void NpcAmountController()
    {
        if (npcList.Count != numOfNPCs)
        {
            if (npcList.Count < numOfNPCs)
            {
                for (int i = npcList.Count; i < numOfNPCs; i++)
                {
                    AddNewNPC();
                }
            }
            else if (npcList.Count > numOfNPCs)
            {
                while (npcList.Count > numOfNPCs)
                {
                    Destroy(npcList[npcList.Count - 1]);
                    npcList.RemoveAt(npcList.Count - 1);
                }
            }
        }
    }

    void AddNewNPC()
    {
        if (NPCPrefab == null)
            npcList.Add(new GameObject(names[npcList.Count]));
        else
            npcList.Add(Instantiate(NPCPrefab));
        //npcList[npcList.Count - 1].AddComponent<NPC>().name = npcList[npcList.Count - 1].gameObject.name;
        npcList[npcList.Count - 1].transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
    }
}
