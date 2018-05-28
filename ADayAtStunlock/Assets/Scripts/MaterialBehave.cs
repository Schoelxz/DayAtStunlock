using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehave : MonoBehaviour
{
    private Renderer[] rend;
    void Start()
    {
        rend = FindObjectsOfType<Renderer>();
    }
    void Update()
    {
        foreach (var item in rend)
        {
            float scaleX = Mathf.Cos(Time.time) * 0.5F + 1;
            float scaleY = Mathf.Sin(Time.time) * 0.5F + 1;
            foreach (var material in item.materials)
            {
                material.mainTextureScale = new Vector2(1, scaleY);
            }
        }
    }
}
