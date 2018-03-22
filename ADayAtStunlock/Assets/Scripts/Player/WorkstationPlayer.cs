using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkstationPlayer : MonoBehaviour
{
    GameObject player;
    Material playerMat;

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerMat = new Material(player.GetComponent<MeshRenderer>().material);
            player.GetComponent<MeshRenderer>().material.color = Color.green;
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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            player.GetComponent<MeshRenderer>().material.color = playerMat.color;
        }
    }

    void Update ()
    {
		
	}
}
