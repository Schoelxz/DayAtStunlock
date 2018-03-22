using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour {
    private Animator m_animator;
    private NavMeshAgent agentRef;
    // Use this for initialization
    void Start ()
    {
        //Find Animator
        m_animator = GetComponentInChildren<Animator>();
        agentRef= GetComponent<NavMeshAgent>();

}
	
	// Update is called once per frame
	void Update () {
        //Movement Animation
        m_animator.SetFloat("MoveSpeed", agentRef.velocity.magnitude);
    }
    
}
