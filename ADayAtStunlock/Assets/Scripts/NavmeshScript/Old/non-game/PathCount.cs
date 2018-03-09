using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DAS
{
    namespace DBUG
    {
        public class PathCount : MonoBehaviour
        {
            NavMeshAgent navAgent;
            TextMesh textMesh;

            // Use this for initialization
            void Start()
            {
                navAgent = transform.parent.GetComponent<NavMeshAgent>();
                textMesh = GetComponent<TextMesh>();
            }

            // Update is called once per frame
            void Update()
            {
                textMesh.text = navAgent.path.corners.Length.ToString();
            }
        }
    }
}