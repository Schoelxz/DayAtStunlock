using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggler : MonoBehaviour
{
    Button[] buttons;
    Image[] images;

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
                foreach (var b in buttons)
                    b.gameObject.SetActive(true);
            else
                foreach (var b in buttons)
                    b.gameObject.SetActive(false);
        }
    }

    private void ShowDisabledImages()
    {
        foreach (var image in images)
            image.enabled = true;
    }

    /// <summary>
    /// Initializes the variables of the button toggler for the NPC.
    /// </summary>
    public void InitButtonToggler()
    {
        buttons = GetComponentsInChildren<Button>(true);
        player = GameObject.Find("Player");
        distance = 5;
    }
}