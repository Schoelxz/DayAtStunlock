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
            gameObject.AddComponent<PlayerRaycast>();
            m_agentRef = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (Input.anyKey || Input.GetMouseButtonUp(0))
                CheckInput();
        }

        void CheckInput()
        {
            //Goto where we clicked
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
                m_agentRef.destination = PlayerRaycast.hit.point;
            //For smooth movement stops
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                m_agentRef.destination = transform.position + (PlayerRaycast.hit.point - transform.position).normalized * 2;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(PlayerRaycast.hit.point, new Vector3(1, 1, 1));
        }
        #endif
    }

    public class PlayerRaycast : MonoBehaviour
    {
        int layerMask;
        public static RaycastHit hit;
        public static Ray ray;

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
    }
}