using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Standard NPC class, contains general NPC stuff.
/// </summary>
class NPC : MonoBehaviour
{
    #region EditorStuff
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
#endif
    #endregion

    #region Variables
    public static List<NPC> s_npcList = new List<NPC>();

    public new string name;

    public DAS.NPCMovement moveRef;
    private Material myMaterial;
    private Material moneyMaterial;

    float dt;
    #endregion

    private void Start()
    {
        /// Add ourself to list
        s_npcList.Add(this);

        /// Add Components
        moveRef = gameObject.AddComponent<DAS.NPCMovement>();

        /// Material
        myMaterial = GetComponentInChildren<MeshRenderer>().material;
        moneyMaterial = new Material(myMaterial);
        moneyMaterial.color = Color.green;
    }

    private void OnDestroy()
    {
        s_npcList.Remove(this);
    }

    private void Update()
    {
        //happy
    }

    #region Functions
    public void GenerateMoney()
    {
        GetComponentInChildren<MeshRenderer>().material = moneyMaterial;
        MoneyManager.currentMoney++;
        Invoke("SetDefaultMaterial", 1);
    }

    private void SetDefaultMaterial()
    {
        GetComponentInChildren<MeshRenderer>().material = myMaterial;
    }
    #endregion
};

public class NpcCreator : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject NPCPrefab;

    // Locations the NPCs can spawn at when created.
    public Transform[] spawnLocations = new Transform[2];
    // Wether to show/use GUI or not.
    public bool toggleGUI = true;

    // Keeps track of NPCs.
    List<GameObject> npcList = new List<GameObject>();

    // Max amount of NPCs
    private static int NPCMAX = 42;
    [Range(0, 42)][SerializeField]private float numOfNPCs;
    private float maxNPCs = NPCMAX;

    // Delta Timers
    float dt;
    float dt2;
    #endregion

    private void OnGUI()
    {
        if (!toggleGUI)
            return;
        // A Slider for controlling the number of NPCs
        numOfNPCs = GUI.VerticalSlider(new Rect(25, 25, 100, 100), numOfNPCs, maxNPCs, 0);
        numOfNPCs = (int)numOfNPCs;
        // Shows the amount of NPCs
        GUI.Box(new Rect(35, 10, 25, 25), numOfNPCs.ToString());
    }

    /*
     * ThoughtBubble:
     * Use invoke instead of update gates?
     * Use invoke instead of tracking delta times?
     */
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

        // Controls NPC amount
        NpcAmountController();
	}

    #region Functions

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

        // Adds 1 NPC while under NPC max limit
        if (npcList.Count < NPCMAX)
        {
            numOfNPCs++;
            AddNewNPC();
        }
    }

    /// <summary>
    /// Adds or removes npcs depending on variable "numOfNPCs"
    /// </summary>
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
            Debug.Assert(NPCPrefab);
        else
            npcList.Add(Instantiate(NPCPrefab));
        npcList[npcList.Count - 1].name = "Stunlocker " + npcList.Count;
        npcList[npcList.Count - 1].AddComponent<NPC>().name = npcList[npcList.Count - 1].gameObject.name;
        npcList[npcList.Count - 1].transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
    }
    #endregion
}
