using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMovement : MonoBehaviour
{
    public GameObject destination;

    private NavMeshAgent navAgent;
    private float dt;
    private bool pathIsStraight = false;
    private bool isDestinationReached = false;

	void Start ()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        navAgent.destination = destination.transform.position;
    }

	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    navAgent.isStopped = !navAgent.isStopped;
        //    Debug.Log(navAgent.isStopped);
        //}

        dt += Time.deltaTime;
        if (dt < 0.1f)
            return;
        dt = 0;

        if(navAgent.remainingDistance <= 2f && !navAgent.isStopped || isDestinationReached)
        {
            isDestinationReached = true;
            navAgent.isStopped = true;
        }
        else
        {
            navAgent.destination = destination.transform.position;
        }
    }

    private bool IsStraightPath()
    {
        if (navAgent.path.corners.Length <= 2)
        {
            pathIsStraight = true;
        }
        else
            pathIsStraight = false;

        return pathIsStraight;
    }
}
