using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakeTable : MonoBehaviour {

    public int cakesAvailable;
    Button button;
    Text text;

	// Use this for initialization
	void Start () {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(AddCake);
        text = GetComponentInChildren<Text>();
        text.text = cakesAvailable.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = "Cakes available: " + cakesAvailable.ToString();
    }

    void AddCake()
    {
        cakesAvailable += 1;
    }
}
