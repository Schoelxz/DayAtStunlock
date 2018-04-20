using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//min
public class TooltipMover : MonoBehaviour {
    private Vector3 m_offset;
    private Vector3 m_tempOffset;

    public Vector2 cerserPos;

    void Start()
    {
        m_offset.x = this.GetComponent<RectTransform>().sizeDelta.x /2;
        m_offset.y = this.GetComponent<RectTransform>().sizeDelta.y /2 *-1;

    }

    // Update is called once per frame
    void Update ()
    {
        m_tempOffset = m_offset;
        cerserPos = Input.mousePosition;
        //top right
        if (cerserPos.y >= Screen.height/2
            &&
            cerserPos.x <= Screen.width /2)
        {
            
        } 
        // lower right
        else if (cerserPos.y <= Screen.height / 2
            &&
            cerserPos.x <= Screen.width / 2)
        {
            m_tempOffset.y = m_tempOffset.y * -1;
        }
        //top left
        else if (cerserPos.y >= Screen.height / 2 
            &&
            cerserPos.x >= Screen.width / 2)
        {
            m_tempOffset.x = m_tempOffset.x * -1;
            
        }
        //lower left
        else if (cerserPos.y <= Screen.height / 2 
                &&
                cerserPos.x >= Screen.width / 2)
        {
                m_tempOffset.x = m_tempOffset.x * -1;
                m_tempOffset.y = m_tempOffset.y * -1;
        }
       

     transform.position = Input.mousePosition + m_tempOffset;
    }
}
