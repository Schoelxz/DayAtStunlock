using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAS
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject m_playerRef;
        private Quaternion m_targetRotation;
        [Header("Camera rotation speed")]
        [Range(0.01f, 1f)]
        [SerializeField]
        private float speed = 0.1f;
        [Header("Indicates how far the camera will rotate")]
        [Range(20f, 90f)]
        [SerializeField]
        private float m_rotationStep = 45f;
        private float m_destinationRotation;

        // Use this for initialization
        void Start()
        {
            // checking if 
            if (m_playerRef == null)
                m_playerRef = FindObjectOfType<PlayerMovement>().gameObject;
            m_destinationRotation = transform.rotation.eulerAngles.y;
        }

        float dt;
        // Update is called once per frame
        void Update()
        {
            //make use of time so here we show/change time
            dt += Time.deltaTime;

            // sets the posison of the of the script user to that of the object in  m_playerRef
            this.transform.position = new Vector3(m_playerRef.transform.position.x, m_playerRef.transform.position.y, m_playerRef.transform.position.z);

            //when you press down Q you turn the camra to the left
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // sets the target rotasion dependent on how rotaded you are allredy with m_rotasionStep  
                // with m_destinationRotasion contaning how mutch you have rotated allready
                m_targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, m_destinationRotation + m_rotationStep, transform.rotation.eulerAngles.z);
                // changeing m_destinationRotation to how rotade the camera is
                m_destinationRotation += m_rotationStep;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                m_targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, m_destinationRotation - m_rotationStep, transform.rotation.eulerAngles.z);
                m_destinationRotation -= m_rotationStep;
            }

            //the acual change that is happening over time
            transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, speed);
        }
    }
}