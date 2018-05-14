using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseEffect : MonoBehaviour
{
    private Button myButton;
    private Image myImage;

    private Vector2 orgSize;
    private Vector2 pulseSpeed = new Vector2(10f, 10f)/4;

    private float timer;

	// Use this for initialization
	void Start ()
    {
        myButton = GetComponent<Button>();
        myImage = GetComponent<Image>();
        orgSize = new Vector2(myImage.rectTransform.sizeDelta.x, myImage.rectTransform.sizeDelta.y);

    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer % 2f < 1f)
            myImage.rectTransform.sizeDelta += pulseSpeed * Time.deltaTime;
        else
            myImage.rectTransform.sizeDelta -= pulseSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        myImage.rectTransform.sizeDelta = orgSize;
    }
}
