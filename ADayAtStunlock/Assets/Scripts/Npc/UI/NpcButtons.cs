using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NpcButtons : MonoBehaviour
{
    public class ClickableObject : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
    {
        private NpcButtons npcButtonsRef;

        private void Awake()
        {
            npcButtonsRef = transform.GetComponentInParent<NpcButtons>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            npcButtonsRef.m_npcRef.moodVisualizerRef.happySprite = MoodIconHolder.MyInstance.iconSpriteHappyPressed;
            npcButtonsRef.m_npcRef.moodVisualizerRef.sadSprite = MoodIconHolder.MyInstance.iconSpriteSadPressed;
            npcButtonsRef.m_npcRef.moodVisualizerRef.demotivatedSprite = MoodIconHolder.MyInstance.iconSpriteDemotivatedPressed;
            npcButtonsRef.m_npcRef.moodVisualizerRef.confusedSprite = MoodIconHolder.MyInstance.iconSpriteConfusedPressed;
            npcButtonsRef.m_npcRef.moodVisualizerRef.miserableSprite = MoodIconHolder.MyInstance.iconSpriteMiserablePressed;
            npcButtonsRef.m_npcRef.moodVisualizerRef.alienSprite = MoodIconHolder.MyInstance.iconSpriteAlienPressed;
            npcButtonsRef.m_npcRef.moodVisualizerRef.coldSprite = MoodIconHolder.MyInstance.iconSpriteColdPressed;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            npcButtonsRef.m_npcRef.moodVisualizerRef.happySprite = MoodIconHolder.MyInstance.iconSpriteHappyHighlighted;
            npcButtonsRef.m_npcRef.moodVisualizerRef.sadSprite = MoodIconHolder.MyInstance.iconSpriteSadHighlighted;
            npcButtonsRef.m_npcRef.moodVisualizerRef.demotivatedSprite = MoodIconHolder.MyInstance.iconSpriteDemotivatedHighlighted;
            npcButtonsRef.m_npcRef.moodVisualizerRef.confusedSprite = MoodIconHolder.MyInstance.iconSpriteConfusedHighlighted;
            npcButtonsRef.m_npcRef.moodVisualizerRef.miserableSprite = MoodIconHolder.MyInstance.iconSpriteMiserableHighlighted;
            npcButtonsRef.m_npcRef.moodVisualizerRef.alienSprite = MoodIconHolder.MyInstance.iconSpriteAlienHighlighted;
            npcButtonsRef.m_npcRef.moodVisualizerRef.coldSprite = MoodIconHolder.MyInstance.iconSpriteColdHighlighted;

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                npcButtonsRef.m_npcRef.moodVisualizerRef.happySprite = MoodIconHolder.MyInstance.iconSpriteHappyPressed;
                npcButtonsRef.m_npcRef.moodVisualizerRef.sadSprite = MoodIconHolder.MyInstance.iconSpriteSadPressed;
                npcButtonsRef.m_npcRef.moodVisualizerRef.demotivatedSprite = MoodIconHolder.MyInstance.iconSpriteDemotivatedPressed;
                npcButtonsRef.m_npcRef.moodVisualizerRef.confusedSprite = MoodIconHolder.MyInstance.iconSpriteConfusedPressed;
                npcButtonsRef.m_npcRef.moodVisualizerRef.miserableSprite = MoodIconHolder.MyInstance.iconSpriteMiserablePressed;
                npcButtonsRef.m_npcRef.moodVisualizerRef.alienSprite = MoodIconHolder.MyInstance.iconSpriteAlienPressed;
                npcButtonsRef.m_npcRef.moodVisualizerRef.coldSprite = MoodIconHolder.MyInstance.iconSpriteColdPressed;
                npcButtonsRef.MoodButtonPressed();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            npcButtonsRef.m_npcRef.moodVisualizerRef.happySprite = MoodIconHolder.MyInstance.iconSpriteHappy;
            npcButtonsRef.m_npcRef.moodVisualizerRef.sadSprite = MoodIconHolder.MyInstance.iconSpriteSad;
            npcButtonsRef.m_npcRef.moodVisualizerRef.demotivatedSprite = MoodIconHolder.MyInstance.iconSpriteDemotivated;
            npcButtonsRef.m_npcRef.moodVisualizerRef.confusedSprite = MoodIconHolder.MyInstance.iconSpriteConfused;
            npcButtonsRef.m_npcRef.moodVisualizerRef.miserableSprite = MoodIconHolder.MyInstance.iconSpriteMiserable;
            npcButtonsRef.m_npcRef.moodVisualizerRef.alienSprite = MoodIconHolder.MyInstance.iconSpriteAlien;
            npcButtonsRef.m_npcRef.moodVisualizerRef.coldSprite = MoodIconHolder.MyInstance.iconSpriteCold;

            npcButtonsRef.MoodButtonPressed();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            npcButtonsRef.m_npcRef.moodVisualizerRef.happySprite = MoodIconHolder.MyInstance.iconSpriteHappy;
            npcButtonsRef.m_npcRef.moodVisualizerRef.sadSprite = MoodIconHolder.MyInstance.iconSpriteSad;
            npcButtonsRef.m_npcRef.moodVisualizerRef.demotivatedSprite = MoodIconHolder.MyInstance.iconSpriteDemotivated;
            npcButtonsRef.m_npcRef.moodVisualizerRef.confusedSprite = MoodIconHolder.MyInstance.iconSpriteConfused;
            npcButtonsRef.m_npcRef.moodVisualizerRef.miserableSprite = MoodIconHolder.MyInstance.iconSpriteMiserable;
            npcButtonsRef.m_npcRef.moodVisualizerRef.alienSprite = MoodIconHolder.MyInstance.iconSpriteAlien;
            npcButtonsRef.m_npcRef.moodVisualizerRef.coldSprite = MoodIconHolder.MyInstance.iconSpriteCold;
        }
    }
    private Canvas m_buttonCanvas;

    private RectTransform m_buttonHolder;
    private RectTransform m_sliderHolder;
    private Vector2 m_holderPos;

    private GameObject m_particleHolder;
    public GameObject particle;

    private Button npcButton;

    private DAS.NPC m_npcRef;

    private EffectsManager effectsManager;

    #region Unity Standard Functions
    void Start ()
    {
        npcButton = gameObject.GetComponentInChildren<Button>();
        npcButton.gameObject.AddComponent<ClickableObject>();
        //Burning Particle
        m_particleHolder = Instantiate(PrefabHolder.MyInstance.PrefabDictionary["2D-Particle"]);
        particle = m_particleHolder.transform.GetChild(0).gameObject;
        m_particleHolder.transform.SetParent(m_sliderHolder.GetChild(0));
        m_particleHolder.transform.localEulerAngles = new Vector3(0,0,0);
        m_particleHolder.transform.localPosition = new Vector3(m_sliderHolder.sizeDelta.x / 2, -m_sliderHolder.sizeDelta.y / 2, 0);
        
    }

    private void Update()
    {
        m_particleHolder.transform.localEulerAngles = new Vector3(0,0, m_npcRef.HappySlider.value * -360);

        if (Time.timeScale == 0)
            m_buttonCanvas.enabled = false;
        else
            m_buttonCanvas.enabled = true;

        // Make buttons follow NPCs in the screen overlay canvas.
        if (m_npcRef != null && m_buttonCanvas.enabled == true)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_buttonCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(m_npcRef.transform.position),
                m_buttonCanvas.worldCamera,
                out m_holderPos);

            m_holderPos += new Vector2(Screen.width / 2f, Screen.height / 2f + 100f); // add some fixing offset to the buttons position.
        }
    }

    void LateUpdate ()
    {
        // Make buttons follow NPCs in the screen overlay canvas.
        if (m_npcRef != null && m_buttonCanvas.enabled == true)
        {
            Vector2 newOffset = new Vector2(m_buttonHolder.sizeDelta.x/1.2f, -50);
            m_buttonHolder.position = m_holderPos + newOffset;
            //m_sliderHolder.position = m_holderPos;
            //m_sliderHolder.position -= new Vector3(0, 60);
            //m_sliderHolder.GetComponent<Material>().renderQueue = 0;
        }
	}
    #endregion

    /// <summary>
    /// Initializes UI elements for the NPC to control.
    /// </summary>
    public void InitNpcButtons()
    {
        // get npc reference
        m_npcRef = GetComponent<DAS.NPC>();
        // get canvas holding buttons
        m_buttonCanvas = transform.GetChild(2).GetComponent<Canvas>();
        // get button holder UI object.
        m_buttonHolder = m_buttonCanvas.transform.GetChild(0).GetComponent<RectTransform>();
        // get slider holder UI object.
        m_sliderHolder = transform.GetChild(3).GetComponent<RectTransform>();
        // find and get effects manager reference.
        effectsManager = FindObjectOfType<EffectsManager>();
    }

    protected void MoodButtonPressed()
    {
        m_npcRef.myFeelings.Happiness++;
        m_npcRef.myFeelings.Motivation++;
        effectsManager.PlayEffectAt(transform.position, new Vector3(0, 3, 0), "HappinessParticle");
    }
}