using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodVisualizer : MonoBehaviour
{
    private Image myMoodImage;
    private MoodIconHolder spriteRef;
    private DAS.NPC npcRef;

    public Sprite happySprite;
    public Sprite confusedSprite;
    public Sprite sadSprite;
    public Sprite miserableSprite;
    public Sprite demotivatedSprite;
    public Sprite alienSprite;
    public Sprite coldSprite;

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
        Debug.Assert(npcRef.moodVisualizerRef);

        myMoodImage.sprite = spriteRef.iconSpriteHappy;

        //Set default sprites on all sprites.
        happySprite = spriteRef.iconSpriteHappy;
        confusedSprite = spriteRef.iconSpriteConfused;
        sadSprite = spriteRef.iconSpriteSad;
        alienSprite = spriteRef.iconSpriteAlien;
        miserableSprite = spriteRef.iconSpriteMiserable;
        demotivatedSprite = spriteRef.iconSpriteDemotivated;
        coldSprite = spriteRef.iconSpriteCold;
    }

    private void Update()
    {
        //Gives special icons, therefore ignoring the other standard effects by returning when statuseffect is true.
        if (hasStatusEffect)
        {
            myMoodImage.sprite = coldSprite;
            return;
        }

        //Sets npc mood image to an image representing their mood.
        if (npcRef.GetComponent<ModelChanger>().isAlien)
            myMoodImage.sprite = alienSprite;
        else if (npcRef.myFeelings.TotalFeelings < 0.1f)
            myMoodImage.sprite = miserableSprite;
        else if (npcRef.myFeelings.Happiness < 0.45f)
            myMoodImage.sprite = sadSprite;
        else if (npcRef.myFeelings.Motivation < 0.45f)
            myMoodImage.sprite = demotivatedSprite;
        else if (npcRef.myFeelings.TotalFeelings < 0.4f)
            myMoodImage.sprite = confusedSprite;
        else
            myMoodImage.sprite = happySprite;
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
        myMoodImage.sprite = coldSprite;
    }

    public void EndStatusEffect()
    {
        hasStatusEffect = false;
    }

}
