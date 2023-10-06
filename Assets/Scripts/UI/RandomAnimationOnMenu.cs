using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationOnMenu : MonoBehaviour
{
    private string ANIN_RANDOM_ANIM = "randomAnimation";
    private string ANIN_TRIGGER_ANIM = "triggerAnimation";
    private Animator animator;
    public float minValue;
    public float maxValue;
    public float timeBetweenAnimations;
    private float nextAnimationTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        nextAnimationTime += timeBetweenAnimations;
    }

    void Update()
    {
        if (nextAnimationTime <= Time.timeSinceLevelLoad)
        {
            nextAnimationTime = Time.timeSinceLevelLoad + timeBetweenAnimations;
            animator.SetFloat(ANIN_RANDOM_ANIM, Random.Range(minValue, maxValue));
            animator.SetTrigger(ANIN_TRIGGER_ANIM);
        }
    }
}
