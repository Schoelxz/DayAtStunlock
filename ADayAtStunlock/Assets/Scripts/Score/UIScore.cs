using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour {

    Text score;

	// Use this for initialization
	void Start () {
        score = gameObject.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        score.text = MoneyManager.moneyEarned.ToString("n0");
	}
}
