using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour {

    static public string buttonPressed; //Gets set from player movement inputs

    private float globalCooldown;
    private float globalTimer;

    private float sectionCooldown;
    private float sectionTimer;

    private float vicinityMoodBoost;

    public static bool usingVicinity;

    private Button global;
    private Button section;
    private Button vicinity;

    // Use this for initialization
    void Start() {

        globalCooldown = 30;
        globalTimer = 30;
        sectionCooldown = 1;
        sectionTimer = 10;
        vicinityMoodBoost = 8;

        global = GameObject.Find("Global").GetComponent<Button>();
        section = GameObject.Find("Section").GetComponent<Button>();
        vicinity = GameObject.Find("Vicinity").GetComponent<Button>();

        global.onClick.AddListener(UseGlobal);
        section.onClick.AddListener(UseSection);
        vicinity.onClick.AddListener(UseVicinity);
    }

    // Update is called once per frame
    void Update()
    {

        globalTimer += Time.deltaTime;
        sectionTimer += Time.deltaTime;

        if(usingVicinity == true)
        {
            UseVicinity();
        }
    }

    void UseGlobal()
    {

        if(globalTimer > globalCooldown)
        { 
            print("Trigger global ability");
            foreach (NpcCharacteristics n in Statics.npcs)
            {
            
                n.motivation += 50;
                n.happiness += 50;
            }
            globalTimer = 0;
        }
        else
        {
            print("Global Ability on cooldown");
        }

    }

    void UseSection()
    {
        if (PlayerCharacteristics.currentRoom != null)
        {
            if(sectionTimer > sectionCooldown)
            {
                foreach (NpcCharacteristics n in PlayerCharacteristics.currentRoom.npcs)
                {
                    n.happiness += 30;
                    n.motivation += 30;
                }
                sectionTimer = 0;
            }
        }
    }

    void UseVicinity()
    {
        usingVicinity = true;

        foreach (NpcCharacteristics n in PlayerCharacteristics.vicinityNPCs)
        {
            n.motivation += vicinityMoodBoost * Time.deltaTime;
            n.happiness += vicinityMoodBoost * Time.deltaTime;
        }
    }
}

