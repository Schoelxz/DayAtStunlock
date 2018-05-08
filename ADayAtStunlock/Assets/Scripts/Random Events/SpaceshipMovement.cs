using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpaceshipMovement : MonoBehaviour
{
    public static SpaceshipMovement myInstance;

    DAS.NPC[] aliens;

    [Range(1f, 50f)]
    public float speed = 10f;
    [Range(0f, 5f)]
    public float pauseTime = 1f;

    [HideInInspector]
    public bool updateSpaceship;

    private Vector3 endPoint;
    private Vector3 startPoint;

    private bool waited;
    private bool invoked;

    private RandomEventTrigger random;
    private EffectsManager effects;

    private int index;

    private void Awake()
    {
        if (myInstance == null)
            myInstance = this;
        else
        {
            Debug.LogWarning("Singleton: More than one SpaceshipMovement exists, removing late one.");
            Destroy(this);
        }
    }

    void Start ()
    {
        effects = GameObject.FindObjectOfType<EffectsManager>();
        endPoint = new Vector3(50, 10, 0);
        startPoint = transform.position;
        random = FindObjectOfType<RandomEventTrigger>();
        index = 0;
        waited = false;
        invoked = false;
    }
	
	void Update ()
    {
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
