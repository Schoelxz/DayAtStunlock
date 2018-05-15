using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Radiator : MonoBehaviour
{
    public class ClickableObject : MonoBehaviour, IPointerDownHandler
    {
        private Radiator radiator;

        private void Awake()
        {
            radiator = GetComponentInParent<Radiator>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            radiator.RadiatorEnd();
        }
    }

    public SnowControl snowControl;

    private List<DAS.NPC> nearbyNpcs = new List<DAS.NPC>();
    public bool isBroken = false;
    public Button fixButton;
    private RectTransform buttonRectTransform;
    private Canvas fixCanvas;

    void Start ()
    {
        fixButton.gameObject.AddComponent<ClickableObject>();
        fixCanvas = fixButton.transform.parent.GetComponent<Canvas>();
        buttonRectTransform = fixButton.GetComponent<RectTransform>();

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
                npc.myFeelings.Happiness -= 0.03f * DAS.TimeSystem.DeltaTime;

                if (!npc.buttonRef.particle.GetComponent<ParticleSystem>().isPlaying)
                    npc.buttonRef.particle.GetComponent<ParticleSystem>().Play();
                npc.moodVisualizerRef.ColdMood();
            }
        }
        else
            foreach (var npc in nearbyNpcs)
            {
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
            foreach (var material in npc.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                material.color = Color.blue;

        ArrowPointer.MyInstance.AddObjectToPointAt(gameObject, true);

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
