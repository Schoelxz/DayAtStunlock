using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {
    
    Rigidbody rb;

    [Range(2, 10)]
    public float movementSpeed;
    private Vector3 m_moveHere;
    private NavMeshAgent m_agentRef;

    void Start()
    {
        m_agentRef = GetComponent<NavMeshAgent>();   
    }
    // Update is called once per frame
    void Update ()
    {
        if ( Input.GetMouseButton(0))
        {
            m_moveHere = PlayerRaycast.hit.point;
            m_agentRef.destination = PlayerRaycast.hit.point;
        }
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawCube(PlayerRaycast.hit.point,new Vector3(1,1,1));
    }/*/
}
//PlayerRaycast.mouseStart == true &&
