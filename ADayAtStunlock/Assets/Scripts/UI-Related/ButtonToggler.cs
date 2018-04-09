using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggler : MonoBehaviour
{
    Button[] buttons;
    Slider[] sliders;
    TextMesh nameDisplay;

    GameObject player;
    int distance;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>(true);
        sliders = GetComponentsInChildren<Slider>(true);
        nameDisplay = GetComponentInChildren<TextMesh>(true);
        player = GameObject.Find("Player");
        distance = 5;
    }

    void Update()
    {
        if (buttons != null && player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < distance)
            {
                nameDisplay.gameObject.SetActive(true);
                foreach (var b in buttons)
                {
                    b.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var b in buttons)
                {
                    b.gameObject.SetActive(false);
                }
            }
        }

        if (sliders != null && player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < distance)
            {
                foreach (var s in sliders)
                {
                    s.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var s in sliders)
                {
                    s.gameObject.SetActive(false);
                }
            }
        }
    }
}