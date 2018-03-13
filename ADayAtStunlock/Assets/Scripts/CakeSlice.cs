using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
public class CakeSlice : MonoBehaviour {

    [Range(0f, 55f)]
    [SerializeField]
    private float m_AnimationDelay;
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public IEnumerator PlayAnimationWithDelay(float delay, int myindex, int lastindex)
    {

        yield return new WaitForSeconds(delay);
        animator.Play("CakeHopping", 0);
        yield return new WaitForSeconds(1f);

        if (myindex == lastindex)
        {
            
        }

        
    }
}
