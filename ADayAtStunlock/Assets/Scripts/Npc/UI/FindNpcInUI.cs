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

            public bool IsWithinScreenBoundaries
            {
                get
                {
                    if  (   
                            myVector2.x > Screen.width  / FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance
                        &&  myVector2.x < Screen.width  - (Screen.width / FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance)
                        &&  myVector2.y > Screen.height / FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance
                        &&  myVector2.y < Screen.height - (Screen.height / FindNpcInUI.myInstance.screenClampMultiplier + myInstance.screenInsideBoundaryAllowance)
                        )
                        return true;
                    else
                        return false;
                }
            }
            public bool IsOutsideScreenBoundaries
            {
                get
                {
                    if (IsWithinScreenBoundaries)
                        return false;

                    if (
                         ((
                            myVector2.x < Screen.width / (FindNpcInUI.myInstance.screenClampMultiplier *2)
                        ||  myVector2.x > Screen.width - (Screen.width / (FindNpcInUI.myInstance.screenClampMultiplier *2))
                          )
                        &&(
                            myVector2.y < Screen.height / (FindNpcInUI.myInstance.screenClampMultiplier *2)
                        ||  myVector2.y > Screen.height - (Screen.height / (FindNpcInUI.myInstance.screenClampMultiplier *2))
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
        [Range(3, 50)]
        [SerializeField]
        private int screenClampMultiplier = 10;
        [Range(0, 150)]
        [SerializeField]
        private int screenInsideBoundaryAllowance = 10;

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

            for (int i = 0; i < DAS.NpcCreator.MaxNumberOfNPCs; i++)
            {
                m_icons.Add(new Icon(new GameObject("NPC Icon " + i)));
                m_icons[i].MyGameObject.transform.parent = transform;
                m_icons[i].MyGameObject.AddComponent<Image>().sprite = imageToDisplay;
            }
        }

        void Update()
        {
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

                m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyVector2 = new Vector2(
                    Mathf.Clamp(pos.x, Screen.width / screenClampMultiplier, Screen.width - (Screen.width / screenClampMultiplier)),
                    Mathf.Clamp(pos.y, Screen.height / screenClampMultiplier, Screen.height - (Screen.height / screenClampMultiplier))
                    );


                // Rotation of the Icons
                Vector3 diff = middlePoint - m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyGameObject.transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyGameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                m_icons[DAS.NPC.s_npcList.IndexOf(npc)].MyGameObject.transform.Rotate(0, 0, 180);
            }
        }

        private void LateUpdate()
        {
            foreach (var icon in m_icons)
            {
                //Set gameobjects position to the Vector2 Position it should have.
                icon.MyGameObject.transform.position = icon.MyVector2;
                icon.MyGameObject.SetActive(false);

                if (icon.IsWithinScreenBoundaries)
                {
                    icon.MyGameObject.SetActive(false);
                }
                else if(icon.myNpcRef != null && icon.myNpcRef.myFeelings.TotalFeelings < 1)
                    icon.MyGameObject.SetActive(true);

                if (icon.IsOutsideScreenBoundaries)
                    icon.MyGameObject.SetActive(false);
            }
        }
    }

}