using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpaceshipMovement : MonoBehaviour {


    DAS.NPC[] aliens;
    float speed;

    Vector3 endPoint;
    Vector3 startPoint;

    RandomEventTrigger random;
    public bool updateSpaceship;
    bool waited;
    bool invoked;
    float pauseTime;

    EffectsManager effects;

    int index;

    // Use this for initialization
    void Start () {
        effects = GameObject.FindObjectOfType<EffectsManager>();
        speed = 10;
        endPoint = new Vector3(50, 10, 0);
        startPoint = transform.position;
        random = FindObjectOfType<RandomEventTrigger>();
        index = 0;
        waited = false;
        invoked = false;
        pauseTime = 2;
    }
	
	// Update is called once per frame
	void Update () {
        if(updateSpaceship)//Updatespaceship is controlled by the RandomEventTrigger script.
        {
            MoveSpaceship();
        }
    }


    public void MoveSpaceship()
    {
        Vector3 offset = new Vector3(0, 8, 0);
        
        if (index < random.aliens.Count)
        {
            if (Vector3.Distance(transform.position, (random.aliens[index].transform.position + offset)) > 0.1)
            {
                transform.position = Vector3.MoveTowards(transform.position, (random.aliens[index].transform.position + offset), speed * DAS.TimeSystem.DeltaTime);
            }
            else if(!invoked)
            {
                Invoke("Waited", pauseTime);
                random.aliens[index].GetComponent<ModelChanger>().ToggleModel();
                random.aliens[index].GetComponent<DAS.NPCMovement>().abducted = true;
                random.aliens[index].GetComponent<NavMeshAgent>().isStopped = true;
                invoked = true;
                effects.PlayEffectAt(transform.position,"SpaceshipLight");
                //Play sound effect here Tomas
            }
            else if(waited == true)
            {
                random.aliens[index].GetComponent<NavMeshAgent>().isStopped = false;
                random.aliens[index].GetComponent<DAS.NPCMovement>().abducted = false;
                index++;
                invoked = false;
                waited = false;
            }
        }
        else
        {
            if ((Vector3.Distance(transform.position, endPoint) > 0.5))
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * DAS.TimeSystem.DeltaTime);
            }
            else
            {
                transform.position = startPoint;
                updateSpaceship = false;
            }
        }
    }

    void Waited()
    {
        waited = true;
    }
}
