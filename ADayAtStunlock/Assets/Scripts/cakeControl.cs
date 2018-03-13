using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cakeControl : MonoBehaviour {

    [Range(0, 9)]
    [SerializeField] private int m_Slices;
    [Range(0.0f, 2.0f)]
    [SerializeField] private float m_AnimationDelay;
    private int rotationOffset = 40;

    [SerializeField] private CakeSlice slicePrefab;
    private List<CakeSlice> cakeSlices = new List<CakeSlice>();

    void Start()
    {
        SpawnCake();
    }
    float dt;
    private void Update()
    {
        dt += Time.deltaTime;
        if(dt >= 1)
        {
            dt = 0;
            SpawnCake();
        }
    }

    private void SpawnCake()
    {
        for (int i = 0; i < m_Slices; i++)
        {
            CakeSlice newSlice = Instantiate(slicePrefab, transform);
           

        }

    }
    void StartAnimation()
    {
        //newSlice.transform.Rotate(newSlice.transform.rotation.x, newSlice.transform.rotation.y + rotationOffset * i, newSlice.transform.rotation.z);
        //StartCoroutine(newSlice.PlayAnimationWithDelay(m_AnimationDelay * i, i, m_Slices - 1));
    }
}
