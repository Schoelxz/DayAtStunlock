using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnowControl : MonoBehaviour
{
    public Vector3 snowBoxSize = Vector3.one;

    private new ParticleSystem particleSystem;
    public ParticleSystem ParticleSystem
    {
        get
        {
            return particleSystem;
        }

        set
        {
            particleSystem = value;
        }
    }

    private void OnDrawGizmos()
    {
        if(ParticleSystem != null)
        {

            Gizmos.color = Color.gray/2;

            Gizmos.DrawCube(transform.position, ParticleSystem.shape.scale);

        }
    }

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ParticleSystem != null)
        {
            var psShape = ParticleSystem.shape;
            psShape.scale = snowBoxSize;
        }

    }


}
