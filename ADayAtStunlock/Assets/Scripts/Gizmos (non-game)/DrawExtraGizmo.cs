#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawExtraGizmo : MonoBehaviour
{
    public bool drawGizmos = true;

    public bool forward = true;
    public bool back = true;
    public bool right = true;
    public bool left = true;

    [Range(1, 10)]
    public int distance = 1;

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {

            if(forward)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(gameObject.transform.position + gameObject.transform.forward * distance, Vector3.one);
            }
            if(back)
            {
                Gizmos.color = Color.blue/2;
                Gizmos.DrawCube(gameObject.transform.position - gameObject.transform.forward * distance, Vector3.one);
            }
            if(right)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(gameObject.transform.position + gameObject.transform.right * distance, Vector3.one);
            }
            if(left)
            {
                Gizmos.color = Color.red/2;
                Gizmos.DrawCube(gameObject.transform.position - gameObject.transform.right * distance, Vector3.one);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawCube(gameObject.transform.position, Vector3.one);
        }
    }
}
#endif