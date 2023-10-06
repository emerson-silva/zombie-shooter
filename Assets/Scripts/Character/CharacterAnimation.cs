using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string ANIM_IS_MOVING = "isMoving";
    private const string ANIM_IS_MOVING_MAGNITUDE = "isMovingMagnitude";
    private const string ANIM_TRIGGER_ATTACK = "attack";
    private const string ANIM_TRIGGER_DEATH = "death";

    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void MoveAnimation(bool isMoving)
    {
        animator.SetBool(ANIM_IS_MOVING, isMoving);
    }

    public void MoveAnimation(float magnitude)
    {
        animator.SetFloat(ANIM_IS_MOVING_MAGNITUDE, magnitude);
    }

    public void MeleeAttack()
    {
        animator.SetTrigger(ANIM_TRIGGER_ATTACK);
    }

    public void Death()
    {
        animator.ResetTrigger(ANIM_TRIGGER_ATTACK);
        animator.SetTrigger(ANIM_TRIGGER_DEATH);
    }
}
