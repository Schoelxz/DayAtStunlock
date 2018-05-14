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

        private Vector3 movementValue = new Vector3();

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

        private void Start()
        {
            playerRaycast = gameObject.AddComponent<PlayerRaycast>();
            m_agentRef = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (Input.anyKey || Input.GetMouseButtonUp(0))
                CheckInput();
        }

        private void CheckInput()
        {
            KeyboardMovement();

            MouseMovement();
        }

        private void MouseMovement()
        {
            //Goto where we clicked
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                m_agentRef.isStopped = false;
                m_agentRef.destination = playerRaycast.DirectionVector(transform.position, playerRaycast.hit.point) + transform.position;
                if (m_agentRef.path.corners.Length > 6)
                    m_agentRef.isStopped = true;
            }
            //For smooth movement stops
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                m_agentRef.destination = transform.position + (playerRaycast.hit.point - transform.position).normalized * 2;
        }

        private void KeyboardMovement()
        {
            bool keyboardPressed = false;
            movementValue = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movementValue += Camera.main.transform.forward.normalized;
                keyboardPressed = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementValue -= Camera.main.transform.right.normalized;
                keyboardPressed = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementValue -= Camera.main.transform.forward.normalized;
                keyboardPressed = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementValue += Camera.main.transform.right.normalized;
                keyboardPressed = true;
            }

            movementValue = new Vector3(movementValue.x, 0, movementValue.z);
            movementValue.Normalize();
            movementValue *= 2;

            if (keyboardPressed)
            {
                m_agentRef.isStopped = false;
                m_agentRef.destination = movementValue + transform.position;
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (playerRaycast == null)
                return;
            //Draw player clicked location
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(playerRaycast.hit.point, new Vector3(1, 1, 1));
            //Draw player clicked direction location
            Gizmos.color = Color.black;
            Gizmos.DrawCube(playerRaycast.DirectionVector(transform.position, playerRaycast.hit.point) + transform.position, new Vector3(1, 1, 1));
            //Draw player keyboard movement location
            Gizmos.color = Color.red;
            Gizmos.DrawCube(movementValue + transform.position, new Vector3(1, 1, 1));
        }
        #endif
    }

    public class PlayerRaycast : MonoBehaviour
    {
        private int layerMask;
        public RaycastHit hit;
        public Ray ray;

        private void Start()
        {
            layerMask = LayerMask.GetMask("Floor");
            hit = new RaycastHit();
        }

        private void Update()
        {
            if (Input.anyKey || Input.GetMouseButtonUp(0))
                CheckInput();
        }

        private void CheckInput()
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