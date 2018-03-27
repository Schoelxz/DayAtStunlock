using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcButtons : MonoBehaviour {

    private Button[] m_buttons;

    private Canvas m_buttonCanvas;

    private RectTransform m_buttonHolder;
    private Vector2 m_holderPos;

    private Button m_happinessButton;
    private Button m_motivationButton;

    private DAS.NPC m_npcRef;

    // Use this for initialization
    void Start ()
    {
        m_buttons = GetComponentsInChildren<Button>(true);
        // get npc reference
        m_npcRef = GetComponent<DAS.NPC>();
        // get canvas holding buttons
        m_buttonCanvas = transform.GetChild(2).GetComponent<Canvas>();
        // get button holder UI object.
        m_buttonHolder = m_buttonCanvas.transform.GetChild(0).GetComponent<RectTransform>();

        foreach (var b in m_buttons)
        {
            if(b.name == "Happiness")
            {
                m_happinessButton = b;
            }

            if(b.name == "Motivation")
            {
                m_motivationButton = b;
            }
        }

        if(m_motivationButton != null && m_happinessButton != null)
        {
            m_motivationButton.onClick.AddListener(AddMotivation);
            m_happinessButton.onClick.AddListener(AddHappiness);
        }
	}

    private void Update()
    {
        if (Time.timeScale == 0)
            m_buttonCanvas.enabled = false;
        else
            m_buttonCanvas.enabled = true;

        // Make buttons follow NPCs in the screen overlay canvas.
        if (m_npcRef != null && m_happinessButton.enabled == true)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_buttonCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(m_npcRef.transform.position),
                m_buttonCanvas.worldCamera,
                out m_holderPos);

            m_holderPos += new Vector2(Screen.width/2f, Screen.height/2f); // add some fixing offset to the buttons position.
        }
    }
    // Update is called once per frame
    void LateUpdate ()
    {
        // Make buttons follow NPCs in the screen overlay canvas.
        if (m_npcRef != null && m_happinessButton.enabled == true)
        {
            m_buttonHolder.position = m_holderPos;
        }
	}

    void AddMotivation()
    {
        m_npcRef.myFeelings.Motivation++;
    }

    void AddHappiness()
    {
        m_npcRef.myFeelings.Happiness++;
    }
}

