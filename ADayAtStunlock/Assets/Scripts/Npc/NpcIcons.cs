using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcIcons : MonoBehaviour {
    
    public Sprite[] statuses;
    //[Range(1, 1)]
    //[SerializeField] private int maxStatuses;
    private Image display;
    public bool hasStatus;
    [Header("Attachments")]
    [Tooltip("The parent of the icon/button")]
    [SerializeField] private GameObject ButtonHolder;
    [Tooltip("The slider that indicates happiness")]
    [SerializeField] private Slider HappySlider;
    [Tooltip("The button child of the buttonHolder(is assigned through buttonHolder)")]
    [SerializeField] private Button button;
    CakeTable cakeTable;

    // Use this for initialization
    void Start () {

        if(ButtonHolder == null)
        {
            Debug.LogError("Missing Attachment: " + ButtonHolder.name);
        }
        else
        {
            button = ButtonHolder.transform.GetChild(0).GetComponentInChildren<Button>();
            button.onClick.AddListener(Clicked);
        }
        if (HappySlider == null)
        {
            Debug.LogError("Missing Attachment: " + HappySlider.name);
        }

        display = transform.GetChild(0).GetComponentInChildren<Image>();
        hasStatus = false;
        display.enabled = false;

        
        cakeTable = FindObjectOfType<CakeTable>();
	}
	
    
    public void DisplayIcon()
    {
        if (hasStatus == false)
        {
            hasStatus = true;
            display.enabled = true;
            display.sprite = statuses[Random.Range(0, statuses.Length)];
        }
    }

    void Clicked()
    {
        

        switch (display.sprite.name)
        {
            case ("confused_icon"):
                display.enabled = false;
                hasStatus = false;

                break;

            case ("coffee_icon"):

                if (cakeTable.cakesAvailable > 0)
                {
                    display.enabled = false;
                    hasStatus = false;
                    cakeTable.cakesAvailable -= 1;
                }
                else
                {
                    print("Not enough cake");
                }

                break;

            default:
                print("Error");
                break;
        }
    }

}
