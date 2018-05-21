using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Radiator : MonoBehaviour
{
    public class ClickableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Radiator radiator;
        public int distanceToPlayer = 5;


        private void Awake()
        {
            radiator = GetComponentInParent<Radiator>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!radiator.fixButton.interactable)
                return;
            radiator.fixButton.image.sprite = MoodIconHolder.MyInstance.iconSpriteRepairPressed;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!radiator.fixButton.interactable)
                return;
            radiator.fixButton.image.sprite = MoodIconHolder.MyInstance.iconSpriteRepair;
            if (Vector3.Distance(radiator.transform.position, DAS.PlayerMovement.s_myInstance.transform.position) < distanceToPlayer)
            {
                radiator.RadiatorEnd();
            }
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!radiator.fixButton.interactable)
                return;
            radiator.fixButton.image.sprite = MoodIconHolder.MyInstance.iconSpriteRepairHighlighted;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!radiator.fixButton.interactable)
                return;
            radiator.fixButton.image.sprite = MoodIconHolder.MyInstance.iconSpriteRepair;
        }

        private void Update()
        {
            //Check distance from player and disable button if player is too far away
            if (Vector3.Distance(radiator.transform.position, DAS.PlayerMovement.s_myInstance.transform.position) > distanceToPlayer &&
                radiator.fixButton.image.sprite != MoodIconHolder.MyInstance.iconSpriteRepairDisabled)
            {
                radiator.fixButton.image.sprite = MoodIconHolder.MyInstance.iconSpriteRepairDisabled;
                radiator.fixButton.interactable = false;
            }
            else if(Vector3.Distance(radiator.transform.position, DAS.PlayerMovement.s_myInstance.transform.position) < distanceToPlayer)
            {
                if(radiator.fixButton.image.sprite == MoodIconHolder.MyInstance.iconSpriteRepairDisabled)
                    radiator.fixButton.image.sprite = MoodIconHolder.MyInstance.iconSpriteRepair;
                radiator.fixButton.interactable = true;
            }
        }
    }

    public SnowControl snowControl;

    private List<DAS.NPC> nearbyNpcs = new List<DAS.NPC>();
    public bool isBroken = false;
    public Button fixButton;
    private RectTransform buttonRectTransform;
    private Canvas fixCanvas;
    private Radiator m_radiator;
    private DAS.PlayerMovement m_playerMovement;


    void Start ()
    {
        fixButton.gameObject.AddComponent<ClickableObject>();
        fixCanvas = fixButton.transform.parent.GetComponent<Canvas>();
        buttonRectTransform = fixButton.GetComponent<RectTransform>();
        m_radiator = GetComponent<Radiator>();
        m_playerMovement = FindObjectOfType<DAS.PlayerMovement>();

        fixButton.gameObject.SetActive(false);
    }
	
	void Update ()
    {
        if (isBroken)
        {
            Vector2 m_fixPos = new Vector2(0, 0);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(fixCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(gameObject.transform.position),
                fixCanvas.worldCamera,
                out m_fixPos);

            m_fixPos += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset to the buttons position.
            Vector2 extraOffset = new Vector2(-75, 110);

            buttonRectTransform.position = m_fixPos + extraOffset;
            

            //Make all npcs in the nearbyNpcs list sad. The list gets updated by the triggerbox on the radiator.
            foreach (var npc in nearbyNpcs)//(var npc in nearbyNpcs)
            {
                if (npc == null)
                {
                    Debug.LogWarning("Npc went null: ", npc);
                    continue;
                }
                npc.myFeelings.Happiness -= 0.03f * DAS.TimeSystem.DeltaTime;

                if (!npc.buttonRef.particle.GetComponent<ParticleSystem>().isPlaying)
                    npc.buttonRef.particle.GetComponent<ParticleSystem>().Play();
                npc.moodVisualizerRef.ColdMood();
            }
        }
        else
            foreach (var npc in nearbyNpcs)
            {
                if(npc == null)
                {
                    Debug.LogWarning("Npc went null: ", npc);
                    continue;
                }
                else if(npc.moodVisualizerRef == null)
                    continue;

                if (npc.buttonRef.particle.GetComponent<ParticleSystem>().isPlaying)
                    npc.buttonRef.particle.GetComponent<ParticleSystem>().Stop();
                npc.moodVisualizerRef.EndStatusEffect();
            }
	}

    public void RadiatorStart()
    {
        isBroken = true;
        //fixButtonImage.enabled = true;

        fixButton.gameObject.SetActive(true);

        //Play radiator sound here
        if (AudioManager.instance != null)
            //print("Trying to play broken radiator sound");
            AudioManager.instance.PlaySound("RadiatorBroken", gameObject);

        foreach (var npc in nearbyNpcs)
        {
            if (npc == null)
            {
                Debug.LogWarning("Npc went null: ", npc);
                continue;
            }

            foreach (var material in npc.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                material.color = Color.blue;
        }

        ArrowPointer.ObjectPointer data = new ArrowPointer.ObjectPointer
        {
            colorOfArrow = Color.cyan,
            colorOfText = Color.cyan,
            extraSprite = MoodIconHolder.MyInstance.iconSpriteRepairNoBackground,
            extraFunction = true
        };
        ArrowPointer.MyInstance.AddObjectToPointAt(gameObject, data);

        snowControl.MyParticleSystem.Play();
    }

    void RadiatorEnd()
    {   
        //fixButtonImage.enabled = false;
        isBroken = false;

        fixButton.gameObject.SetActive(false);

        //Pause radiator sound here
        //Play fix radiator sound here
        if (AudioManager.instance != null)
        {
            //print("Trying to stop broken radiator sound");
            AudioManager.instance.StopSound("RadiatorBroken", gameObject);
            AudioManager.instance.PlaySound("RadiatorRepair", gameObject);
        }

        foreach (var npc in nearbyNpcs)
        {
            npc.moodVisualizerRef.EndStatusEffect();
            foreach (var material in npc.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                material.color = Color.white;
        }

        ArrowPointer.MyInstance.RemoveObjectToPointAt(gameObject);

        snowControl.MyParticleSystem.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            nearbyNpcs.Add(other.GetComponent<DAS.NPC>());

            if (isBroken)
            {
                foreach (var material in other.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                    material.color = Color.blue;
                
                other.GetComponent<DAS.NPC>().buttonRef.particle.GetComponent<ParticleSystem>().Play();
                other.GetComponent<DAS.NPC>().moodVisualizerRef.ColdMood();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            nearbyNpcs.Remove(other.GetComponent<DAS.NPC>());

            foreach (var material in other.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                material.color = Color.white;


            other.GetComponent<DAS.NPC>().buttonRef.particle.GetComponent<ParticleSystem>().Stop();
            other.GetComponent<DAS.NPC>().moodVisualizerRef.EndStatusEffect();
        }
    }
}
