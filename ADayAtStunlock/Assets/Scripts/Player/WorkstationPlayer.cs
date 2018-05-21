using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkstationPlayer : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agentRef;
    [SerializeField]
    private ParticleSystem particleSystemMoneyGained;
    [SerializeField]
    private ParticleSystem particleSystemMoneyLost;

    private bool isWorking = false;

    void Start()
    {
        //myBoxCollider = GetComponentInChildren<BoxCollider>();
        player      = GameObject.FindGameObjectWithTag("Player");
        agentRef    = player.GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
            isWorking = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
            isWorking = false;
    }
    
    void FixedUpdate()
    {
        if (!MoneyManager.IsEarningMoney && !particleSystemMoneyLost.isPlaying)
            particleSystemMoneyLost.Play();
        else if (MoneyManager.IsEarningMoney)
            particleSystemMoneyLost.Stop();

        if (isWorking)
        {
            //Pulls the player towards the workstation.
            PullPlayer();

            MoneyManager.GenerateMoney();

            //Unhide particle emission
            if(!particleSystemMoneyGained.isPlaying && MoneyManager.IsEarningMoney)
                particleSystemMoneyGained.Play();
        }
        else
        {
            //Hide particle emission
            if (particleSystemMoneyGained.isPlaying)
                particleSystemMoneyGained.Stop();
        }

        //Update particle emission based on how many workers are working and how motivated they are.
        //Less motivated & non working people = more red dollar signs, less green dollar signs
        //more motivated & working people = more green dollar signs, less red dollar signs
        ParticleSystem.EmissionModule emissionMoneyGained = particleSystemMoneyGained.emission;
        ParticleSystem.EmissionModule emissionMoneyLost = particleSystemMoneyLost.emission;

        float totalMotivation = 0;
        float totalHappiness = 0;

        foreach (DAS.NPC npc in DAS.NPC.s_npcList)
        {
            if (npc.moveRef.IsCurrentlyWorking)
            {
                totalMotivation += npc.myFeelings.Motivation;
                totalHappiness += npc.myFeelings.Happiness;
            }
            else
                totalHappiness += npc.myFeelings.Happiness;
        }

        float resultMotivation;
        float resultHappiness;

        resultMotivation = (totalMotivation / DAS.NPC.s_npcList.Count);
        resultHappiness  = 1 - (totalHappiness / DAS.NPC.s_npcList.Count);
        
        emissionMoneyGained.rateOverTime = new ParticleSystem.MinMaxCurve(resultMotivation * 4f);
        emissionMoneyLost.rateOverTime = new ParticleSystem.MinMaxCurve(resultHappiness * 4f);
    }

    private void PullPlayer()
    {
        // targetDir is our work seats position + forward its own direction (roughly our desk's position)
        Vector3 targetDir = (transform.position + (transform.forward) - transform.position);
        // How fast we turn every frame.
        float step = 1.2f * Time.fixedDeltaTime;
        // Our complete rotate towards direction.
        Vector3 newDir = Vector3.RotateTowards(player.transform.forward, targetDir, step, 0.0F);

        // Apply the rotate towards.
        player.transform.rotation = Quaternion.LookRotation(newDir);

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            agentRef.isStopped = false;
        }
        else //if the player is not trying to move.
        {
            // Stop our agent from fidgeting in his seat and smoothly pulls the player towards the workbench.
            agentRef.isStopped = true;
            agentRef.velocity = Vector3.zero;
            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, Time.fixedDeltaTime * 3);
        }

        // Draw ray towards the position we're rotating towards.
        //Debug.DrawRay(transform.position, newDir, Color.red, 3);
    }
}
