using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NpcButtons : MonoBehaviour
{
    public class ClickableObject : MonoBehaviour, IPointerDownHandler
    {
        private NpcButtons npcButtonsRef;

        private void Awake()
        {
            npcButtonsRef = transform.GetComponentInParent<NpcButtons>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            npcButtonsRef.MoodButtonPressed();
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

        //m_particleHolder = new GameObject("ParticleEffectHandle");


        //m_particleHolder.transform.parent = m_sliderHolder.GetChild(0);
        //m_particleHolder.transform.position = m_sliderHolder.GetChild(0).transform.position;

        //particle = Instantiate(PrefabHolder.MyInstance.PrefabDictionary["2D-ParticleSliderBurn"]);
        //particle.transform.SetParent(m_particleHolder.transform);
        //particle.transform.localPosition = Vector3.zero;
        //particle.SetActive(false);
    }

    private void Update()
    {
        //m_particleHolder.transform.eulerAngles = new Vector3(m_particleHolder.transform.rotation.x, m_particleHolder.transform.rotation.y, m_npcRef.HappySlider.value * 360);

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
            m_buttonHolder.position = m_holderPos;
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