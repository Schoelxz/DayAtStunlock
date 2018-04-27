using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggler : MonoBehaviour
{
    Button[] buttons;
    Slider[] sliders;
    Image[] images;
    //TextMesh nameDisplay;

    GameObject player;
    int distance;

    void Start()
    {
        //Called in NPC
        //InitButtonToggler();
        images = GetComponentsInChildren<Image>(true);

        Invoke("ShowDisabledImages", 2);
    }

    void Update()
    {
        if (buttons != null && player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < distance)
            {
                //nameDisplay.gameObject.SetActive(true);
                foreach (var b in buttons)
                    b.gameObject.SetActive(true);
            }
            else
                foreach (var b in buttons)
                    b.gameObject.SetActive(false);
        }

        //if (sliders != null && player != null)
        //{
        //    if (Vector3.Distance(player.transform.position, transform.position) < distance)
        //        foreach (var s in sliders)
        //            s.gameObject.SetActive(true);
        //    else
        //        foreach (var s in sliders)
        //            s.gameObject.SetActive(false);
        //}
    }

    private void ShowDisabledImages()
    {
        foreach (var image in images)
        {
            image.enabled = true;
        }
    }

    /// <summary>
    /// Initializes the variables of the button toggler for the NPC.
    /// </summary>
    public void InitButtonToggler()
    {
        buttons = GetComponentsInChildren<Button>(true);
        sliders = GetComponentsInChildren<Slider>(true);
        //nameDisplay = GetComponentInChildren<TextMesh>();
        player = GameObject.Find("Player");
        distance = 5;
    }
}