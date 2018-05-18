using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseEffect : MonoBehaviour
{
    public Color pulseColor = new Color(1, 1, 1, 0.5f);
    [Range(1f, 40f)]
    public float pulseSpeed = 2;
    [Range(1, 40)]
    public int pulseLength = 5;

    private Image myImage;
    private Image myChildImage;
    private GameObject childObject;

    private Button myButton;
    private RectTransform myButtonRectTransform;

    private Vector2 orgSize;
    private Vector2 buttonOrgSize;

    private float timer;
    private float timeForther;
    private bool timeForthed = false;

	// Use this for initialization
	void Start ()
    {
        myButton = GetComponent<Button>();
        myButtonRectTransform = myButton.transform as RectTransform;
        buttonOrgSize = myButtonRectTransform.sizeDelta;
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
        if (myButtonRectTransform.sizeDelta.x < buttonOrgSize.x)
        {
            myButtonRectTransform.sizeDelta += new Vector2(buttonOrgSize.x/10, buttonOrgSize.y/10);
            myChildImage.rectTransform.sizeDelta += new Vector2(buttonOrgSize.x / 10, buttonOrgSize.y / 10);
            return;
        }

        if (timeForthed)
            timeForther -= Time.deltaTime * pulseSpeed;
        else
            timeForther += Time.deltaTime * pulseSpeed;

        if (timeForther >= 1f)
            timeForthed = true;
        else if (timeForther <= 0f)
            timeForthed = false;

        myChildImage.sprite = myImage.sprite;

        timer += Time.deltaTime;

        myChildImage.rectTransform.sizeDelta = new Vector2(
            Mathf.Lerp(orgSize.x - pulseLength, orgSize.x + pulseLength, timeForther),
            Mathf.Lerp(orgSize.y - pulseLength, orgSize.y + pulseLength, timeForther));

        myChildImage.transform.position = myImage.transform.position;
    }

    private void OnDisable()
    {
        myButtonRectTransform.sizeDelta = Vector2.zero;
        myChildImage.rectTransform.sizeDelta = Vector2.zero;
    }
}
