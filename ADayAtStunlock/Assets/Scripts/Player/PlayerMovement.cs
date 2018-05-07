using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace DAS
{
    public class PlayerMovement : MonoBehaviour
    {
        //Singleton Behaviour and Script Reference for other classes
        public static PlayerMovement s_myInstance;

        private PlayerRaycast playerRaycast;

        private NavMeshAgent m_agentRef;

        private void Awake()
        {
            if (s_myInstance == null)
                s_myInstance = this;
            else
            {
                Debug.LogError("More than one PlayerMovement class");
                Destroy(this);
            }
        }

        void Start()
        {
            playerRaycast = gameObject.AddComponent<PlayerRaycast>();
            m_agentRef = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (Input.anyKey || Input.GetMouseButtonUp(0))
                CheckInput();
        }

        void CheckInput()
        {
            Vector3 value = new Vector3();
            bool keyboardPressed = false;
            if(Input.GetKey(KeyCode.W))
            {
                value += Camera.main.transform.forward.normalized;
                keyboardPressed = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                value -= Camera.main.transform.right.normalized;
                keyboardPressed = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                value -= Camera.main.transform.forward.normalized;
                keyboardPressed = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                value += Camera.main.transform.right.normalized;
                keyboardPressed = true;
            }

            value = new Vector3(value.x, 0, value.z);
            value *= 2;

            if(keyboardPressed)
            {
                m_agentRef.isStopped = false;
                m_agentRef.destination = value + transform.position;
            }

            Debug.Log(value);

            //Goto where we clicked
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                m_agentRef.isStopped = false;
                m_agentRef.destination = playerRaycast.DirectionVector(transform.position, playerRaycast.hit.point) + transform.position;
                if (m_agentRef.path.corners.Length > 6)
                    m_agentRef.isStopped = true;
                //if(m_agentRef.path.corners)
                if (IsInvoking("StopMovement"))
                    CancelInvoke("StopMovement");
            }
            //For smooth movement stops
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                m_agentRef.destination = transform.position + (playerRaycast.hit.point - transform.position).normalized * 2;
                Invoke("StopMovement", 0.3f);
            }
        }

        void StopMovement()
        {
            m_agentRef.destination = transform.position;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(playerRaycast.hit.point, new Vector3(1, 1, 1));
            Gizmos.color = Color.black;
            Gizmos.DrawCube(playerRaycast.DirectionVector(transform.position, playerRaycast.hit.point) + transform.position, new Vector3(1, 1, 1));
            
        }
        #endif
    }

    public class PlayerRaycast : MonoBehaviour
    {
        int layerMask;
        public RaycastHit hit;
        public Ray ray;

        void Start()
        {
            layerMask = LayerMask.GetMask("Floor");
            hit = new RaycastHit();
        }

        void Update()
        {
            if (Input.anyKey || Input.GetMouseButtonUp(0))
                CheckInput();
        }

        void CheckInput()
        {
            if (Input.GetMouseButton(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, 1000f, layerMask);
            }
        }

        public Vector3 DirectionVector(Vector3 from, Vector3 to)
        {
            Vector3 targetDir = to - from;
            return targetDir.normalized;
        }
    }
}