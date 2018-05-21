using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DAS
{
    public class ToiletSystem : MonoBehaviour
    {
        #region Classes
        public class Toilet
        {
            public GameObject gameObject;
            public ToiletObject myObjects;
            public DAS.NPCMovement occupier;
            public List<DAS.NPCMovement> toiletQueue = new List<DAS.NPCMovement>();

            public bool broken = false;

            public Toilet(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }
            public bool IsOccupied
            {
                get
                {
                    if (occupier != null)
                        return true;
                    else
                        return false;
                }
            }
            public void RepairMe()
            {
                broken = false;
                myObjects.button.gameObject.SetActive(false);
                ArrowPointer.MyInstance.RemoveObjectToPointAt(gameObject);

                foreach (var particleSystem in gameObject.GetComponentsInChildren<ParticleSystem>())
                    particleSystem.Stop();
            }
        }

        [System.Serializable]
        public class ToiletObject
        {
            public GameObject parent;
            public GameObject queue;
            public GameObject button;
        }

        public class ClickableObject : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
        {
            public Toilet toiletRef;
            public int distanceToPlayer = 5;

            public void OnPointerDown(PointerEventData eventData)
            {
                toiletRef.myObjects.button.GetComponent<Button>().image.sprite = MoodIconHolder.MyInstance.iconSpriteRepairPressed;
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                if (!toiletRef.myObjects.button.GetComponent<Button>().interactable)
                    return;
                toiletRef.myObjects.button.GetComponent<Button>().image.sprite = MoodIconHolder.MyInstance.iconSpriteRepair;
                if (Vector3.Distance(toiletRef.gameObject.transform.position, DAS.PlayerMovement.s_myInstance.transform.position) < distanceToPlayer)
                {
                    toiletRef.RepairMe();
                }
            }

            public void OnPointerEnter(PointerEventData eventData)
            {
                if (!toiletRef.myObjects.button.GetComponent<Button>().interactable)
                    return;
                toiletRef.myObjects.button.GetComponent<Button>().image.sprite = MoodIconHolder.MyInstance.iconSpriteRepairHighlighted;
            }

            public void OnPointerExit(PointerEventData eventData)
            {
                if (!toiletRef.myObjects.button.GetComponent<Button>().interactable)
                    return;
                toiletRef.myObjects.button.GetComponent<Button>().image.sprite = MoodIconHolder.MyInstance.iconSpriteRepair;
            }
            private void Update()
            {
                //Check distance from player and disable button if player is too far away
                if (Vector3.Distance(toiletRef.gameObject.transform.position, DAS.PlayerMovement.s_myInstance.transform.position) > distanceToPlayer &&
                    toiletRef.myObjects.button.GetComponent<Button>().image.sprite != MoodIconHolder.MyInstance.iconSpriteRepairDisabled)
                {
                    toiletRef.myObjects.button.GetComponent<Button>().image.sprite = MoodIconHolder.MyInstance.iconSpriteRepairDisabled;
                    toiletRef.myObjects.button.GetComponent<Button>().interactable = false;
                }
                else if (Vector3.Distance(toiletRef.gameObject.transform.position, DAS.PlayerMovement.s_myInstance.transform.position) < distanceToPlayer)
                {
                    if (toiletRef.myObjects.button.GetComponent<Button>().image.sprite == MoodIconHolder.MyInstance.iconSpriteRepairDisabled)
                        toiletRef.myObjects.button.GetComponent<Button>().image.sprite = MoodIconHolder.MyInstance.iconSpriteRepair;
                        toiletRef.myObjects.button.GetComponent<Button>().interactable = true;
                }
            }
        }
        #endregion

        #region Variables
        public Canvas twoDCanvasRef;

        /// <summary>
        /// Singleton class variable.
        /// </summary>
        public static ToiletSystem s_myInstance;

        public ToiletObject toiletObject1, toiletObject2;

        /// <summary>
        /// List of all toilets.
        /// </summary>
        private static List<Toilet> s_toiletPoints = new List<Toilet>();
        /// <summary>
        /// Dictionary of toilets with NPC as key.
        /// </summary>
        private static Dictionary<DAS.NPCMovement, Toilet> s_npcAssignedToToilet = new Dictionary<DAS.NPCMovement, Toilet>();

        /// <summary>
        /// Look at the property NextToilet for more info.
        /// </summary>
        private int toiletTrack;
        #endregion

        #region Start/Awake
        private void Awake()
        {
            if (s_myInstance == null)
                s_myInstance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            InitToilets();

            s_toiletPoints[0].myObjects = toiletObject2;
            s_toiletPoints[1].myObjects = toiletObject1;

            toiletObject2.button.AddComponent<ClickableObject>().toiletRef = s_toiletPoints[0];
            toiletObject1.button.AddComponent<ClickableObject>().toiletRef = s_toiletPoints[1];
            toiletObject1.button.SetActive(false);
            toiletObject2.button.SetActive(false);

            foreach (var toilet in s_toiletPoints)
            {
                PositionToiletButtons(toilet.gameObject, toilet.myObjects.button);
            }
        }
        #endregion


        private void Update()
        {
            foreach (var toilet in s_toiletPoints)
                if(toilet.broken)
                    PositionToiletButtons(toilet.gameObject, toilet.myObjects.button);
        }


        #region Functions
        #region Toilet
        /// <summary>
        /// Find and add all toilets in the scene.
        /// </summary>
        public void InitToilets()
        {
            s_toiletPoints.Clear();
            if (s_toiletPoints.Count == 0)
                foreach (var go in GameObject.FindGameObjectsWithTag("Toilet"))
                {
                    s_toiletPoints.Add(new Toilet(go));
                }
        }

        /// <summary>
        /// Called in Update.
        /// Random Chance of NPC wanting to go to the toilet.
        /// </summary>
        public void RandomGotoToiletChance(DAS.NPCMovement dasNpcMovement)
        {
            if (Random.Range(0f, 1000f) > 999.3f && dasNpcMovement.IsCurrentlyWorking && dasNpcMovement.workTimeStreak >= 100)
            {
                bool allowedToilet = true;

                if (s_npcAssignedToToilet.ContainsKey(dasNpcMovement) || dasNpcMovement.isQueued)
                    allowedToilet = false;

                if (allowedToilet)
                {
                    dasNpcMovement.isQueued = true;
                    if (TryGotoToilet(dasNpcMovement)) { }
                }
            }
        }

        /// <summary>
        /// NPCs destination becomes one of the toilets in the scene.
        /// </summary>
        private bool TryGotoToilet(DAS.NPCMovement dasNpcMovement)
        {
            if (s_npcAssignedToToilet.ContainsKey(dasNpcMovement))
                Debug.LogWarning("Something went bad");

            AddToQueue(dasNpcMovement, NextToilet);
            dasNpcMovement.timeInsideDestination = 0;
            StartCoroutine(StandInLine(dasNpcMovement));
            return false;
        }

        /// <summary>
        /// NPCs destination becomes one of the toilets in the scene.
        /// </summary>
        public void GotoToilet(DAS.NPCMovement dasNpcMovement)
        {
            s_npcAssignedToToilet[dasNpcMovement].occupier = dasNpcMovement;

            dasNpcMovement.m_agentRef.destination = s_npcAssignedToToilet[dasNpcMovement].gameObject.transform.position;
            dasNpcMovement.isAtToilet = true;
            dasNpcMovement.timeInsideDestination = 0;
            dasNpcMovement.m_agentRef.isStopped = false;
        }

        /// <summary>
        /// Checks if NPC is within 0.2 units within its' assigned toilet.
        /// </summary>
        public bool IsInsideToilet(DAS.NPCMovement dasNpcMovement)
        {
            if (Vector3.Distance(dasNpcMovement.m_agentRef.destination, s_npcAssignedToToilet[dasNpcMovement].gameObject.transform.position) < 0.2f)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks all toilets and returns the first available one. If none is available this WILL RETURN NULL!!!
        /// </summary>
        private Toilet CheckUnoccupiedToilet()
        {
            foreach (var toilet in s_toiletPoints)
            {
                if (toilet.IsOccupied == false)
                {
                    return toilet;
                }
            }
            return null;
        }

        /// <summary>
        /// Coroutine function for standing in line to the toilet.
        /// </summary>
        private IEnumerator StandInLine(DAS.NPCMovement dasNpcMovement)
        {
            dasNpcMovement.m_agentRef.isStopped = false;
            do
            {
                yield return new WaitForSeconds(0.2f);

                do
                {
                    yield return new WaitForSeconds(0.2f);

                    //1. if not first in queue and a queue exists.
                    if (s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement) > 0 && s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Count > 0)
                        //2. Which side of the toilets?
                        if (s_npcAssignedToToilet[dasNpcMovement] == s_toiletPoints[0])
                            dasNpcMovement.m_agentRef.destination = toiletObject1.queue.transform.position +
                                toiletObject1.queue.transform.forward +
                                (-toiletObject1.queue.transform.forward * s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement));
                        else
                            dasNpcMovement.m_agentRef.destination = toiletObject2.queue.transform.position +
                                toiletObject2.queue.transform.forward +
                                (-toiletObject2.queue.transform.forward * s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement));
                    else //1. else if not first in queue and a queue exists.
                        //2. Which side of the toilets?
                        if (s_npcAssignedToToilet[dasNpcMovement] == s_toiletPoints[0])
                            dasNpcMovement.m_agentRef.destination = toiletObject1.queue.transform.position + toiletObject1.queue.transform.forward;
                        else
                            dasNpcMovement.m_agentRef.destination = toiletObject2.queue.transform.position + toiletObject2.queue.transform.forward;

                } while (s_npcAssignedToToilet[dasNpcMovement].toiletQueue[0] != dasNpcMovement);
            } while (s_npcAssignedToToilet[dasNpcMovement].broken);
            GotoToilet(dasNpcMovement);
        }

        /// <summary>
        /// Adds an NPC to the toilet queue.
        /// </summary>
        private void AddToQueue(DAS.NPCMovement dasNpcMovement, Toilet toiletToQueue)
        {
            //Debug.Log(dasNpcMovement.name + " is being added to queue");
            s_npcAssignedToToilet.Add(dasNpcMovement, toiletToQueue);
            s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Add(dasNpcMovement);
        }

        /// <summary>
        /// Removes NPC from the queue to the toilet.
        /// </summary>
        public void RemoveFromQueue(DAS.NPCMovement dasNpcMovement)
        {
            //Debug.Log(dasNpcMovement.name + " is being removed from queue");
            s_npcAssignedToToilet[dasNpcMovement].occupier = null;
            s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Remove(dasNpcMovement);
            s_npcAssignedToToilet.Remove(dasNpcMovement);
            dasNpcMovement.isQueued = false;
        }
        #endregion

        private void PositionToiletButtons(GameObject worldPositionObject, GameObject button)
        {
            Vector2 twoDPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                twoDCanvasRef.transform as RectTransform,
                Camera.main.WorldToScreenPoint(worldPositionObject.transform.position),
                twoDCanvasRef.worldCamera,
                out twoDPos
            );

            twoDPos += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset.;

            button.transform.position = twoDPos;
        }

        #region Event
        /// <summary>
        /// Makes all npcs go to the toilet.
        /// </summary>
        public void EveryoneNeedsToiletEvent()
        {
            foreach (var npc in NPC.s_npcList)
            {
                if (!s_npcAssignedToToilet.ContainsKey(npc.moveRef))
                    TryGotoToilet(npc.moveRef);
            }
        }

        /// <summary>
        /// Breaks all toilets rendering them unusuable until fixed.
        /// </summary>
        public void ToiletBreakEvent()
        {
            foreach (var toilet in s_toiletPoints)
            {
                toilet.broken = true;
                toilet.myObjects.button.SetActive(true);

                ArrowPointer.ObjectPointer data = new ArrowPointer.ObjectPointer
                {
                    colorOfArrow = Color.blue,
                    colorOfText = Color.blue,
                    extraSprite = MoodIconHolder.MyInstance.iconSpriteRepairNoBackground,
                    extraFunction = true
                };
                ArrowPointer.MyInstance.AddObjectToPointAt(toilet.gameObject, data);

                foreach (var particleSystem in toilet.gameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particleSystem.Play();
                }
            }
        }

        /// <summary>
        /// Fixes all toilets, making them not broken.
        /// </summary>
        public void FixAllToilets()
        {
            foreach (var toilet in s_toiletPoints)
            {
                toilet.broken = false;
                toilet.myObjects.button.SetActive(false);
                ArrowPointer.MyInstance.RemoveObjectToPointAt(toilet.gameObject);

                foreach (var particleSystem in toilet.gameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particleSystem.Stop();
                }
            }
        }
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Used to even out the toilet uses between all toilets.
        /// </summary>
        private Toilet NextToilet
        {
            get
            {
                toiletTrack++;
                if (toiletTrack >= s_toiletPoints.Count)
                    toiletTrack = 0;
                return s_toiletPoints[toiletTrack];
            }
        }
        #endregion
    }
}