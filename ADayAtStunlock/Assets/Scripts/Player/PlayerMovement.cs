using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {
    
    Rigidbody rb;
    //Singleton Behaviour
    private static PlayerMovement s_myInstance;

    public static GameObject s_playerGoRef;

    [Range(2, 10)]
    public float movementSpeed;
    private Vector3 m_moveHere;
    private NavMeshAgent m_agentRef;

    private void Awake()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
        {
            Debug.LogError("More than one PlayerMovement class");
            Destroy(this); }
    }

    void Start()
    {
        s_playerGoRef = gameObject;
        m_agentRef = GetComponent<NavMeshAgent>();   
    }
    // Update is called once per frame
    void Update ()
    {
        if ( Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            m_moveHere = PlayerRaycast.hit.point;
            
            m_agentRef.destination = PlayerRaycast.hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(PlayerRaycast.hit.point,new Vector3(1,1,1));
    }
}
//PlayerRaycast.mouseStart == true &&
