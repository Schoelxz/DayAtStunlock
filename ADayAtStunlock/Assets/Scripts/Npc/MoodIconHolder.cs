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
    public Sprite iconSpriteSad;
    public Sprite iconSpriteMiserable;
    public Sprite iconSpriteConfused;
    public Sprite iconSpriteDemotivated;
    public Sprite iconSpriteAlien;

    [Header("Other Images")]
    public Sprite iconSpriteRepair;
    public Sprite iconSpriteCold;
    public Sprite iconSpriteShaky;

    private void Awake()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
            Destroy(this);
    }
}
