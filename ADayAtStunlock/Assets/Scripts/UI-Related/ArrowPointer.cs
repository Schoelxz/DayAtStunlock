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
        for (int i = 0; i < listOfStuffToPointAt.Count; i++)
        {
            //2D canvas-positioning of a 3D object-world
            Vector2 pos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_myCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(listOfStuffToPointAt[i].transform.position),
                m_myCanvas.worldCamera,
                out pos);

            //Positioning of the Icons
            pos += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset.

            m_listOfArrows[i].position = pos;
        }

    }

    private void LateUpdate()
    {
        
    }

    #region Functions

    #endregion
}
