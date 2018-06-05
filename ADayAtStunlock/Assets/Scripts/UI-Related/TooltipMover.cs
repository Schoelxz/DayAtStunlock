using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//min
public class TooltipMover : MonoBehaviour {
    private Vector3 m_offset;
    private Vector3 m_tempOffset;

    public Vector2 cursorPos;
    RectTransform tooltipTransform;
    [SerializeField] RectTransform tooltipText;
    

    void Start()
    {
        m_offset.x = this.GetComponent<RectTransform>().sizeDelta.x /2;
        m_offset.y = this.GetComponent<RectTransform>().sizeDelta.y /2 *-1;
        tooltipTransform = GetComponent<RectTransform>();
        if(tooltipText == null)
        {
            tooltipText = GetComponent<Text>().gameObject.GetComponent<RectTransform>();
        }
        
    }

    // Update is called once per frame
    void Update ()
    {
        tooltipTransform.pivot = new Vector2(Mathf.Clamp(Mathf.Sign((Screen.width / 2 - Input.mousePosition.x)* -1), 0, 1), Mathf.Clamp(Mathf.Sign((Screen.height / 2 - Input.mousePosition.y) * -1), 0, 1));

        if (Input.GetKey(KeyCode.Escape))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            tooltipText.GetComponent<Text>().text = "";

        }

        //m_tempOffset = m_offset;
        //cursorPos = Input.mousePosition;
        ////top right
        //if (cursorPos.y >= Screen.height/2
        //    &&
        //    cursorPos.x <= Screen.width /2)
        //{

        //} 
        //// lower right
        //else if (cursorPos.y <= Screen.height / 2
        //    &&
        //    cursorPos.x <= Screen.width / 2)
        //{
        //    m_tempOffset.y = m_tempOffset.y * -1;
        //}
        ////top left
        //else if (cursorPos.y >= Screen.height / 2 
        //    &&
        //    cursorPos.x >= Screen.width / 2)
        //{
        //    m_tempOffset.x = m_tempOffset.x * -1;

        //}
        ////lower left
        //else if (cursorPos.y <= Screen.height / 2 
        //        &&
        //        cursorPos.x >= Screen.width / 2)
        //{
        //        m_tempOffset.x = m_tempOffset.x * -1;
        //        m_tempOffset.y = m_tempOffset.y * -1;
        //}


        transform.position = Input.mousePosition + m_tempOffset;
    }
}
