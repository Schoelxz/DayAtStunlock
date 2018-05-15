using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseEffect : MonoBehaviour
{
    public Color pulseColor = new Color(1, 1, 1, 0.5f);
    public float pulseSpeed = 2;

    private Image myImage;
    private Image myChildImage;
    private GameObject childObject;

    private Vector2 orgSize;

    private float timer;
    private float timeForther;
    private bool timeForthed = false;

	// Use this for initialization
	void Start ()
    {
        myImage = GetComponent<Image>();

        if (myImage.transform.childCount > 0 && myImage.transform.GetChild(0).GetComponent<Image>())
            myChildImage = transform.GetChild(0).GetComponent<Image>();
        else
        {
            childObject = new GameObject("ImagePulseHolder");
            childObject.transform.SetParent(transform);
            myChildImage = childObject.AddComponent<Image>();
        }
        myChildImage.transform.position = myImage.transform.position;
        myChildImage.color = pulseColor;
        orgSize = new Vector2(myImage.rectTransform.sizeDelta.x, myImage.rectTransform.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(timeForthed)
            timeForther -= Time.deltaTime * pulseSpeed;
        else
            timeForther += Time.deltaTime * pulseSpeed;

        if (timeForther >= 1f)
            timeForthed = !timeForthed;
        else if (timeForther <= 0f)
            timeForthed = !timeForthed;

        myChildImage.sprite = myImage.sprite;

        timer += Time.deltaTime;
        /*if(timer % 2f < 1f)
            myChildImage.rectTransform.sizeDelta += pulseSpeed * Time.deltaTime;
        else
            myChildImage.rectTransform.sizeDelta -= pulseSpeed * Time.deltaTime;*/

        myChildImage.rectTransform.sizeDelta = new Vector2(
            Mathf.Lerp(orgSize.x - 5, orgSize.x + 5, timeForther),
            Mathf.Lerp(orgSize.y - 5, orgSize.y + 5, timeForther));
    }

    //private void OnDisable()
    //{
    //    myImage.rectTransform.sizeDelta = orgSize;
    //}
}
