using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    Vector2 up;
    Vector2 right;
    Vector2 upRight;
    Vector2 downRight;
    Rigidbody2D rb2D;

    [Range(0.01f, 2)]
    public float movementSpeed;

    dayTransition dayManager;
    RaycastHit2D hit;
    
    RaycastHit rayhit;
    // Use this for initialization
    void Start () {
        dayManager = FindObjectOfType<dayTransition>();
        rb2D = GetComponent<Rigidbody2D>();
        up = new Vector2(0.0f,20.0f);
        right = new Vector2(20.0f, 0.0f);
        upRight = new Vector2(15.0f, 15.0f);
        downRight = new Vector2(15.0f, -15.0f);
        
        //movementSpeed = 2;
}
	
	// Update is called once per frame
	void Update () {


        if(dayManager.timePassing)
        {
            // 1 input **************************************************************

            if (Input.GetKey("w") == true && Input.GetKey("s") == false && (Input.GetKey("d") == false && Input.GetKey("a") == false|| (Input.GetKey("d") == true && Input.GetKey("a") == true)) )// && ( ManagerTest.Instance.TIME < 0 + 0.1f  ||  ManagerTest.Instance.TIME > (60 / ManagerTest.Instance.BPM - 0.1f) ))
            {
                rb2D.MovePosition(rb2D.position + (up* movementSpeed) *Time.deltaTime);
            }

            if (Input.GetKey("w") == false && Input.GetKey("s") == true && (Input.GetKey("d") == false && Input.GetKey("a") == false || (Input.GetKey("d") == true && Input.GetKey("a") == true))) //&& ( ManagerTest.Instance.TIME < 0 + 0.1f  ||  ManagerTest.Instance.TIME > (60 / ManagerTest.Instance.BPM - 0.1f) ))
            {
                rb2D.MovePosition(rb2D.position - (up * movementSpeed) * Time.deltaTime);
            }

            if ((Input.GetKey("w") == false && Input.GetKey("s") == false || Input.GetKey("w") == true && Input.GetKey("s") == true) && Input.GetKey("d") == true && Input.GetKey("a") == false)// && (ManagerTest.Instance.TIME < 0 + 0.1f || ManagerTest.Instance.TIME > (60 / ManagerTest.Instance.BPM - 0.1f)))
            {
                rb2D.MovePosition(rb2D.position + (right * movementSpeed) * Time.deltaTime);
            }

            if ((Input.GetKey("w") == false && Input.GetKey("s") == false || (Input.GetKey("w") == true) && Input.GetKey("s") == true) && Input.GetKey("d") == false && Input.GetKey("a") == true)// && (ManagerTest.Instance.TIME < 0 + 0.1f || ManagerTest.Instance.TIME > (60 / ManagerTest.Instance.BPM - 0.1f)))
            {
                rb2D.MovePosition(rb2D.position - (right * movementSpeed) * Time.deltaTime);
            }


            // 2 input ****************************************************************************

            if (Input.GetKey("w") == true && Input.GetKey("s") == false && Input.GetKey("d") == true && Input.GetKey("a") == false )
            {
                rb2D.MovePosition(rb2D.position + (upRight * movementSpeed) * Time.deltaTime);
            }

            if (Input.GetKey("w") == true && Input.GetKey("s") == false && Input.GetKey("d") == false && Input.GetKey("a") == true)
            {
                rb2D.MovePosition(rb2D.position - (downRight * movementSpeed) * Time.deltaTime);
            }

            if (Input.GetKey("w") == false && Input.GetKey("s") == true && Input.GetKey("d") == true && Input.GetKey("a") == false)
            {
                rb2D.MovePosition(rb2D.position + (downRight * movementSpeed) * Time.deltaTime);
            }

            if (Input.GetKey("w") == false && Input.GetKey("s") == true && Input.GetKey("d") == false && Input.GetKey("a") == true)
            {
                rb2D.MovePosition(rb2D.position - (upRight * movementSpeed) * Time.deltaTime);
            }
        }

        var mousePos = Input.mousePosition;
        mousePos.z = 5;
        

        if(Input.GetMouseButton(0))
        {

            
            transform.position = Vector3.MoveTowards(transform.position,Camera.main.ScreenToWorldPoint(mousePos), movementSpeed * Time.deltaTime);
        }


        //print(Input.mousePosition);

        
            

    }
}
