using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    #region Variables
    public List<GameObject> listOfStuffToPointAt = new List<GameObject>();
    public Sprite typeOfSprite;
    private List<RectTransform> m_listOfArrows = new List<RectTransform>();
    private Canvas m_myCanvas;
    private Vector3 middlePoint;


    #endregion

    private void Start()
    {
        m_myCanvas = GetComponent<Canvas>();

        foreach (var item in listOfStuffToPointAt)
        {
            GameObject temp = new GameObject();
            temp.transform.parent = gameObject.transform;
            Image tempImage;
            temp.AddComponent<RectTransform>();
            tempImage = temp.AddComponent<Image>();
            tempImage.sprite = typeOfSprite;
            tempImage.color = Color.yellow;

            m_listOfArrows.Add(temp.GetComponent<RectTransform>());
        }
    }

    private void Update()
    {
        // Point in the middle of the screen
        middlePoint = new Vector3(Screen.width / 2, Screen.height / 2);

        for (int i = 0; i < listOfStuffToPointAt.Count; i++)
        {
            //2D canvas-positioning of a 3D object-world
            Vector2 stuffPos;

            //Transform 3D-position to a 2D-canvas postiopn
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_myCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(listOfStuffToPointAt[i].transform.position),
                m_myCanvas.worldCamera,
                out stuffPos);

            //Positioning of the Icons
            stuffPos += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset.

            //Set allowed position to keep inside
            Vector3 allowedPos = (Vector3)stuffPos - middlePoint;
            allowedPos = Vector3.ClampMagnitude(allowedPos, Screen.height / (2f * 3f));

            //Set arrow position to the Vector2 Position it should have but clamping it within a .
            m_listOfArrows[i].position = middlePoint + allowedPos;

            //Rotate the arrow towards the object it follows
            RotateArrowTowardsTarget(stuffPos, m_listOfArrows[i]);

            //Hide arrows when destination is reached.
            if ((Vector2)m_listOfArrows[i].position == stuffPos)
                m_listOfArrows[i].gameObject.SetActive(false);
            else
                m_listOfArrows[i].gameObject.SetActive(true);
        }
    }

    private void RotateArrowTowardsTarget(Vector2 targetPos, RectTransform arrow)
    {
        // Rotation of the Icons
        Vector3 diff = (Vector3)targetPos - arrow.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private void LateUpdate()
    {
        
    }

    #region Functions

    #endregion
}
