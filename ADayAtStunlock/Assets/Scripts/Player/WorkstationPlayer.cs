using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkstationPlayer : MonoBehaviour
{
    GameObject player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            print("Trigger Player");
            //MoneyManager.GenerateMoney();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == player)
        {
            print("Collision Player");

            MoneyManager.GenerateMoney();
        }
    }

    void Update ()
    {
		
	}
}
