using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkstationPlayer : MonoBehaviour
{
    GameObject player;
    //Never used?
    //BoxCollider myBoxCollider;
    NavMeshAgent agentRef;
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

    private void Update()
    {
        //Cheats start
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    foreach (DAS.NPC npc in DAS.NPC.s_npcList)
        //    {
        //        npc.myFeelings.Happiness = 1f;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    foreach (DAS.NPC npc in DAS.NPC.s_npcList)
        //    {
        //        npc.myFeelings.Motivation = 1f;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    foreach (DAS.NPC npc in DAS.NPC.s_npcList)
        //    {
        //        npc.myFeelings.Happiness = 0f;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    foreach (DAS.NPC npc in DAS.NPC.s_npcList)
        //    {
        //        npc.myFeelings.Motivation = 0f;
        //    }
        //}
        //Cheats end
    }
    
    void FixedUpdate()
    {
        if (isWorking)
        {
            
            MoneyManager.GenerateMoney();
            //Unhide particle emission
            particleSystemMoneyGained.gameObject.SetActive(true);
            
            

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
                player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, Time.fixedDeltaTime * 3);
            }

        }
        else
        {
            //Hide particle emission
            particleSystemMoneyGained.gameObject.SetActive(false);
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
            {
                totalHappiness += npc.myFeelings.Happiness;
            }
        }
        float resultMotivation;
        float resultHappiness;

        resultMotivation = (totalMotivation / DAS.NPC.s_npcList.Count);
        resultHappiness  = 1 - (totalHappiness / DAS.NPC.s_npcList.Count);
        
        emissionMoneyGained.rateOverTime = new ParticleSystem.MinMaxCurve(resultMotivation * 4f);
        emissionMoneyLost.rateOverTime = new ParticleSystem.MinMaxCurve(resultHappiness * 4f);

    }
    
}
