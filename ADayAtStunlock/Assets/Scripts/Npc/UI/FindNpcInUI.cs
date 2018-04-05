using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DAS
{
    public class FindNpcInUI : MonoBehaviour
    {
        class Icon
        {
            private GameObject myGameObject;
            private Vector2 myVector2;
            public DAS.NPC myNpcRef;

            public Icon(GameObject go)
            {
                myGameObject = go;
            }

            public GameObject MyGameObject
            {
                get
                {
                    return myGameObject;
                }
                set
                {
                    myGameObject = value;
                }
            }
            public Vector2 MyVector2
            {
                get
                {
                    return myVector2;
                }
                set
                {
                    myVector2 = value;
                }
            }

            public void AddToMyVector2(Vector2 addV2)
            {
                MyVector2 += addV2;
            }
            public void ReplaceMyVector2(Vector2 newV2)
            {
                MyVector2 = newV2;
            }

            private bool IsWithinScreenBoundaries
            {
                get
                {
                    if (
                            myVector2.x > Screen.width / (FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance)
                        && myVector2.x < Screen.width - (Screen.width / (FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance))
                        && myVector2.y > Screen.height / (FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance)
                        && myVector2.y < Screen.height - (Screen.height / (FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance))
                        )
                        return true;
                    else
                        return false;
                }
            }
            private bool IsOutsideScreenBoundaries
            {
                get
                {
                    if (IsWithinScreenBoundaries)
                        return false;

                    if (
                         ((
                            myVector2.x < Screen.width / (FindNpcInUI.myInstance.screenClampMultiplier * 2)
                        || myVector2.x > Screen.width - (Screen.width / (FindNpcInUI.myInstance.screenClampMultiplier * 2))
                          )
                        && (
                            myVector2.y < Screen.height / (FindNpcInUI.myInstance.screenClampMultiplier * 2)
                        || myVector2.y > Screen.height - (Screen.height / (FindNpcInUI.myInstance.screenClampMultiplier * 2))
                          ))
                       )
                        return true;
                    else
                        return false;
                }
            }
        }

        //Singleton behaviour class.
        public static FindNpcInUI myInstance;

        public Sprite imageToDisplay;

        [Header("Artist Variables")]
        [Range(1f, 5f)]
        [SerializeField]
        private float screenClampMultiplier = 1.1f;
        [Range(1, 300)]
        [SerializeField]
        private int distanceTilArrowPlaced = 1;
        //[Range(0, 150)]
        //[SerializeField]
        private int screenInsideBoundaryAllowance = 10;
        [SerializeField]
        private bool showArrowWhenClose = true;

        private Canvas m_myCanvas;
        private List<Icon> m_icons = new List<Icon>();
        private Vector3 middlePoint;

        void Start()
        {
            //Singleton behaviour;
            if (myInstance == null)
                myInstance = this;
            else
                Destroy(this.gameObject);

            m_myCanvas = GetComponent<Canvas>();

            // Add an icon arrow spot for all npcs, starting at the max amount of npcs
            for (int i = 0; i < DAS.NpcCreator.MaxNumberOfNPCs; i++)
            {
                m_icons.Add(new Icon(new GameObject("NPC Icon " + i)));
                m_icons[i].MyGameObject.transform.parent = transform;
                m_icons[i].MyGameObject.AddComponent<Image>().sprite = imageToDisplay;
                m_icons[i].MyGameObject.GetComponent<Image>().rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                m_icons[i].MyGameObject.GetComponent<Image>().color = new Color(0.5f, 0, 0.5f, 1);
            }
        }

        void Update()
        {
            // Point in the middle of the screen
            middlePoint = new Vector3(Screen.width / 2, Screen.height / 2);

            foreach (var npc in DAS.NPC.s_npcList)
            {
                if (m_icons[DAS.NPC.s_npcList.IndexOf(npc)].myNpcRef == null)
                    m_icons[DAS.NPC.s_npcList.IndexOf(npc)].myNpcRef = npc;

                Vector2 pos;
                //Get position from the 3D world and convert it into a 2D position on the screen
                RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    m_myCanvas.transform as RectTransform,

                    Camera.main.WorldToScreenPoint(npc.transform.position),

                    m_myCanvas.worldCamera,

                    out pos
                );

                //Positioning of the Icons
                pos += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset.

                m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyVector2 = new Vector2(pos.x, pos.y);

                // Rotation of the Icons
                Vector3 diff = (Vector3)pos - m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyGameObject.transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyGameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }
        }

        private void LateUpdate()
        {
            // Update all icon arrows; their position and if visible(and active)
            foreach (var icon in m_icons)
            {
                //Set allowed position to keep inside
                Vector3 allowedPos = new Vector3(icon.MyVector2.x, icon.MyVector2.y) - middlePoint;
                allowedPos = Vector3.ClampMagnitude(allowedPos, Screen.height / (2f * screenClampMultiplier));

                //Set gameobjects position to the Vector2 Position it should have.
                icon.MyGameObject.transform.position = middlePoint + allowedPos;
                icon.MyGameObject.SetActive(false);

                if (icon.myNpcRef == null)
                    continue;

                if (icon.myNpcRef != null && icon.myNpcRef.myFeelings.TotalFeelings <= 0)
                    icon.MyGameObject.SetActive(true);
                else
                    continue;

                Vector2 npcPosInUIPos; //Npc position in UI Position

                //Get position from the 3D world and convert it into a 2D position on the screen
                RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    m_myCanvas.transform as RectTransform,
                    Camera.main.WorldToScreenPoint(icon.myNpcRef.transform.position),
                    m_myCanvas.worldCamera,
                    out npcPosInUIPos
                );

                //Positioning of the Icons
                npcPosInUIPos += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset.;

                //Lerp a color by checking deistance between middlepoint of screen and npc position in 2D space
                float distColLerp = Mathf.Clamp(1 - Vector3.Distance(middlePoint, icon.MyVector2) / 1500, 0.2f, 0.8f);

                //if the distance between the npc 2D-Pos and that npcs' icon position is less than "distanceTilArrowPlaced"
                if (Vector3.Distance(npcPosInUIPos, icon.MyGameObject.transform.position) < distanceTilArrowPlaced)
                {
                    if (!showArrowWhenClose)
                    {
                        icon.MyGameObject.SetActive(false);
                        continue;
                    }
                    //Set icon position roughly above the head of the NPC
                    icon.MyGameObject.transform.position = (Vector3)icon.MyVector2 + new Vector3(0, 110);
                    icon.MyGameObject.GetComponent<Image>().color = new Color(0.9f, 0.15f, 0.15f, 1);//Arrow color change
                }
                else
                    icon.MyGameObject.GetComponent<Image>().color = new Color(distColLerp, 0, 1 - distColLerp, 1); //Arrow color change
            }
        }


        private void OnDrawGizmos()
        {
            Camera camera = Camera.main;
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p, 0.1F);

            UnityEditor.Handles.color = Color.red;



        }
    }
}