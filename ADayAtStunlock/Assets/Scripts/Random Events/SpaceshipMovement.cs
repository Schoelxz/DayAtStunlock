using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour {


    DAS.NPC[] aliens;
    float speed;

    Vector3 endPoint;
    Vector3 startPoint;

    RandomEventTrigger random;
    bool updateSpaceship;

    int index;

    // Use this for initialization
    void Start () {

        speed = 5;
        endPoint = new Vector3(50, 10, 0);
        startPoint = transform.position;
        random = FindObjectOfType<RandomEventTrigger>();
        index = 0;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            updateSpaceship = true;
        }

        if(updateSpaceship)
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
            else
            {
                random.aliens[index].GetComponent<ModelChanger>().ToggleModel();
                index++;
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
            }
        }
    }

    
}
