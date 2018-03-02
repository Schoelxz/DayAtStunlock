using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcIcons : MonoBehaviour {
    
    public Sprite[] statuses;
    private Image display;
    public bool hasStatus;
    private Button button;
    CakeTable cakeTable;

    // Use this for initialization
    void Start () {
        display = transform.GetChild(0).GetComponentInChildren<Image>();
        hasStatus = false;
        display.enabled = false;
        button = transform.GetChild(0).GetComponentInChildren<Button>();
        button.onClick.AddListener(Clicked);
        cakeTable = FindObjectOfType<CakeTable>();
	}
	
    
    public void DisplayIcon()
    {
        hasStatus = true;
        display.enabled = true;
        display.sprite = statuses[Random.Range(0, statuses.Length)];
    }

    void Clicked()
    {
        print(display.sprite + display.sprite.name);
        print("Button clicked");

        switch (display.sprite.name)
        {
            case ("Confused"):
                display.enabled = false;
                hasStatus = false;

                break;

            case ("Coffee"):

                if (cakeTable.cakesAvailable > 0)
                {
                    display.enabled = false;
                    hasStatus = false;
                    cakeTable.cakesAvailable -= 1;
                }
                else
                {
                    print("Not enough cakes boi");
                }

                break;

            default:
                print("Invalid button name");
                break;
        }
    }

}
