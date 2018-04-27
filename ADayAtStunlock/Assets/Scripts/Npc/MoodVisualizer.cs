using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodVisualizer : MonoBehaviour
{
    private Image myMoodImage;
    private MoodIconHolder spriteRef;
    private DAS.NPC npcRef;

    private bool hasStatusEffect = false;

	// Use this for initialization
	void Start ()
    {
        myMoodImage = GetComponent<Image>();
        spriteRef = MoodIconHolder.MyInstance;
        npcRef = GetComponentInParent<DAS.NPC>();

        Debug.Assert(myMoodImage);
        Debug.Assert(spriteRef);
        Debug.Assert(npcRef);

        npcRef.moodVisualizerRef = this;
        myMoodImage.sprite = spriteRef.iconSpriteHappy;
    }

    private void Update()
    {
        //Gives special icons, therefore ignoring the other standard effects by returning when statuseffect is true.
        if (hasStatusEffect)
            return;

        //Sets npc mood image to an image representing their mood.
        if (npcRef.myFeelings.TotalFeelings < 0.1f)
            myMoodImage.sprite = spriteRef.iconSpriteMiserable;
        else if (npcRef.myFeelings.Happiness < 0.45f)
            myMoodImage.sprite = spriteRef.iconSpriteSad;
        else if (npcRef.myFeelings.Motivation < 0.45f)
            myMoodImage.sprite = spriteRef.iconSpriteDemotivated;
        else if (npcRef.myFeelings.TotalFeelings < 0.4f)
            myMoodImage.sprite = spriteRef.iconSpriteConfused;
        else
            myMoodImage.sprite = spriteRef.iconSpriteHappy;
    }

    /*
     * Hard to use the status effect with multiple events as they will conflict (making status effect dissapear when they actually have a status effect).
     * Therefore only use it for cold moods for now.
     * Otherwise use mutliple bool checks for every event to prioritize which to use.
     */

    // Maybe not use this since demotivated is a thing aswell.
    private void TrainShakey()
    {
        hasStatusEffect = true;
        myMoodImage.sprite = spriteRef.iconSpriteShaky;
    }
    // Private aswell.
    private void AlienMood()
    {
        hasStatusEffect = true;
        myMoodImage.sprite = spriteRef.iconSpriteAlien;
    }

    public void ColdMood()
    {
        hasStatusEffect = true;
        myMoodImage.sprite = spriteRef.iconSpriteCold;
    }

    public void EndStatusEffect()
    {
        hasStatusEffect = false;
    }

}
