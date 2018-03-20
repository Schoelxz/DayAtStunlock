using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private GameObject m_playerRef;
    Vector3 cameraPostison;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3 (m_playerRef.transform.position.x, m_playerRef.transform.position.y, m_playerRef.transform.position.z);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.transform.Rotate(this.transform.rotation.x, this.transform.rotation.y + 90, this.transform.rotation.z);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            this.transform.Rotate(Vector3.Lerp(new Vector3 (this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z), new Vector3(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z), );
            Vector3.Lerp();this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z);
        }
        else { }
	}
}
