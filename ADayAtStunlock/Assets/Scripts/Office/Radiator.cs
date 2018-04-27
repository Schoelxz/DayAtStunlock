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
    private Button fixButton;
    private Image fixButtonImage;
    private AudioManager audioManager;

    void Start ()
    {
        fixButton = gameObject.GetComponentInChildren<Button>();
        fixButton.gameObject.AddComponent<ClickableObject>();

        fixButtonImage = gameObject.GetComponentInChildren<Image>();
        fixButtonImage.enabled = false;

        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }
	
	void Update ()
    {
        if (isBroken)
            //Make all npcs in the nearbyNpcs list sad. The list gets updated by the triggerbox on the radiator.
            foreach (var npc in nearbyNpcs)//(var npc in nearbyNpcs)
            {
                npc.myFeelings.Happiness -= 0.03f * DAS.TimeSystem.DeltaTime;
                npc.buttonRef.particle.SetActive(true);
                npc.moodVisualizerRef.ColdMood();
            }
        else
            foreach (var npc in nearbyNpcs)
            {
                npc.buttonRef.particle.SetActive(false);
                npc.moodVisualizerRef.EndStatusEffect();
            }
	}

    public void RadiatorStart()
    {
        isBroken = true; 
        fixButtonImage.enabled = true;

        //Play radiator sound here
        if(audioManager != null)
            //print("Trying to play broken radiator sound");
            audioManager.Play("RadiatorBroken");

        foreach (var npc in nearbyNpcs)
            foreach (var material in npc.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                material.color = Color.blue;

        ArrowPointer.MyInstance.AddObjectToPointAt(gameObject);

        snowControl.ParticleSystem.Play();
    }

    void RadiatorEnd()
    {   
        fixButtonImage.enabled = false;
        isBroken = false;

        //Pause radiator sound here
        //Play fix radiator sound here
        if (audioManager != null)
        {
            //print("Trying to stop broken radiator sound");
            audioManager.Stop("RadiatorBroken");
            audioManager.Play("RadiatorRepair");
        }

        foreach (var npc in nearbyNpcs)
        {
            npc.moodVisualizerRef.EndStatusEffect();
            foreach (var material in npc.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().materials)
                material.color = Color.white;
        }

        ArrowPointer.MyInstance.RemoveObjectToPointAt(gameObject);

        snowControl.ParticleSystem.Stop();
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
                other.GetComponent<DAS.NPC>().buttonRef.particle.SetActive(true);
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
            other.GetComponent<DAS.NPC>().buttonRef.particle.SetActive(false);
            other.GetComponent<DAS.NPC>().moodVisualizerRef.EndStatusEffect();
        }
    }
}
