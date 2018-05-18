using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodIconHolder : MonoBehaviour
{
    private static MoodIconHolder s_myInstance;
    public static MoodIconHolder MyInstance
    {
        get
        {
            return s_myInstance;
        }
    }

    [Header("Images for moods")]
    public Sprite iconSpriteHappy;
    public Sprite iconSpriteHappyDisabled;
    public Sprite iconSpriteHappyPressed;
    public Sprite iconSpriteHappyHighlighted;

    public Sprite iconSpriteSad;
    public Sprite iconSpriteSadDisabled;
    public Sprite iconSpriteSadPressed;
    public Sprite iconSpriteSadHighlighted;

    public Sprite iconSpriteMiserable;
    public Sprite iconSpriteMiserableDisabled;
    public Sprite iconSpriteMiserablePressed;
    public Sprite iconSpriteMiserableHighlighted;

    public Sprite iconSpriteConfused;
    public Sprite iconSpriteConfusedDisabled;
    public Sprite iconSpriteConfusedPressed;
    public Sprite iconSpriteConfusedHighlighted;

    public Sprite iconSpriteDemotivated;
    public Sprite iconSpriteDemotivatedDisabled;
    public Sprite iconSpriteDemotivatedPressed;
    public Sprite iconSpriteDemotivatedHighlighted;

    public Sprite iconSpriteAlien;
    public Sprite iconSpriteAlienDisabled;
    public Sprite iconSpriteAlienPressed;
    public Sprite iconSpriteAlienHighlighted;

    public Sprite iconSpriteRepair;
    public Sprite iconSpriteRepairDisabled;
    public Sprite iconSpriteRepairPressed;
    public Sprite iconSpriteRepairHighlighted;

    public Sprite iconSpriteCold;
    public Sprite iconSpriteColdDisabled;
    public Sprite iconSpriteColdPressed;
    public Sprite iconSpriteColdHighlighted;

    public Sprite iconSpriteRepairNoBackground;

    [Header("Other Images")]
    public Sprite iconSpriteShaky;

    private void Awake()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
            Destroy(this);
    }
}
