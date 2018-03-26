﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private GameObject m_playerRef;
  /*  private Vector3 m_startRotasion;
    private bool m_inRotasion;
    private bool m_goingRight;
    private bool m_goingLeft;
    */
    private Quaternion m_targetRotation;
    [Header("this is ther camera rotasion speed")]
    [Range(0.01f, 1f)]
    [SerializeField]
    private float speed = 0.1f;
    private float m_rotationStep = 45f;


    // Use this for initialization
    void Start () {
   
    }

    float dt;
    // Update is called once per frame
    void Update()
    {
        dt += Time.deltaTime;
        this.transform.position = new Vector3(m_playerRef.transform.position.x, m_playerRef.transform.position.y, m_playerRef.transform.position.z);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + m_rotationStep, transform.rotation.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - m_rotationStep, transform.rotation.eulerAngles.z);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, speed);

        /*if (Input.GetKeyDown(KeyCode.Q) && m_inRotasion==false)
        {
            m_goingLeft = true;
            m_inRotasion = true;
        }
           

        
        else if (Input.GetKeyDown(KeyCode.E) && m_inRotasion == false)
        {
            m_goingRight = true;
            m_inRotasion = true;
        }
        if (m_inRotasion == true && m_goingLeft == true)
        {
            this.transform.Rotate(Vector3.Lerp(new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z),
                                                new Vector3(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z),
                                                dt));
        }


        if (m_inRotasion == true && m_goingLeft == true)
        {
            this.transform.Rotate(Vector3.Lerp(new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z),
                                                          new Vector3(this.transform.rotation.x, this.transform.rotation.y + 90, this.transform.rotation.z),
                                                          dt));
        }
        if (dt > 1) { 
        dt = 0;
            m_goingRight = false;
            m_inRotasion = false;
            m_goingLeft = false;
        }*/
    }
}