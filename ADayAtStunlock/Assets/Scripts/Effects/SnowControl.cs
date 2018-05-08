using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnowControl : MonoBehaviour
{
    public Vector3 snowBoxSize = Vector3.one;

    private ParticleSystem myParticleSystem;
    public ParticleSystem MyParticleSystem
    {
        get
        {
            return myParticleSystem;
        }

        set
        {
            myParticleSystem = value;
        }
    }

    private void OnDrawGizmos()
    {
        if(MyParticleSystem != null)
        {

            Gizmos.color = Color.gray/2;

            Gizmos.DrawCube(transform.position, MyParticleSystem.shape.scale);

        }
    }

    private void Awake()
    {
        MyParticleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (MyParticleSystem != null)
        {
            var psShape = MyParticleSystem.shape;
            psShape.scale = snowBoxSize;
        }

    }


}
