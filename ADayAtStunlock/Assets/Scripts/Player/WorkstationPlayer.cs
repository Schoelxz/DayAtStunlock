using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkstationPlayer : MonoBehaviour
{
    GameObject player;
    BoxCollider myBoxCollider;
    NavMeshAgent agentRef;

    private bool isWorking = false;

	void Start ()
    {
        myBoxCollider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        agentRef = player.GetComponent<NavMeshAgent>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            isWorking = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            isWorking = false;
        }
    }

    void FixedUpdate ()
    {
        if (isWorking)
        {
            MoneyManager.GenerateMoney();

            // Stop our agent from fidgeting in his seat.
            //agentRef.isStopped = true;
            // targetDir is our work seats position + forward its own direction (roughly our desk's position)
            Vector3 targetDir = (transform.position + (transform.forward) - transform.position);
            // How fast we turn every frame.
            float step = 1.2f * Time.fixedDeltaTime;
            // Our complete rotate towards direction.
            Vector3 newDir = Vector3.RotateTowards(player.transform.forward, targetDir, step, 0.0F);

            // Draw ray towards the position we're rotating towards.
            //Debug.DrawRay(transform.position, newDir, Color.red, 3);

            // Apply the rotate towards.
            player.transform.rotation = Quaternion.LookRotation(newDir);

            if (Input.GetMouseButton(0))
            {
                agentRef.isStopped = false;
            }
            else
            {
                agentRef.isStopped = true;
                agentRef.velocity = Vector3.zero;
                player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, Time.fixedDeltaTime*3);
            }
            
        }

    }
}
