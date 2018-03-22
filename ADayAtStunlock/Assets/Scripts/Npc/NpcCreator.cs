using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DAS
{
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

        #region Structs
        public struct Feelings
        {
            private float happiness;
            private float motivation;

            public float Happiness
            {
                get { return happiness; }
                set { happiness = Mathf.Clamp01(value); }
            }
            public float Motivation
            {
                get { return motivation; }
                set { motivation = Mathf.Clamp01(value); }
            }

            public Feelings(float happiness, float motivation)
            {
                this.happiness = Mathf.Clamp01(happiness);
                this.motivation = Mathf.Clamp01(motivation);
            }
        }
        #endregion

        #region Variables
        public static List<NPC> s_npcList = new List<NPC>();

        private Slider happySlider, motivationSlider;

        public new string name;

        public DAS.NPCMovement moveRef;
        private Material myMaterial;
        private Material moneyMaterial;

        public Feelings myFeelings;
        private GameObject nameHolder;
        private TextMesh myNameDisplay;

        float dt;
        #endregion

        private void Start()
        {
            /// Add ourself to list
            s_npcList.Add(this);

            /// Assign Values
            myFeelings = new Feelings(1, 1);

            /// Get Components
            foreach (var item in GetComponentsInChildren<Slider>())
            {
                if (item.gameObject.name == "Happiness Slider")
                    happySlider = item;
                else if (item.gameObject.name == "Motivation Slider")
                    motivationSlider = item; 
            }

            /// Add Components
            moveRef = gameObject.AddComponent<DAS.NPCMovement>();
            gameObject.AddComponent<NpcButtons>();
            gameObject.AddComponent<ButtonToggler>();
            nameHolder = new GameObject("Name Holder");
            nameHolder.transform.parent = gameObject.transform.GetChild(0);
            myNameDisplay = nameHolder.AddComponent<TextMesh>();

            /// Text
            if (myNameDisplay != null)
            {
                myNameDisplay.text = name;
                myNameDisplay.alignment = TextAlignment.Center;
                myNameDisplay.anchor = TextAnchor.MiddleCenter;
                myNameDisplay.transform.position = transform.position;
                myNameDisplay.transform.position += new Vector3(0, 3, 0);
                myNameDisplay.transform.Rotate(0, 180, 0);
                myNameDisplay.characterSize = 0.03f;
                myNameDisplay.fontSize = 355;
            }

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
            //+++ Reduce update calls
            dt += Time.deltaTime;
            if (dt >= 0.01f)
            { dt = 0; }
            else
                return;
            //---

            myFeelings.Happiness  -= Mathf.Clamp01(DAS.TimeSystem.DeltaTime / 50);
            myFeelings.Motivation -= Mathf.Clamp01(DAS.TimeSystem.DeltaTime / 10);

            happySlider.value      = Mathf.Clamp01(myFeelings.Happiness);
            motivationSlider.value = Mathf.Clamp01(myFeelings.Motivation);
        }

        #region Functions
        /*public void GenerateMoney()
        {
            GetComponentInChildren<MeshRenderer>().material = moneyMaterial;
            MoneyManager.currentMoney += (happySlider.value + motivationSlider.value);
            Invoke("SetDefaultMaterial", 1);
        }*/

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

        int numOfWorkSeats;
        [Range(0, 45)] [SerializeField] private float numOfNPCs;
        private int NumOfNPCs
        {
            get { return (int)Mathf.Clamp(numOfNPCs, 0, numOfWorkSeats); }
        }

        string[] names = new string[45];
        void AssignNamesToArray()
        {
            names[0] = "Peter";
            names[1] = "Filip";
            names[2] = "Martin";
            names[3] = "Max";
            names[4] = "Philip";
            names[5] = "Pierre";
            names[6] = "Rickard";
            names[7] = "Srdan";
            names[8] = "Tau";

            //Art team
            names[9] = "Patrik";
            names[10] = "Gabriel";
            names[11] = "Oskar";
            names[12] = "Mattias";
            names[13] = "Martin";
            names[14] = "Max";
            names[15] = "Tara";
            names[16] = "Fanny";
            names[17] = "Sofia";
            names[18] = "Andreas";
            names[19] = "Viktor";
            names[20] = "Johan A";
            names[21] = "Johan W";

            //UI
            names[22] = "Karl";
            names[23] = "Katey";
            names[24] = "Daniel";
            names[25] = "Arvid";

            //Design
            names[26] = "Simon";
            names[27] = "Konrad";
            names[28] = "Christian";
            names[29] = "Erik";
            names[30] = "Emil";

            //Programmers
            names[31] = "Fredrik";
            names[32] = "Jonas";

            //Temps
            names[33] = "Christoffer";
            names[34] = "Tobias";
            names[35] = "Jimmy";

            //Community
            names[36] = "Johan";
            names[37] = "Liz";
            names[38] = "Christopher";
            names[39] = "Lisabeth";
            names[40] = "Ruth";


            for (int i = 41; i < names.Length; i++)
            {
                names[i] = "Bob";
            }
        }

        // Delta Timers
        float dt, dt2;
        #endregion

        private void Start()
        {
            AssignNamesToArray();
            numOfWorkSeats = GameObject.FindGameObjectsWithTag("WorkSeat").Length;
        }

        private void OnGUI()
        {
            if (!toggleGUI)
                return;
            // A Slider for controlling the number of NPCs
            numOfNPCs = GUI.VerticalSlider(new Rect(25, 25, 100, 100), NumOfNPCs, numOfWorkSeats, 0);
            // Shows the amount of NPCs
            GUI.Box(new Rect(35, 10, 25, 25), numOfNPCs.ToString());
        }

        /*
         * ThoughtBubble:
         * Use invoke instead of tracking delta times?
         */
        void Update()
        {
            numOfNPCs = NumOfNPCs;

            NpcCreationPerXSeconds(2);

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
        /// Create new NPCs at the rate of game speed seconds.
        /// </summary>
        /// <param name="seconds">How often the function will run its' program (e.g. value 3 would call everything every third second).</param>
        void NpcCreationPerXSeconds(float seconds)
        {
            //+++ Reduce update calls
            dt2 += DAS.TimeSystem.DeltaTime;
            if (dt2 >= seconds)
            { dt2 = 0; }
            else
                return;
            //---

            // Adds 1 NPC while under NPC max limit
            if (npcList.Count < numOfWorkSeats)
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

        /// <summary>
        /// Creates a new NPC when called.
        /// </summary>
        void AddNewNPC()
        {
            if (NPCPrefab == null)
                Debug.Assert(NPCPrefab);
            else
                npcList.Add(Instantiate(NPCPrefab));
            npcList[npcList.Count - 1].name = names[npcList.Count - 1]; //+ npcList.Count;
            npcList[npcList.Count - 1].AddComponent<NPC>().name = npcList[npcList.Count - 1].gameObject.name;
            npcList[npcList.Count - 1].transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
        }

        #endregion
    }

}