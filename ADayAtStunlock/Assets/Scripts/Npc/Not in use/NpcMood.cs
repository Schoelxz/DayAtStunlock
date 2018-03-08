using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMood : MonoBehaviour {

    [HideInInspector]
    public float happiness;
    float happinessDecay;

    [HideInInspector]
    public float motivation;
    float motivationDecay;

    [HideInInspector]
    public bool working;

	// Use this for initialization
	void Start () {
        happiness = 100;
        motivation = 100;

        happinessDecay = 5;
        motivationDecay = 10;

        working = true;
	}
	
	// Update is called once per frame
	void Update () {

        MoodDecay();
	}

    void MoodDecay() //Decays the npcs mood
    {
        happiness -= happinessDecay * Time.deltaTime;
        motivation -= motivationDecay * Time.deltaTime;
    }
}
