using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DAS
{
    /// <summary>
    /// Standard NPC class, contains general NPC stuff.
    /// </summary>
    public class NPC : MonoBehaviour
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
            /// <summary>
            /// Total of motivation and happiness (value will be between 0 and 2)
            /// </summary>
            public float TotalFeelings
            {
                get { return motivation+happiness; }
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

        public static float s_happyAverage, s_motivationAverage;

        private float myHappinessDecay, myMotivationDecay;

        private Slider happySlider, motivationSlider;

        public Slider HappySlider
        {
            get { return happySlider; }
        }
        public Slider MotivationSlider
        {
            get { return motivationSlider; }
        }

        public new string name;

        public WorkSeat myWorkSeat;
        public DAS.NPCMovement moveRef;
        public NpcButtons buttonRef;
        public ButtonToggler buttonTogglerRef;
        private Material[] myMaterials;
        private Material moneyMaterial;
        private ModelChanger modelChanger;

        public MoodVisualizer moodVisualizerRef;
        public Feelings myFeelings;
        private GameObject nameHolder;
        private TextMesh myNameDisplay;

        float dt;

       
        #endregion

        public Material[] MyMaterials
        {
            get { return myMaterials; }
        }

        private void Start()
        {
            /// Add ourself to list
            s_npcList.Add(this);

            /// Assign Values
            myFeelings = new Feelings(1.0f, 1.0f);

            Random.State oldState = Random.state;
            Random.InitState(name.Length);
            myHappinessDecay = Random.Range(150, 250);
            myMotivationDecay = Random.Range(140, 240);
            Debug.Log(name.Length + ": mhd->" + myHappinessDecay + " mmd->" + myMotivationDecay);
            Random.state = oldState;

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
            modelChanger = gameObject.AddComponent<ModelChanger>();
            buttonRef = gameObject.AddComponent<NpcButtons>();
            buttonRef.InitNpcButtons();
            buttonTogglerRef = gameObject.AddComponent<ButtonToggler>();
            buttonTogglerRef.InitButtonToggler();

            nameHolder = new GameObject("Name Holder");
            nameHolder.transform.parent = gameObject.transform;
            myNameDisplay = nameHolder.AddComponent<TextMesh>();

            /// Text
            if (myNameDisplay != null)
            {
                myNameDisplay.text = name;
                myNameDisplay.alignment = TextAlignment.Center;
                myNameDisplay.anchor = TextAnchor.MiddleCenter;
                myNameDisplay.transform.position = transform.position;
                myNameDisplay.transform.position += new Vector3(0, 3, 0);
                myNameDisplay.transform.forward = Camera.main.transform.forward;
                myNameDisplay.transform.Rotate(0, 180, 0);
                myNameDisplay.characterSize = 0.03f;
                myNameDisplay.fontSize = 155;
            }

            /// Material
            myMaterials = GetComponentInChildren<MeshRenderer>().materials;
            moneyMaterial = new Material(myMaterials[0]);
            moneyMaterial.color = Color.green;
        }

        private void OnDestroy()
        {
            WorkSeatManager.myInstance.workSeats.Remove(this.myWorkSeat);
            s_npcList.Remove(this);
        }

        private void Update()
        {
            // Feelings depletion;
            // example explaination: feeling -= (delta time / amount of seconds until 0)

            if(modelChanger.isAlien)
            {
                myFeelings.Happiness = 1;
                myFeelings.Motivation = 1;
            }
            else
            {
                myFeelings.Happiness -= Mathf.Clamp01(DAS.TimeSystem.DeltaTime / myHappinessDecay);
                myFeelings.Motivation -= Mathf.Clamp01(DAS.TimeSystem.DeltaTime / myMotivationDecay);
            }
            happySlider.value      = Mathf.Clamp01(myFeelings.Happiness);
            motivationSlider.value = Mathf.Clamp01(myFeelings.Motivation);

            
        }
        private void FixedUpdate()
        {
            myNameDisplay.transform.forward = Camera.main.transform.forward;
        }

        #region Functions

       /* private void SetDefaultMaterial()
        {
            //GetComponentInChildren<MeshRenderer>().material = myMaterial;
        }*/
        #endregion
    };

    public class NpcCreator : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private GameObject NPCPrefab;

        private GameObject npcFolder;

        // Locations the NPCs can spawn at when created.
        public Transform[] spawnLocations = new Transform[2];
        // Wether to show/use GUI or not.
        public bool toggleGUI = true;

        // Keeps track of NPCs.
        List<GameObject> npcList = new List<GameObject>();

        [Range(0, 41)]
        [SerializeField]
        private int maxAllowedNpcs = 41;
        public int MaxAllowedNpcs
        {
            get { return maxAllowedNpcs; }
            set { maxAllowedNpcs = Mathf.Clamp(value, 0, MaxNumberOfNPCsByWorkseatAmount); }
        }

        static int numOfWorkSeats;
        /// <summary>
        /// Returns an integer.
        /// <para>Number is set at Start() of NpcCreator.</para>
        /// <para>Is also number of work seats.</para>
        /// </summary>
        public static int MaxNumberOfNPCsByWorkseatAmount
        {
            get { return numOfWorkSeats; }
        }

        [Range(0f, 45f)]
        [SerializeField]
        private float secondsTilNpcSpawn = 1;

        [Range(0, 45)]
        [SerializeField]
        private float numOfNPCs;
        private int NumOfNPCs
        {
            get { return (int)Mathf.Clamp(numOfNPCs, 0, numOfWorkSeats); }
        }

        string[] names = new string[45];
        void AssignNamesToArray()
        {
            names[0] = "Peter";//ll     //product owner, game director
            names[1] = "Filip";//mr     //Ekonomichef
            names[2] = "Martin L";//bs    //creative director
            names[3] = "Max T";//ml     //Producer
            names[4] = "Philip";//ml    //lead programmer
            names[5] = "Pierre";//bs    //HR
            names[6] = "Rickard";//rrr   //VD
            names[7] = "Srdan";//bs //lead artist
            names[8] = "Tau";//ll       //PR

            //Art team
            names[9] = "Patrik";//bs
            names[10] = "Gabriel";//bs
            names[11] = "Oskar";//bl
            names[12] = "Mattias";//bl
            names[13] = "Martin";//bl
            names[14] = "Max";//bl
            names[15] = "Tara";//bl
            names[16] = "Fanny";//bl
            names[17] = "Sofia";//bl
            names[18] = "Andreas";//bl
            names[19] = "Viktor";//bl
            names[20] = "Johan A";//bl
            names[21] = "Johan W";//rr

            //UI
            names[22] = "Karl";//rr
            names[23] = "Katey";//rr
            names[24] = "Daniel";//rr
            names[25] = "Arvid";//rr

            //Design
            names[26] = "Simon";//rr
            names[27] = "Konrad";//rr
            names[28] = "Christian";//rr
            names[29] = "Erik";//rr
            names[30] = "Emil";//rr

            //Programmers
            names[31] = "Fredrik";//ml
            names[32] = "Jonas";//ml

            //Temps
            names[33] = "Christoffer";//bs
            names[34] = "Tobias";//bs
            names[35] = "Jimmy";//bs

            //Community
            names[36] = "Johan";//mr
            names[37] = "Liz";//mr
            names[38] = "Christopher";//mr
            names[39] = "Lisabeth";//ll
            names[40] = "Ruth";//ll


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
            npcFolder = GameObject.Find("NPC Folder");
        }

#if UNITY_EDITOR
        
        private void OnGUI()
        {
            if (!toggleGUI)
                return;
            // A Slider for controlling the number of NPCs
            numOfNPCs = GUI.VerticalSlider(new Rect(25, 25, 100, 100), NumOfNPCs, numOfWorkSeats, 0);
            // Shows the amount of NPCs
            GUI.Box(new Rect(35, 10, 25, 25), numOfNPCs.ToString("00"));

            secondsTilNpcSpawn = GUI.VerticalSlider(new Rect(25, 140, 100, 100), secondsTilNpcSpawn, 30, 0);
            // Shows the amount of NPCs
            GUI.Box(new Rect(35, 130, 35, 25), secondsTilNpcSpawn.ToString("00.0"));
        }
#endif

        void Update()
        {
            numOfNPCs = NumOfNPCs;

            if(NumOfNPCs < MaxAllowedNpcs)
                NpcCreationPerXSeconds(secondsTilNpcSpawn);

            //+++ Reduce update calls
            dt += Time.deltaTime;
            if (dt >= 0.05f)
            { dt = 0; }
            else
                return;
            //---
            
            // Controls NPC amount
            NpcAmountController();

            UpdateAverageValues();
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
            NPC tempNPC;
            if (NPCPrefab == null)
                Debug.Assert(NPCPrefab);
            else
                npcList.Add(Instantiate(NPCPrefab));
            npcList[npcList.Count - 1].name = names[npcList.Count - 1]; //+ npcList.Count;
            tempNPC = npcList[npcList.Count - 1].AddComponent<NPC>();
            tempNPC.name = npcList[npcList.Count - 1].gameObject.name;
            tempNPC.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
            tempNPC.myWorkSeat = new WorkSeat(WorkSeatManager.myInstance.gameobjectSeats[tempNPC.name].gameObject, tempNPC);
            tempNPC.gameObject.transform.parent = npcFolder.transform;
        }

        /// <summary>
        /// Sets average values of all NPCs feelings.
        /// </summary>
        void UpdateAverageValues()
        {
            float hA = 0;
            float mA = 0;

            foreach (var npc in DAS.NPC.s_npcList)
            {
                hA += npc.myFeelings.Happiness;
                mA += npc.myFeelings.Motivation;
            }

            NPC.s_happyAverage = hA / DAS.NPC.s_npcList.Count;
            NPC.s_motivationAverage = mA / DAS.NPC.s_npcList.Count;
        }

        #endregion
    }
}